using HtmlAgilityPack;
using KoronoMetre.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KoronoMetre.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KoronaContextDb _db;
        private readonly string url = "https://www.worldometers.info/coronavirus/";

        public HomeController(ILogger<HomeController> logger, KoronaContextDb db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.KoronaBilgileri
                .Include(x => x.Ulke)
                .OrderByDescending(x => x.VakaSayisi)
                .ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Doldur()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            HtmlNodeCollection ulkeAdlar = doc.DocumentNode.SelectNodes("//tr/td[2]//*");
            HtmlNodeCollection ulkeVakalar = doc.DocumentNode.SelectNodes("//tr/td[3]");
            HtmlNodeCollection ulkeOlumler = doc.DocumentNode.SelectNodes("//tr/td[5]");
            HtmlNodeCollection ulkeNufus = doc.DocumentNode.SelectNodes("//tr/td[15]");
            List<string> adlar = new List<string>();
            List<string> vakalar = new List<string>();
            List<string> olumler = new List<string>();
            List<string> nufuslar = new List<string>();
            for (int i = 7; i < 227; i++)
            {
                adlar.Add(ulkeAdlar[i].InnerHtml);
            }
            for (int i = 8; i < 228; i++)
            {
                vakalar.Add(ulkeVakalar[i].InnerHtml == " " ? "0" : ulkeVakalar[i].InnerHtml.Trim().Replace(",", ""));
                olumler.Add(ulkeOlumler[i].InnerHtml == " " ? "0" : ulkeOlumler[i].InnerHtml.Trim().Replace(",", ""));
                nufuslar.Add(ulkeNufus[i].InnerText == " " ? "0" : ulkeNufus[i].InnerText.Trim().Replace(",", ""));
            }
            for (int i = 0; i < adlar.Count; i++)
            {
                if (!_db.Ulkeler.Select(x => x.Ad).Contains(adlar[i]))
                {
                    Ulke ulke = new Ulke()
                    {
                        Ad = adlar[i],
                        Nufus = Convert.ToInt64(nufuslar[i]),
                        KoronaBilgi = new KoronaBilgi()
                        {
                            OlumSayisi = Convert.ToInt32(olumler[i] == " " || string.IsNullOrEmpty(olumler[i]) ? "0" : olumler[i]),
                            VakaSayisi = Convert.ToInt32(vakalar[i])
                        }
                    };
                    _db.Ulkeler.Add(ulke);
                    _db.SaveChanges();
                }
                else
                {
                    Ulke ulke = _db.Ulkeler.Include(x => x.KoronaBilgi).FirstOrDefault(x => x.Ad == adlar[i]);
                    ulke.Nufus = Convert.ToInt64(nufuslar[i]);
                    ulke.KoronaBilgi.OlumSayisi = Convert.ToInt32(olumler[i]);
                    ulke.KoronaBilgi.VakaSayisi = Convert.ToInt32(vakalar[i]);
                    _db.Update(ulke);
                }
            }
            return RedirectToAction("Index");
        }
    }
}

