using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthVet.Models
{
    public class UsersModel
    {
        [Key]
        [DisplayName("id cliente")]
        public int id { get; set; }

        [DisplayName("Número de cédula")]
        [Required]
        public int idcard { get; set; }

        [DisplayName("Nombre")]
        [Required]
        public string name { get; set; }

        [DisplayName("Apellido")]
        [Required]
        public string lastname { get; set; }

        [DisplayName("Teléfono")]
        [Required]
        public string phone { get; set; }

        [DisplayName("Correo electrónico (email)")]
        [Required]

        public string email { get; set; }

        [DisplayName("Contraseña")]
        [Required]
        public string password { get; set; }

        public string rol { get; set; }
    }
}

