namespace DotGeo.Extensions.NetTopologySuite.Swagger;

public class GeoJsonSchemaFilter : SchemaFilterBase
{
    protected override Dictionary<Type, SchemaChangeType> Mapper =>
        new Dictionary<Type, SchemaChangeType>()
        {
            [typeof(Point)] = new(nameof(Point),
                "{\"type\":\"Point\",\"coordinates\":[120,31]}"),

            [typeof(MultiPoint)] = new(nameof(MultiPoint),
                "{\"type\":\"MultiPoint\",\"coordinates\":[[120,31],[120,31.5]]}"),

            [typeof(LineString)] = new(nameof(LineString),
                "{\"type\":\"LineString\",\"coordinates\":[[120,30],[120,31],[121,31]]}"),

            [typeof(LinearRing)] = new(nameof(LinearRing),
                "{\"type\":\"LineString\",\"coordinates\":[[120,30],[120,31],[121,31],[120,30]]}"),

            [typeof(MultiLineString)] = new(nameof(MultiLineString),
                "{ \"type\": \"MultiLineString\",\"coordinates\":" +
                "[[[120, 30],[121, 30],[121, 31], [120, 31]],[[120, 30],[119,30]]]}"),

            [typeof(Polygon)] = new(nameof(Polygon),
                "{ \"type\": \"Polygon\",\"coordinates\":" +
                "[[[120, 30],[121, 30],[121, 31], [120, 31], [120, 30]]]}"),

            [typeof(MultiPolygon)] = new(nameof(MultiPolygon),
                "{\"type\": \"MultiPolygon\",\"coordinates\":" +
                "[[[[109,30], [115,30], [115,32], [109,32], [109,30]]]," +
                 "[[[112,26], [116,26], [116,29], [112,29], [112,26]]]]}"),

            [typeof(Geometry)] = new(nameof(Geometry),
                "{\"type\":\"LineString\",\"coordinates\":[[120,30],[120,31],[121,30]]}"),

            [typeof(FeatureCollection)] = new(nameof(FeatureCollection),
                "{\"type\": \"FeatureCollection\",\"features\": " +
                "[{\"type\":\"Feature\",\"properties\":{},\"geometry\":" +
                "{\"type\":\"Point\",\"coordinates\":[105,31]}}]}")
        };
}