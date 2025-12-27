using System.ComponentModel.DataAnnotations;

namespace EmplyeeCRUDApp.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }

        [Display(Name = "State")]

        public string StateName { get; set; }
        public int CountryId { get; set; }
    }
}
