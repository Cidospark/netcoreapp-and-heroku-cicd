using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UMS.Models
{
    
    public class EmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public EmployeeRepository()
        {
            _employeeList = new List<Employee>
            {
                new Employee{ Id = 1, Name = "Mary", Department="HR", Email = "mary@sample.com" },
                new Employee{ Id = 2, Name = "John", Department="IT", Email = "john@sample.com" },
                new Employee{ Id = 3, Name = "Sam", Department="IT", Email = "sam@sample.com" }
            };
        }
        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }

        public List<Employee> GetEmployees()
        {
            return _employeeList;
        }
    }
}
