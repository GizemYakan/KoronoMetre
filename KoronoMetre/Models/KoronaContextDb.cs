using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoronoMetre.Models
{
    public class KoronaContextDb : DbContext
    {
        public KoronaContextDb(DbContextOptions<KoronaContextDb> options) : base(options)
        {
            
        }
        public DbSet<KoronaBilgi> KoronaBilgileri { get; set; }
        public DbSet<Ulke> Ulkeler { get; set; }
    }
}
