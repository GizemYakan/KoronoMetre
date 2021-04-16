using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoronoMetre.Models
{
    public class Ulke
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Ad { get; set; }

        public long Nufus { get; set; }

        //https://www.learnentityframeworkcore.com/conventions/one-to-one-relationship
        public KoronaBilgi KoronaBilgi { get; set; }
    }
}
