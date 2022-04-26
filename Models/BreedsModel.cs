using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HealthVet.Models
{
    public class BreedsModel
    {
        [Key]
        public int id { get; set; }

        public string name { get; set; }

        public int animal_id { get; set; }

        public string animal_name { get; set; }

        public string fullbreed
        {
            get
            {
                return animal_name + " - " + name;
            }

        }
    }
}