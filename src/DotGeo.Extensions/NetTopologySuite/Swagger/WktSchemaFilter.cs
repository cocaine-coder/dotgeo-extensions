namespace DotGeo.Extensions.NetTopologySuite.Swagger;

public class WktSchemaFilter : SchemaFilterBase
{
    protected override Dictionary<Type, SchemaChangeType> Mapper =>
        new Dictionary<Type, SchemaChangeType>()
        {
            [typeof(Point)] = new("string","POINT (1 2)"),
            [typeof(MultiPoint)] = new("string", "MULTIPOINT (1 2, 1 3)"),
            [typeof(LinearRing)] = new("string", "LINEARRING (1 0, 1 1, 0 0, 1 0)"),
            [typeof(LineString)] = new("string", "LINESTRING (1 0, 1 1)"),
            [typeof(Polygon)] = new("string", "POLYGON ((1 0,1 1, 0 0 , 1 0))"),
            [typeof(MultiPolygon)] = new("string", "MULTIPOLYGON (((1 0,1 1, 0 0 , 1 0)))"),
            [typeof(Geometry)] = new("string", "POINT (1 2)"),
        };
}