using System.ComponentModel.DataAnnotations;

namespace EmplyeeCRUDApp.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }
    }
}
