using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ClearScript.V8;
using Newtonsoft.Json;

namespace TheIntegrator._0040_CallbacksIntoDotNet
{
    public class EngineNativeWrapper
    {
        internal V8ScriptEngine Engine;
        internal NativeFunctions Native;
        internal string ScriptSource;

        public string ExecuteCommand(string command)
        {
            return Engine.ExecuteCommand(command);
        }

        public async Task<T> ExecuteAsyncCommand<T>(string command, string jsonData)
        {
            if (string.IsNullOrWhiteSpace(jsonData)) jsonData = "null";

            TaskCompletionSource<T> completionSource = new TaskCompletionSource<T>();
            Task<T> task = completionSource.Task;

            TaskCompletionSource<string> javaScriptCompletionSource = new TaskCompletionSource<string>();
            var callbackId = Native.RegisterTaskCompletionSourceAndReturnId(javaScriptCompletionSource);

            var commandText = command + "(" + jsonData +
                              ", function(data){nativeFunctions.HandleAsyncJavaScriptSuccess('" + callbackId + "', JSON.stringify(data));}" +
                              ", function(err){nativeFunctions.HandleAsyncJavaScriptError('" + callbackId + "',JSON.stringify(err));}" +
                              ");";

            ExecuteCommand(commandText);

            try
            {
                var stringResult = await javaScriptCompletionSource.Task;
                var data = JsonConvert.DeserializeObject<T>(stringResult);
                completionSource.SetResult(data);
            }
            catch (Exception ex)
            {
                completionSource.SetException(ex);
            }

            return await task;
        }
    }

}
