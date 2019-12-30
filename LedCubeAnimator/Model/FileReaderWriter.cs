using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model
{
    public static class FileReaderWriter
    {
        public static void Save(string path, Animation animation)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.Write(JsonConvert.SerializeObject(animation, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }));
            }
        }

        public static Animation Open(string path)
        {
            try
            {
                using (var sr = new StreamReader(path))
                {
                    return JsonConvert.DeserializeObject<Animation>(sr.ReadToEnd(), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
