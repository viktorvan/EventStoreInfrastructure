using System;
using System.IO;
using System.Reflection;

namespace Workout.Infrastructure
{
    public class Resources
    {
        public static string Read(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new Exception(
                        $"Could not find {resourceName}. Available resources: {string.Join(", ", assembly.GetManifestResourceNames())}");

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}