using System.Collections.Generic;

namespace BackOffice.Model
{
    public class Department
    {       
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}