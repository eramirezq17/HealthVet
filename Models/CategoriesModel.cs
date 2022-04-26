using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthVet.Models
{
    public class CategoriesModel
    {
        [Key]

        [DisplayName("Id Procedimiento")]
        public int id { get; set; }

        [Required]
        [DisplayName("Procedimiento")]
        public string name { get; set; }
    }
}
