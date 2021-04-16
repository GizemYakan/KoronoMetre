using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KoronoMetre.Models
{
    public class KoronaBilgi
    {
        public int Id { get; set; }

        [Required]
        public int VakaSayisi { get; set; }

        [Required]
        public int OlumSayisi { get; set; }

        public int UlkeId { get; set; }
        public Ulke Ulke { get; set; }
    }
}
