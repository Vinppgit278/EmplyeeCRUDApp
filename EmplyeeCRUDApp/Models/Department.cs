using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EmplyeeCRUDApp.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        [Required]
        [Display(Name = "Department")]
        public string DepartmentName { get; set; }


        public ICollection<Employee> Employees { get; set; }
    }
}

