using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthVet.Models
{
    public class PetsModel
    {
        [Required]
        [DisplayName("Id Dueño")]
        public int user_id { get; set; }

        [Required]
        [DisplayName("Nombre Dueño")]
        public string user_name { get; set; }

        [Key]
        [DisplayName("Id Mascota")]
        public int id { get; set; }

        [DisplayName("Nombre Mascota")]
        [Required]
        public string name { get; set; }

        [DisplayName("Edad")]
        [Required]
        public int age { get; set; }

        [Required]
        [DisplayName("Id Raza")]
        public int breed_id { get; set; }

        [DisplayName("Nombre Raza")]
        public string breed_name { get; set; }

        [DisplayName("Nombre Animal")]
        public string animal_name { get; set; }

        [DisplayName("Raza")]
        [Required]
        public string fullbreed
        {
            get
            {
                return animal_name + " - " + breed_name;
            }

        }
    }
}

