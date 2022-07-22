namespace DotGeo.Extensions.Extensions;

internal static class StringExtensions
{
    public static void ThrowIfNullOrWhiteSpace(this string value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{name} is null or whitespace");
    }
}