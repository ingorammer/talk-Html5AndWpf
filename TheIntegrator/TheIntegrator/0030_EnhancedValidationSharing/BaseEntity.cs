using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using RemObjects.Script;
using TheIntegrator.Annotations;

namespace TheIntegrator._0030_EnhancedValidationSharing
{
    public class BaseEntity : INotifyPropertyChanged, IDataErrorInfo, INotifyDataErrorInfo
    {
        [ThreadStatic]
        private volatile static ScriptComponent _scriptComponent;
        private static object _initLock = new object();

        private Dictionary<string, string> _errors = new Dictionary<string, string>();

        public static void EnsureScriptComponent()
        {
            if (_scriptComponent == null)
            {
                lock (_initLock)
                {
                    if (_scriptComponent == null)
                    {
                        var scr = new EcmaScriptComponent();
                        scr.Source = ScriptHelper.GetCode(true, "_0030_EnhancedValidationSharing.EmployeeValidation.js");
                        scr.Run();
                        _scriptComponent = scr;
                    }
                }
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            string errorText;
            if (_errors.TryGetValue(propertyName, out errorText))
            {
                return new[] { errorText };
            }

            return new string[0];
        }

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
            EnsureScriptComponent();
            var functionName = "validate" + this.GetType().Name;
            var jsonData = JsonConvert.SerializeObject(this);

            var res = (string)_scriptComponent.RunFunction(functionName, jsonData);
            var oldErrors = _errors;
            _errors = JsonConvert.DeserializeObject<Dictionary<string, string>>(res);

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

        public string Error { get; private set; }
    }
}