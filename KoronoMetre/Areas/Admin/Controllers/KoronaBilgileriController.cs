using KoronoMetre.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoronoMetre.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KoronaBilgileriController : Controller
    {
        private readonly KoronaContextDb _db;
        public KoronaBilgileriController(KoronaContextDb db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.KoronaBilgileri.Include(x => x.Ulke).ToListAsync());
        }

        public async Task<IActionResult> Yeni()
        {

            ViewBag.Ulkeler = new SelectList(await _db.Ulkeler.ToListAsync(), "Id", "Ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Yeni(KoronaBilgi koronaBilgi)
        {
            if (ModelState.IsValid)
            {
                _db.KoronaBilgileri.Add(koronaBilgi);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
            //https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-5.0#the-select-tag-helperer
            ViewBag.Ulkeler = new SelectList(await _db.Ulkeler.ToListAsync(), "Id", "Ad");
            return View(koronaBilgi);
        }

        public async Task<IActionResult> Duzenle(int id)
        {
            var koronaBilgi = await _db.KoronaBilgileri.FindAsync(id);
            if (koronaBilgi == null)
            {
                return NotFound();
            }
            ViewBag.Ulkeler = new SelectList(await _db.Ulkeler.ToListAsync(), "Id", "Ad");
            return View(koronaBilgi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(KoronaBilgi koronaBilgi)
        {
            if (ModelState.IsValid)
            {
                //_db.Entry(koronaBilgi).State = EntityState.Modified;
                _db.Update(koronaBilgi);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Ulkeler = new SelectList(await _db.Ulkeler.ToListAsync(), "Id", "Ad");
            return View(koronaBilgi);
        }
        public async Task<IActionResult> Sil(int id)
        {
            var koronaBilgi = await _db.KoronaBilgileri.Include(x => x.Ulke).FirstOrDefaultAsync(x => x.Id == id);
            if (koronaBilgi == null)
            {
                return NotFound();
            }
            return View(koronaBilgi);
        }
        [HttpPost,ActionName("Sil")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SilOnay(int id)
        {
            var koronaBilgi = await _db.KoronaBilgileri.Include(x => x.Ulke).FirstOrDefaultAsync(x => x.Id == id);
            if (koronaBilgi == null)
            {
                return NotFound();
            }
            _db.KoronaBilgileri.Remove(koronaBilgi);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
