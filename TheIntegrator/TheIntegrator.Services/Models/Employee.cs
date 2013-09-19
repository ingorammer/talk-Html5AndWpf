using System;

namespace TheIntegrator.Services.Models
{
    public class Employee 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public decimal Salary { get; set; }
    }
}
