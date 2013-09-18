using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ClearScript.V8;
using Newtonsoft.Json;

namespace TheIntegrator._0040_CallbacksIntoDotNet
{
    public class NativeFunctions
    {
        private readonly EngineNativeWrapper _engine;
        private readonly string _baseUrl;

        private static long _nextCallbackId = 1;

        private Dictionary<string, TaskCompletionSource<string>> _taskCompletionSources = new Dictionary<string, TaskCompletionSource<string>>();

        public NativeFunctions(EngineNativeWrapper engine, string baseUrl)
        {
            _engine = engine;
            _baseUrl = baseUrl;
        }

        private void InvokeCallback(string id, bool success, string data)
        {
            var callbackFunc = success ? id + ".success" : id + ".error";
            var dataWrapper = new { data };
            var jsonWrapped = JsonConvert.SerializeObject(dataWrapper);

            _engine.ExecuteCommand(callbackFunc + "(" + jsonWrapped + ")");
        }

        public void SetTimeout(int msec, string callbackId)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(msec);
                InvokeCallback(callbackId, true, "");
            });

        }


        public async void SendRequest(string url, string method, string data, string callbackId)
        {
            try
            {
                HttpClient client = new HttpClient();
                Uri requestUri = new Uri(_baseUrl + url);

                HttpContent content = null;

                if (data != null)
                {
                    content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(data));
                }

                HttpResponseMessage response;

                switch (method.ToUpperInvariant())
                {
                    case "POST":
                        response = await client.PostAsync(requestUri, content);
                        break;
                    case "PUT":
                        response = await client.PutAsync(requestUri, content);
                        break;
                    case "DELETE":
                        response = await client.DeleteAsync(requestUri);
                        break;
                    case "GET":
                        response = await client.GetAsync(requestUri);
                        break;
                    default:
                        InvokeCallback(callbackId, false, "Invalid method '" + method + "'");
                        return;
                }


                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    InvokeCallback(callbackId, true, responseBody);
                }
                else
                {
                    InvokeCallback(callbackId, false, "Received HTTP Status " + response.StatusCode + " -  " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                InvokeCallback(callbackId, false, "Error while executing request: " + ex.Message);
                return;
            }
        }

        public void HandleAsyncJavaScriptError(string callbackId, string data)
        {
            TaskCompletionSource<string> src;
            lock (_taskCompletionSources)
            {
                if (!_taskCompletionSources.TryGetValue(callbackId, out src))
                {
                    Trace.WriteLine("ERROR: No task completion source for '" + callbackId + "'");
                    return;
                }

                _taskCompletionSources.Remove(callbackId);
            }

            try
            {
                // get a full stack trace to this point ...
                throw new Exception(data);
            }
            catch (Exception ex)
            {
                src.SetException(ex);
            }
        }

        public void HandleAsyncJavaScriptSuccess(string callbackId, string data)
        {
            TaskCompletionSource<string> src;
            lock (_taskCompletionSources)
            {
                if (!_taskCompletionSources.TryGetValue(callbackId, out src))
                {
                    Trace.WriteLine("ERROR: No task completion source for '" + callbackId + "'");
                    return;
                }

                _taskCompletionSources.Remove(callbackId);
            }

            src.SetResult(data);
        }

        public string RegisterTaskCompletionSourceAndReturnId(TaskCompletionSource<string> completionSource)
        {
            var callbackId = "CB" + Interlocked.Increment(ref _nextCallbackId);
            lock (_taskCompletionSources)
            {
                _taskCompletionSources[callbackId] = completionSource;
            }

            return callbackId;
        }
    }
}
