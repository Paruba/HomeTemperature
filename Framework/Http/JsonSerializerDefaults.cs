using System.Text.Json;

namespace Boiler.Mobile.Framework.Http;

public static class JsonSerializerDefaults
{
    public static readonly JsonSerializerOptions DefaultSettings = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}