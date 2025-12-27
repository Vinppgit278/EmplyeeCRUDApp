using System.ComponentModel.DataAnnotations;

namespace EmplyeeCRUDApp.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Gender { get; set; }
        [DataType(DataType.Date)] public DateTime DOB { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        public string Phone { get; set; }

        [Required] public int DepartmentId { get; set; }

        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }

        // Photo
        public IFormFile Photo { get; set; }
        public string PhotoPath { get; set; }

        // Hobbies selected
        public List<string> SelectedHobbies { get; set; } = new List<string>();

        // For drop-downs
        public IEnumerable<Department> Departments { get; set; }
    }
}

