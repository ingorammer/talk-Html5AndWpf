using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RemObjects.Script.EcmaScript;

namespace TheIntegrator
{
    public class ScriptHelper
    {
        private static string[] _libs = { "json2.js" };

        public static string ReadResource(string name)
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
                    bld.AppendLine(ReadResource("JsLib." + lib));
                    bld.Append(";");
                }
            }

            foreach (var name in resourceNames)
            {
                bld.Append(ReadResource(name));
            }

            return bld.ToString();
        }

        public static Dictionary<string, string> ConvertToStringArray(EcmaScriptObject res)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var name in res.Names)
            {
                var val = res.Values[name].Value;
                if (val != null)
                {
                    dict.Add(name, val.ToString());
                }
            }

            return dict;
        }

        public static Dictionary<string, string> ConvertToStringArray(string res)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
        }
    }
}
