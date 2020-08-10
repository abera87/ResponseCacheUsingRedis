namespace BackOffice.Model
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public int? EmpId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public string NameWithId() => $"{Name} ({EmpId})";
    }
}