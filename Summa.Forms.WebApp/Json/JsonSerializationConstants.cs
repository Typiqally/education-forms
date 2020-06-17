using System.Text.Json;

namespace Summa.Forms.WebApp.Json
{
    public static class JsonSerializationConstants
    {
        public static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }
}