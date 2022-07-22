using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;

namespace DotGeo.Simple.Controllers;

[ApiController]
[Route("api/geojson/geometry")]
public class GeometryController : ControllerBase
{
    [HttpPost("point")]
    public ActionResult<Point> Create(Point point)
    {
        return Ok(point);
    }
    
    [HttpPost("line")]
    public ActionResult<LineString> Create(LineString line)
    {
        return Ok(line);
    }

    [HttpPost("polygon")]
    public ActionResult<Point> Create(Polygon polygon)
    {
        return Ok(polygon);
    }
    
    [HttpPost("polygons")]
    public ActionResult<MultiPolygon> Create(MultiPolygon polygons)
    {
        return Ok(polygons);
    }
    
    [HttpPost("feature_collection")]
    public ActionResult<FeatureCollection> Create(FeatureCollection featureCollection)
    {
        return Ok(featureCollection);
    } 
}