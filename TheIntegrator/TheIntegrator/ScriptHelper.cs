using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheIntegrator
{
    public class ScriptHelper
    {
        private static string[] _libs = { "json2.js" };

        public static string ReadRessource(string name)
        {
            using (var stream = typeof(ScriptHelper).Assembly.GetManifestResourceStream("TheIntegrator." + name))
            {
                using (var sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }

        }

        public static string GetCode(bool includeLibs, params string[] resourceNames)
        {
            StringBuilder bld = new StringBuilder();

            if (includeLibs)
            {
                foreach (var lib in _libs)
                {
                    bld.AppendLine(ReadRessource("JsLib." + lib));
                    bld.Append(";");
                }
            }

            foreach (var name in resourceNames)
            {
                bld.Append(ReadRessource(name));
            }

            return bld.ToString();
        }
    }
}
