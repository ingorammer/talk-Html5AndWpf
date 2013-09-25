using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using CefSharp.Wpf;

namespace TheIntegrator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            /*
             
             * If DevTools are not required, this can be configured
             */

            
            var settings = new CefSharp.Settings
            {
                PackLoadingDisabled = false,
            };
            

            if (!CEF.Initialize(settings))
            {
                MessageBox.Show("Could not initialize CEF");
            }
            else
            {
                var webView = new WebView();
                webView.Height = 0;
                webView.Width = 0;
                mainPanel.Children.Add(webView);

            }
        }

        private void UsingJavaScriptButton_Click(object sender, RoutedEventArgs e)
        {
            UsingJSFromDotNetWindow window = new UsingJSFromDotNetWindow();
            window.Show();
        }

        private void SharedValidation_Click(object sender, RoutedEventArgs e)
        {
            SharedValidationWindow window = new SharedValidationWindow();
            window.Show();
        }

        private void ExtendedValidation_Click(object sender, RoutedEventArgs e)
        {
            EnhancedSharedValidationWindow window = new EnhancedSharedValidationWindow();
            window.Show();
            
        }

        private void CallbackIntoDotNet_Click(object sender, RoutedEventArgs e)
        {
            CallbacksIntoDotNetWindow window = new CallbacksIntoDotNetWindow();
            window.Show();
        }

        private void SimpleWebBrowserInteractions_Click(object sender, RoutedEventArgs e)
        {
            SimpleWebBrowserInteractionsWindow window = new SimpleWebBrowserInteractionsWindow();
            window.Show();
        }

        private void EmbeddingChromiumButton_Click(object sender, RoutedEventArgs e)
        {
            IntegratingCefSharpWindow window = new IntegratingCefSharpWindow();
            window.Show();
        }

        private void AdvancedCefSharpButton_Click(object sender, RoutedEventArgs e)
        {
            AdvancedCefSharpWindow window = new AdvancedCefSharpWindow();
            window.Show();
        }

    }
}
