using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthVet.Models
{
    public class AppointmentsViewModel
    {

        [Key]
        [Required]
        [DisplayName("Número de cita")]
        public int id { get; set; }

        [Required]
        [DisplayName("Fecha y Hora de la cita")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public DateTime datetime { get; set; }

        [Required]
        [DisplayName("ID Procedimiento")]
        public int category_id { get; set; }

        [Required]
        [DisplayName("Procedimiento")]
        public string category_name { get; set; }

        [Required]
        [DisplayName("ID del usuario")]
        public int user_id { get; set; }

        [Required]
        [DisplayName("ID de la mascota")]
        public int pet_id { get; set; }

        [Required]
        [DisplayName("Nombre Mascota")]
        public string pet_name { get; set; }

        [DisplayName("Edad")]
        [Required]
        public int age { get; set; }

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
