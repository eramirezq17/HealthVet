using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthVet.Models
{
    public class AppointmentsModel
    {
 

        [Key]
       // [RegularExpression("[AT]+[0-9]{3}]", ErrorMessage = "El formato de los vuelos es: AT### (donde # puede ser cualquier digito del 0 al 9)")]
        [Required]
        [DisplayName("Número de cita")]
        public int id { get; set; }

        [Required]
        [DisplayName("Fecha y Hora de la cita")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public DateTime datetime { get; set; }

        [Required]
        [DisplayName("ID del usuario")]
        public int user_id { get; set; }

        [Required]
        [DisplayName("ID de la mascota")]
        public int pet_id { get; set; }

        [Required]
        [DisplayName("Procedimiento")]
        public int category_id { get; set; }

    }
}
