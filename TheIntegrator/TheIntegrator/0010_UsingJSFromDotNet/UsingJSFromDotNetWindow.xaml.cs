using System;
using System.Windows;
using RemObjects.Script;

namespace TheIntegrator
{
    public partial class UsingJSFromDotNetWindow : Window
    {
        readonly ScriptComponent _scriptComponent = new EcmaScriptComponent();

        public UsingJSFromDotNetWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _scriptComponent.Clear(true);
            _scriptComponent.Source = SourceExpression.Text;
            try
            {
                _scriptComponent.Run();
                var res = _scriptComponent.RunResult;
                ResultLabel.Content = res;
            }
            catch (Exception ex)
            {
                ResultLabel.Content = "Error: " + ex.Message;
            }

        }
    }
}
