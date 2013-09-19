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
using TheIntegrator._0040_CallbacksIntoDotNet;

namespace TheIntegrator
{
    public partial class CallbacksIntoDotNetWindow : Window
    {
        private Employee _emp;

        public CallbacksIntoDotNetWindow()
        {
            InitializeComponent();
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void LoadData()
        {
            try
            {
                this.Cursor = Cursors.Wait;
                _emp = await GetEmployee();
                DataContext = _emp;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                _emp = await StoreEmployee(_emp);
                DataContext = _emp;
                this.Cursor = Cursors.Arrow;
                this.Close();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // ==== Actual implementations without the necessary errorhandlers below

        private async Task<Employee> GetEmployee()
        {
            return await ScriptEngineWrapper.ExecuteAsyncCommand<Employee>("dataAccess.Employee.get", new { Id = 101 });
        }

        private async Task<Employee> StoreEmployee(Employee emp)
        {
            return await ScriptEngineWrapper.ExecuteAsyncCommand<Employee>("dataAccess.Employee.store", emp);
        }
    }
}
