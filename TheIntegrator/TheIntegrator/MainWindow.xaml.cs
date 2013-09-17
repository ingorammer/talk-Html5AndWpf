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

    }
}
