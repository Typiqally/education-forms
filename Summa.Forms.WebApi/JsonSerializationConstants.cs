using System.Text.Json;

namespace Summa.Forms.WebApi
{
    public static class JsonSerializationConstants
    {
        public static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            WriteIndented = true
        };
    }
}