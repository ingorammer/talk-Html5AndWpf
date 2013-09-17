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
using TheIntegrator._0020_SharingValidationBetweenJsAndDotNet;

namespace TheIntegrator
{
    public partial class SharedValidationWindow 
    {
        ScriptComponent _scriptComponent = new EcmaScriptComponent();
        private Employee _emp;

        public SharedValidationWindow()
        {
            InitializeComponent();

            _scriptComponent.Source = ScriptHelper.ReadRessource("_0020_SharingValidationBetweenJsAndDotNet.EmployeeValidation.js");
            _scriptComponent.Run();

            _emp = GetEmployee();
            DataContext = _emp;
        }

        private Employee GetEmployee()
        {
            var emp = new Employee();
            emp.Name = "Markus Mustermann";
            emp.Salary = 12345;
            emp.Email = "markus.mustermann@example.com";
            return emp;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var jsonEmp = JsonConvert.SerializeObject(_emp);
            string jsonResult = (string)_scriptComponent.RunFunction("validateEmployee", jsonEmp);
            var res = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);

            if (res.Count == 0)
            {
                MessageBox.Show("Everything is OK!");
            }
            else
            {
                StringBuilder bld = new StringBuilder();
                foreach (var entry in res)
                {
                    bld.Append(entry.Key).Append(": ").AppendLine(entry.Value);
                }

                MessageBox.Show("Validation errors: \n\n" + bld);
            }
        }

    }
}
