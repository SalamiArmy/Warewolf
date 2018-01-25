using System.IO;
using System.Reflection;

namespace Dev2.Common.Tests
{
    public class JsonResource
    {
        public static string Fetch(string name)
        {
            var resourceName = $"Dev2.Common.Tests.Json.{name}.json";
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return string.Empty;
                }
                using (StreamReader sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
