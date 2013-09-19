using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TheIntegrator.Services.Models;

namespace TheIntegrator.Services.Controllers
{
    public class EmployeeController : ApiController
    {
        #region dummy data holder
        private static int _nextId = 100;
        private static Dictionary<int, Employee> _dummyData = new Dictionary<int, Employee>();

        static EmployeeController()
        {
            _nextId++;
            _dummyData.Add(_nextId, new Employee() { Id = _nextId, Name = "Markus Mustermann", Email = "markus.mustermann@example.com", });
        }
        #endregion

        public Employee Post([FromBody]Employee employee)
        {
            lock (_dummyData)
            {
                _dummyData[employee.Id] = employee;
                return employee;
            }
        }

        public Employee Put(int id, [FromBody]Employee employee)
        {
            _nextId++;
            lock (_dummyData)
            {
                employee.Id = _nextId;
                _dummyData.Add(_nextId, employee);
            }

            return employee;
        }

        public Employee Get(int id)
        {
            lock (_dummyData)
            {
                Employee employee;

                if (_dummyData.TryGetValue(id, out employee))
                {
                    return employee;
                }

                throw new Exception("Employee not found");
            }
        }
    }
}