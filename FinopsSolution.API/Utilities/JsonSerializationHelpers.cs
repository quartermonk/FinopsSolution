using System.Text.Json;

namespace FinopsSolution.API.Utilities;

public static class JsonSerializationHelpers
{
    public static readonly JsonSerializerOptions WebDefaults = new(JsonSerializerDefaults.Web);
}
