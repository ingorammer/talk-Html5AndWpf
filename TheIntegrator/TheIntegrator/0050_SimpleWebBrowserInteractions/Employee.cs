using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TheIntegrator._0050_SimpleWebBrowserInteractions
{
    public class Employee
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public decimal Salary { get; set; }
        public int Id { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
