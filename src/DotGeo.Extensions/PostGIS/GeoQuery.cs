using DotGeo.Extensions.Extensions;

namespace DotGeo.Extensions.PostGIS;

public interface IGeoQuery
{
    /// <summary>
    /// get mapbox-vector-tiles buffer
    /// 获取mapbox矢量切片buffer
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="table">name of table</param>
    /// <param name="geomColumn">name of geometry column</param>
    /// <param name="z">{z}</param>
    /// <param name="x">{x}</param>
    /// <param name="y">{y}</param>
    /// <param name="columns">eg: id,name</param>
    /// <param name="filter">eg: name like 'scc%'</param>
    /// <returns></returns>
    Task<byte[]> GetMvtBufferAsync(string connectionString, string table, string geomColumn, int z, int x, int y,
        string? columns, string? filter);

    /// <summary>
    /// get GeoBuf
    /// 获取GeoBuf格式的数据
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="table">name of table</param>
    /// <param name="geomColumn">name of geometry column</param>
    /// <param name="columns">eg: id,name</param>
    /// <param name="filter">eg: name like 'scc%'</param>
    /// <returns></returns>
    Task<byte[]> GetGeoBufferAsync(string connectionString, string table, string geomColumn, 
        string? columns, string? filter);

    /// <summary>
    /// get GeoJson
    /// 获取GeoJson
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="table">name of table</param>
    /// <param name="geomColumn">name of geometry column</param>
    /// <param name="idColumn">name of id column</param>
    /// <param name="columns">eg: id,name</param>
    /// <param name="filter">eg: name like 'scc%'</param>
    /// <returns></returns>
    Task<object> GetGeoJsonAsync(string connectionString, string table, string geomColumn, string? idColumn,
        string? columns, string? filter);
}

public class GeoQuery : IGeoQuery
{
    public async Task<byte[]> GetMvtBufferAsync(
        string connectionString, 
        string table, 
        string geomColumn, 
        int z, 
        int x,
        int y, 
        string? columns,
        string? filter)
    {
        connectionString.ThrowIfNullOrWhiteSpace(nameof(connectionString));
        table.ThrowIfNullOrWhiteSpace(nameof(table));
        geomColumn.ThrowIfNullOrWhiteSpace(nameof(geomColumn));
        
        var sql = $@"
            WITH mvt_geom as (
              SELECT
                ST_AsMVTGeom (
                  ST_Transform({geomColumn}, 3857),
                  ST_TileEnvelope({z}, {x}, {y})
                ) as geom
                {(columns != null ? $",{columns}" : "")}
              FROM
                {table},
                (SELECT ST_SRID({geomColumn}) AS srid FROM {table} LIMIT 1) a
              WHERE
                ST_Intersects(
                  {geomColumn},
                  ST_Transform(ST_TileEnvelope({z}, {x}, {y}),srid)
                ) {(filter != null ? $" AND {filter}" : "")}
            )
            SELECT ST_AsMVT(mvt_geom.*, '{table}', 4096, 'geom') AS mvt from mvt_geom;";

        return await QuerySingleValueAsync<byte[]>(connectionString, sql);
    }

    public async Task<byte[]> GetGeoBufferAsync(
        string connectionString, 
        string table, 
        string geomColumn,
        string? columns, 
        string? filter)
    {
        connectionString.ThrowIfNullOrWhiteSpace(nameof(connectionString));
        table.ThrowIfNullOrWhiteSpace(nameof(table));
        geomColumn.ThrowIfNullOrWhiteSpace(nameof(geomColumn));
        
        var sql = $@"SELECT ST_AsGeobuf(q, 'geom')
                          FROM (SELECT
                                  ST_Transform({geomColumn}, 4326) as geom
                                  {(columns != null ? $", {columns}" : "")}
                                FROM
                                  {table}
                                {(filter != null ? $"WHERE {filter}" : "")}
                          ) as q;";

        return await QuerySingleValueAsync<byte[]>(connectionString, sql);
    }

    public async Task<object> GetGeoJsonAsync(
        string connectionString, 
        string table, 
        string geomColumn, 
        string? idColumn,
        string? columns,
        string? filter)
    {
        connectionString.ThrowIfNullOrWhiteSpace(nameof(connectionString));
        table.ThrowIfNullOrWhiteSpace(nameof(table));
        geomColumn.ThrowIfNullOrWhiteSpace(nameof(geomColumn));
        
        var sql = $@"
            SELECT
                row_to_json(fc)
            FROM (
                SELECT
                    'FeatureCollection' AS type
                    ,array_to_json(array_agg(f)) AS features
                FROM (
                    SELECT
                        'feature' AS type
                        {(idColumn != null ? $",{idColumn} as id" : "")}
                        , ST_AsGeoJSON({geomColumn})::json as geometry  --geom表中的空间字段
                        , (
                            SELECT
                                row_to_json(t)
                            FROM (
                                SELECT
                                   {columns ?? ""}
                                ) AS t
                            ) AS properties
                    FROM {table} 
                    {(filter != null ? $"WHERE {filter}" : "")} ) AS f
               ) AS fc";

        return await QuerySingleValueAsync<object>(connectionString, sql);
    }

    private static async Task<T> QuerySingleValueAsync<T>(string connectionString, string sql, Array? parameters = null)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(sql, conn);

        if (parameters != null)
            cmd.Parameters.AddRange(parameters);

        await using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();

        return (T) reader[0];
    }
}