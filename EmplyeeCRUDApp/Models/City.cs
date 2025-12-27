using System.ComponentModel.DataAnnotations;

namespace EmplyeeCRUDApp.Models
{
    //test
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Display(Name = "City")]

        public string CityName { get; set; }
        public int StateId { get; set; }
    }
}
