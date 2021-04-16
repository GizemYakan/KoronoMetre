using KoronoMetre.Areas.Admin.ViewModels;
using KoronoMetre.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace KoronoMetre.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly KoronaContextDb _db;
        public DashboardController(KoronaContextDb context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            var ulkeler = _db.KoronaBilgileri
                .OrderByDescending(x => x.VakaSayisi)
                .Select(x => x.Ulke.Ad)
                .ToArray();
            var vakalar = _db.KoronaBilgileri
                .OrderByDescending(x => x.VakaSayisi)
                .Select(x => x.VakaSayisi)
                .ToArray();
            var olumler = _db.KoronaBilgileri
                .OrderByDescending(x => x.VakaSayisi)
                .Select(x => x.OlumSayisi)
                .ToArray();

            var vm = new DashboardVM()
            {
                UlkelerJson = JsonSerializer.Serialize(ulkeler),
                VakaSayilariJson = JsonSerializer.Serialize(vakalar), 
                OlumSayilariJson = JsonSerializer.Serialize(olumler)
            };
            return View(vm);
        }
    }
}


