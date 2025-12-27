using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace EmplyeeCRUDApp.Models
{
    public class Employee : IValidatableObject
    {
        [Key]
        public int EmployeeId { get; set; }
        [Column("FirstName", TypeName ="varchar(100)")] 
        [Required, MaxLength(100)]
        [Display(Name = "First Name")]

        public string FirstName { get; set; }

        [Column("LastName", TypeName = "varchar(100)")]
        [Required, MaxLength(100)]
        [Display(Name = "Last Name")]

        public string LastName { get; set; }

        [Column("Gender", TypeName = "varchar(10)")]
        [Required]
        public string Gender { get; set; }


        [DataType(DataType.Date)]
        [Required]
       // [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Column("Email", TypeName = "varchar(100)")]
        [Required, EmailAddress]
        public string Email { get; set; }

        [Column("Phone", TypeName = "varchar(100)")]
        [Phone]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone must contain only numbers.")]
        public string Phone { get; set; }


        // FK
        [Required]
        [Display(Name = "Department")]

        public int DepartmentId { get; set; }
        public Department Departments { get; set; }
        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public Country Countries { get; set; }
        [Required]
        [Display(Name = "State")]
        public int StateId { get; set; }
        public State States { get; set; }
        [Required]
        [Display(Name = "City")]
        public int CityId { get; set; }
        public City Cities { get; set; }

        [Display(Name = "Pin Code")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Pin Code must contain only numbers.")]
        public string PinCode { get; set; }


        // stored relative path to uploaded file
        [Display(Name = "Photo Path")]
        public string PhotoPath { get; set; } = "-";


        // store hobbies as comma-separated values
        [Required]
        [MinLength(3)]
        public string Hobbies { get; set; }

        // Custom validation for min hobby count
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Hobbies))
            {
                var hobbyList = Hobbies.Split(',', StringSplitOptions.RemoveEmptyEntries);

                if (hobbyList.Length < 2)
                {
                    yield return new ValidationResult(
                        "Please enter at least 2 hobbies (comma separated).",
                        new[] { nameof(Hobbies) });
                }
            }
        }

    }
}
