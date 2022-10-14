using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Features;
using NetTopologySuite.Geometries;

namespace NetTopologySuite.Swagger
{
    public static class GeometryMappers
    {
        public static Dictionary<Type,string> GeometryWktMapper =>
            new Dictionary<Type, string>()
            {
                [typeof(Point)] = "POINT (1 2)",
                [typeof(MultiPoint)] = "MULTIPOINT (1 2, 1 3)",
                [typeof(LinearRing)] = "LINEARRING (1 0, 1 1, 0 0, 1 0)",
                [typeof(LineString)] = "LINESTRING (1 0, 1 1)",
                [typeof(Polygon)] = "POLYGON ((1 0,1 1, 0 0 , 1 0))",
                [typeof(MultiPolygon)] = "MULTIPOLYGON (((1 0,1 1, 0 0 , 1 0)))",
                [typeof(Geometry)] = "POINT (1 2)",
            };

        public static Dictionary<Type, string> GeometryGeojsonMapper =>
            new Dictionary<Type, string>()
            {
                [typeof(Point)] = "{\"type\":\"Point\",\"coordinates\":[120,31]}",

                [typeof(MultiPoint)] = "{\"type\":\"MultiPoint\",\"coordinates\":[[120,31],[120,31.5]]}",

                [typeof(LineString)] = "{\"type\":\"LineString\",\"coordinates\":[[120,30],[120,31],[121,31]]}",

                [typeof(LinearRing)] = "{\"type\":\"LineString\",\"coordinates\":[[120,30],[120,31],[121,31],[120,30]]}",

                [typeof(MultiLineString)] = "{ \"type\": \"MultiLineString\",\"coordinates\":" +
                    "[[[120, 30],[121, 30],[121, 31], [120, 31]],[[120, 30],[119,30]]]}",

                [typeof(Polygon)] = "{ \"type\": \"Polygon\",\"coordinates\":" +
                    "[[[120, 30],[121, 30],[121, 31], [120, 31], [120, 30]]]}",

                [typeof(MultiPolygon)] = "{\"type\": \"MultiPolygon\",\"coordinates\":" +
                    "[[[[109,30], [115,30], [115,32], [109,32], [109,30]]]," +
                    "[[[112,26], [116,26], [116,29], [112,29], [112,26]]]]}",

                [typeof(Geometry)] = "{\"type\":\"LineString\",\"coordinates\":[[120,30],[120,31],[121,30]]}",

                [typeof(FeatureCollection)] = "{\"type\": \"FeatureCollection\",\"features\": " +
                    "[{\"type\":\"Feature\",\"properties\":{},\"geometry\":" +
                    "{\"type\":\"Point\",\"coordinates\":[105,31]}}]}"
            };
    }
}