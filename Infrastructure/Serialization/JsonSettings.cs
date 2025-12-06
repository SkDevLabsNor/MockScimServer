using System.Text.Json;
using System.Text.Json.Serialization;

namespace MockScimServer.Infrastructure.Serialization;

public static class JsonSettings
{
    public static readonly JsonSerializerOptions ApiOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
