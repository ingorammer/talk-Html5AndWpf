﻿using System;
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
using TheIntegrator._0030_EnhancedValidationSharing;

namespace TheIntegrator
{
    public partial class EnhancedSharedValidationWindow : Window
    {
        private Employee _emp;

        public EnhancedSharedValidationWindow()
        {
            InitializeComponent();


            _emp = GetEmployee();
            this.DataContext = _emp;
        }

        private Employee GetEmployee()
        {
            var emp = new Employee();
            emp.Name = "Markus Mustermann";
            emp.Salary = 12345;
            emp.Email = "markus.mustermann@example.com";
            return emp;
        }

    }
}
