namespace DotGeo.Extensions.NetTopologySuite.Swagger;

public class SchemaChangeType
{
    public SchemaChangeType(string type, string value)
    {
        Type = type;
        Value = value;
    }

    public string Type { get; }
    public string Value { get; }
}

public abstract class SchemaFilterBase : ISchemaFilter
{
    protected abstract Dictionary<Type, SchemaChangeType> Mapper { get; }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!Mapper.TryGetValue(context.Type, out var schemaChangeType))
            return;
        schema.Properties.Clear();
        schema.Type = schemaChangeType.Type;
        schema.Example = new OpenApiString(schemaChangeType.Value);
    }
}