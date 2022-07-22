using Microsoft.AspNetCore.Mvc;

namespace DotGeo.Simple.Controllers;

public enum LayerType
{
    mvt,
    geobuf,
    geojson,
}

[ApiController]
[Route("api/map")]
public class MapController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(LayerType type,string database,string table,string geomColumn)
    {
        var layer_url =
            $"http://{HttpContext.Request.Host.Host}:{HttpContext.Request.Host.Port}/geo/{type.ToString()}/{database}/{table}/{geomColumn}";
        
        switch (type)
        {
            case LayerType.mvt:
                layer_url += "/{z}/{x}/{y}.pbf";
                break;
            case LayerType.geobuf:
                layer_url += ".pbf";
                break;
            case LayerType.geojson:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
        return Redirect($"http://{HttpContext.Request.Host.Host}:{HttpContext.Request.Host.Port}/mapbox.html?layer_url={layer_url}&layer_type={type.ToString()}&layer_name={table}");
    }
}