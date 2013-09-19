using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.ClearScript.V8;
using Newtonsoft.Json;
using TheIntegrator.Annotations;

namespace TheIntegrator._0040_CallbacksIntoDotNet
{
    public class BaseEntity : INotifyPropertyChanged, IDataErrorInfo, INotifyDataErrorInfo
    {
        private Dictionary<string, string> _errors = new Dictionary<string, string>();

        public IEnumerable GetErrors(string propertyName)
        {
            string errorText;
            if (_errors.TryGetValue(propertyName, out errorText))
            {
                return new[] { errorText };
            }

            return new string[0];
        }

        [JsonIgnore]
        public bool HasErrors { get; private set; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));

            RefreshJavaScriptValidation();
        }

        private void RefreshJavaScriptValidation()
        {
            ScriptEngineWrapper.EnsureScriptComponent();
            var functionName = "validators." + this.GetType().Name;


            var res = ScriptEngineWrapper.ExecuteCommand(functionName, this);

            var oldErrors = _errors;
            _errors = ScriptHelper.ConvertToStringArray(res);

            RaiseErrorChangedForAll(oldErrors);
        }

        private void RaiseErrorChangedForAll(Dictionary<string, string> oldErrors)
        {
            var handler = ErrorsChanged;
            if (handler == null) return;

            foreach (var err in oldErrors)
            {
                string newErrorValue;

                if (_errors.TryGetValue(err.Key, out newErrorValue))
                {
                    // this error has existed before, let's compare it

                    if (err.Value != newErrorValue)
                    {
                        // if it has changed, notify!
                        handler(this, new DataErrorsChangedEventArgs(err.Key));
                    }
                }
                else
                {
                    // this error has existed before but does not exist anymore: notify!
                    handler(this, new DataErrorsChangedEventArgs(err.Key));
                }
            }

            // and now check all new errors, to notify for the ones which have not existed before

            foreach (var err in _errors)
            {
                if (!oldErrors.ContainsKey(err.Key))
                {
                    // this is a new error which did not exit before: notify!
                    handler(this, new DataErrorsChangedEventArgs(err.Key));
                }
            }
        }

        public string this[string columnName]
        {
            get
            {
                string errorText;
                if (_errors.TryGetValue(columnName, out errorText))
                {
                    return errorText;
                }

                return null;
            }
        }

        [JsonIgnore]
        public string Error { get; private set; }
    }
}