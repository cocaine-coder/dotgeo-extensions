namespace DotGeo.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddGeoQuery(this IServiceCollection services) =>
        services.AddSingleton<IGeoQuery, GeoQuery>();

    public static WebApplication UseGeoQuery(this WebApplication app, string connectionStringTemplate,
        string? prefix = null,Action<RouteHandlerBuilder>? builderAction = null)
    {
        if (string.IsNullOrWhiteSpace(connectionStringTemplate))
            throw new ArgumentException("connection_string_template must set");

        if (string.IsNullOrWhiteSpace(prefix))
            prefix = "geo";

        var routeHandlerBuilders = new RouteHandlerBuilder[3];

        routeHandlerBuilders[0] = app.MapGet
        (prefix + "/mvt/{database}/{table}/{geomColumn}/{z:int}/{x:int}/{y:int}.pbf", async (
            string database,
            string table,
            string geomColumn,
            int z,
            int x,
            int y,
            string? columns,
            string? filter,
            [FromServices] IGeoQuery geoQuery
        ) =>
        {
            var connectionString = string.Format(connectionStringTemplate, database);
            var bytes = await geoQuery
                .GetMvtBufferAsync(connectionString, table, geomColumn, z, x, y, columns, filter);
            return Results.Bytes(bytes, "application/x-protobuf");
        });

        routeHandlerBuilders[1] = app.MapGet
        (prefix + "/geobuf/{database}/{table}/{geomColumn}.pbf", async (
            string database,
            string table,
            string geomColumn,
            string? columns,
            string? filter,
            [FromServices] IGeoQuery geoQuery
        ) =>
        {
            var connectionString = string.Format(connectionStringTemplate, database);
            var bytes = await geoQuery.GetGeoBufferAsync(connectionString, table, geomColumn, columns, filter);
            return Results.Bytes(bytes, "application/x-protobuf");
        });

        routeHandlerBuilders[2] = app.MapGet
        (prefix + "/geojson/{database}/{table}/{geomColumn}", async (
            string database,
            string table,
            string geomColumn,
            string? idColumn,
            string? columns,
            string? filter,
            [FromServices] IGeoQuery geoQuery
        ) =>
        {
            var connectionString = string.Format(connectionStringTemplate, database);
            var geoJson =
                await geoQuery.GetGeoJsonAsync(connectionString, table, geomColumn, idColumn, columns, filter);
            return Results.Ok(geoJson);
        });

        if (builderAction == null) return app;
        foreach (var builder in routeHandlerBuilders)
        {
            builderAction(builder);
        }

        return app;
    }
}