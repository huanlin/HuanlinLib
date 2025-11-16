using System.Text.Json;

namespace Huanlin.Common.Helpers
{
    public static class JsonHelper
    {
        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T Deserialize<T>(string jsonStr)
        {
            return JsonSerializer.Deserialize<T>(jsonStr);
        }
    }
}