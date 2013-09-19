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
using CefSharp.Wpf;
using Newtonsoft.Json;
using RemObjects.Script;
using TheIntegrator._0060_IntegratingCefSharp;

namespace TheIntegrator
{
    public partial class AdvancedCefSharpWindow
    {


        private Employee _emp;
        private List<Action> _queuedInteractions = new List<Action>();

        private WebView _webView;
        private bool _loaded = false;

        public AdvancedCefSharpWindow()
        {
            InitializeComponent();

            _webView = new WebView();

            _webView.RequestHandler = new AdvancedCefSharpRequestHandler();

            _webView.LoadCompleted += webView_LoadCompleted;
            webPlaceholder.Children.Add(_webView);

            TabPreloader.PreloadTabs(mainTab);

            ShowData(GetEmployee());
        }


        protected override void OnActivated(EventArgs e)
        {
            if (!_loaded)
            {
                _loaded = true;
                DisplayHtml(true);
            }
            base.OnActivated(e);
        }

        void webView_LoadCompleted(object sender, CefSharp.LoadCompletedEventArgs url)
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
                _queuedInteractions.Add(() => _webView.ExecuteScript(scriptName + " ('" + parameter + "')"));
            }
            else
            {
                _webView.ExecuteScript(scriptName + " ('" + parameter + "')");
            }
        }

        private void DisplayHtml(Boolean includeStyles)
        {
            if (_queuedInteractions == null) _queuedInteractions = new List<Action>();
            _webView.LoadHtml(GetHtmlContent());
            ShowData(_emp);
        }


        private static string GetHtmlContent()
        {
            var html = ScriptHelper.ReadResource("_0070_AdvancedCefSharp.AdditionalData.html");
            html += "<script type='text/javascript' src='res://JsLib/jquery.js'></script>";
            html += "<script type='text/javascript' src='res://_0070_AdvancedCefSharp/AdditionalData.js'></script>";
            html += "<link rel='stylesheet' href='res://_0070_AdvancedCefSharp/AdditionalData.css'></link>";
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
            var resp = (string)_webView.EvaluateScript("getData()");
            _emp.AdditionalInformation = resp;
        }

        private void ShowDevTools_Click(object sender, RoutedEventArgs e)
        {
            _webView.ShowDevTools();
        }

    }
}
