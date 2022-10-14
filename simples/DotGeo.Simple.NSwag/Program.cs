using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using NetTopologySuite.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options=>options.JsonSerializerOptions.Converters.Add(new GeoJsonConverterFactory()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument(settings =>
{
    settings.TypeMappers.AddGeometry(GeoSerializeType.Geojson);
});

var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi3();

app.MapPost("line", (LineString line) =>
{
    Results.Ok(line);
});
app.MapControllers();

app.Run();

[ApiController]
[Route("api/line")]
public class LineController : ControllerBase
{
    [HttpPost]
    public LineString Create(LineString line) => line;
}