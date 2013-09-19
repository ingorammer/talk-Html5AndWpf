using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using RemObjects.Script;
using TheIntegrator._0050_SimpleWebBrowserInteractions;

namespace TheIntegrator
{
    public partial class SimpleWebBrowserInteractionsWindow
    {
        private Employee _emp;
        private bool _loaded = false;

        private List<Action> _queuedInteractions = new List<Action>();

        public SimpleWebBrowserInteractionsWindow()
        {
            InitializeComponent();
            webBrowser.LoadCompleted += webBrowser_LoadCompleted;
            ShowData(GetEmployee());
        }

        void webBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (_queuedInteractions != null && _queuedInteractions.Count > 0)
            {
                foreach (var func in _queuedInteractions)
                {
                    func();
                }
            }

            _queuedInteractions = null;
        }


        private void ShowData(Employee emp)
        {
            _emp = emp;
            DataContext = _emp;
            InvokeScriptOrQueue("showData", emp.AdditionalInformation);
        }

        private void InvokeScriptOrQueue(string scriptName, string parameter)
        {
            if (_queuedInteractions != null)
            {
                _queuedInteractions.Add(() => webBrowser.InvokeScript(scriptName, parameter));
            }
            else
            {
                webBrowser.InvokeScript(scriptName, parameter);
            }
        }

        private void DisplayHtml(Boolean includeStyles)
        {
            if (_queuedInteractions == null) _queuedInteractions = new List<Action>();
            webBrowser.NavigateToString(GetHtmlContent(includeStyles));
            ShowData(_emp);
        }

        protected override void OnActivated(EventArgs e)
        {
            if (!_loaded)
            {
                _loaded = true;
                DisplayHtml(false);
            }
            base.OnActivated(e);
        }

        private static string GetHtmlContent(bool includeStyles)
        {
            var html = ScriptHelper.ReadResource("_0050_SimpleWebBrowserInteractions.AdditionalData.html");
            html += "<script type='text/javascript'>" + ScriptHelper.GetCode(true, "JsLib.jquery.js") +
                    "</script>";
            html += "<script>" + ScriptHelper.GetCode(false, "_0050_SimpleWebBrowserInteractions.AdditionalData.js") +
                    "</script type='text/javascript'>";
            if (includeStyles)
            {
                html += "<style>" + ScriptHelper.GetCode(false, "_0050_SimpleWebBrowserInteractions.AdditionalData.css") +
                        "</style>";
            }
            return html;
        }

        private Employee GetEmployee()
        {
            var emp = new Employee();
            emp.Name = "Markus Mustermann";
            emp.Salary = 12345;
            emp.Id = 8888;
            emp.Email = "markus.mustermann@example.com";
            emp.AdditionalInformation = "{\"Spouse\":\"Bertha Mustermann\", \"HomeEmail\":\"foo@example.com\"}";
            return emp;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var resp = (string) webBrowser.InvokeScript("getData");
            _emp.AdditionalInformation = resp;
        }

        private void ReloadWithStyle_Click(object sender, RoutedEventArgs e)
        {
            DisplayHtml(true);
        }

    }
}
