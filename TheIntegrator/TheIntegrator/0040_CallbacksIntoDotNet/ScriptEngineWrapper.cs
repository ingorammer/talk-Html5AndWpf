using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ClearScript.V8;
using Newtonsoft.Json;
using RemObjects.Script.EcmaScript;

namespace TheIntegrator._0040_CallbacksIntoDotNet
{
    public class ScriptEngineWrapper
    {
        public static string BaseUrl = "http://localhost:23186/";

        [ThreadStatic]
        private volatile static EngineNativeWrapper _engineWrapper;

        private static object _initLock = new object();

        public static void EnsureScriptComponent()
        {
            if (_engineWrapper == null)
            {
                lock (_initLock)
                {
                    if (_engineWrapper == null)
                    {
                        EngineNativeWrapper engineWrapper = new EngineNativeWrapper();
                        engineWrapper.Engine = new V8ScriptEngine();
                        engineWrapper.Native = new NativeFunctions(engineWrapper, BaseUrl);
                        engineWrapper.ScriptSource = ScriptHelper.GetCode(false, "_0040_CallbacksIntoDotNet.WrapperAbstraction.js", "_0040_CallbacksIntoDotNet.Employee.js");
                        engineWrapper.Engine.AddHostObject("nativeFunctions", engineWrapper.Native);
                        engineWrapper.Engine.Execute(engineWrapper.ScriptSource);
                        _engineWrapper = engineWrapper;
                    }
                }
            }
        }

        public static Task<T> ExecuteAsyncCommand<T>(string command, object data)
        {
            // this method invokes a JavaScript command which has to follow this convention
            //            command = function(data, success, failure)

            // i.e.: it has to take two callbacks, one for success and one for failure.

            // the call to this command is passed through wrapperAbstractions.js to setup callback-ID 
            // based handling back to C# without the dependency to pass specific function-delegates
            // into the javascript engine

            EnsureScriptComponent();
            var jsonData = GetJsonData(data);
            return _engineWrapper.ExecuteAsyncCommand<T>(command, jsonData);
        }

        public static string ExecuteCommand(string command, object data)
        {
            EnsureScriptComponent();
            var jsonData = GetJsonData(data);
            return _engineWrapper.ExecuteCommand(command + "(" + jsonData + ")");
        }

        private static string GetJsonData(object data)
        {
            string jsonData = "";

            if (data != null)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                jsonData = JsonConvert.SerializeObject(data, settings);
            }
            return jsonData;
        }
    }
}
