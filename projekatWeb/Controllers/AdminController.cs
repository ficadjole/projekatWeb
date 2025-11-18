using projekatWeb.Models;
using projekatWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace projekatWeb.Controllers
{
    public class AdminController : Controller
    {
        KorisnikService korisnikService;
        public AdminController()
        {
            korisnikService = (KorisnikService)System.Web.HttpContext.Current.Application["KorisniciService"];
        }

        // GET: Admin
        public ActionResult Panel()
        {
            var sviKorisnici = korisnikService.GetAll();

            var sviSemNjega = sviKorisnici.Where(k => k.KorisnickoIme != ((Models.Korisnik)Session["LOGGEDIN"]).KorisnickoIme).ToList();

            return View(sviSemNjega);
        }

        [HttpPost]
        public ActionResult PretragaKorisnika(string Ime, string Prezime, Uloga? uloga,string akcija)
        {
            var sviKorisnici = korisnikService.GetAll();

            var sviSemNjega = sviKorisnici.Where(k => k.KorisnickoIme != ((Models.Korisnik)Session["LOGGEDIN"]).KorisnickoIme).ToList();

            if (akcija == "osvezi")
            {
                return View("Panel",sviSemNjega);
            }


            if (!string.IsNullOrEmpty(Ime))
            {
                sviSemNjega = pretragaPoImenu(sviSemNjega, Ime);
            }

            if (!string.IsNullOrEmpty(Prezime))
            {
                sviSemNjega = pretragaPoPrezimenu(sviSemNjega, Prezime);
            }

            if (!uloga.ToString().IsEmpty())
            {
                sviSemNjega = pretragaPoUlozi(sviSemNjega, uloga.Value);
            }


            return View("Panel",sviSemNjega);
        }

        private List<Korisnik> pretragaPoImenu(List<Korisnik> korisnici, string Ime)
        {

            List<Korisnik> filtrirani = new List<Korisnik>();
            foreach (var k in korisnici)
            {
                if (k.Ime.ToLower().Contains(Ime.ToLower()))
                {
                    filtrirani.Add(k);
                }
            }
            return filtrirani;

        }

        private List<Korisnik> pretragaPoPrezimenu(List<Korisnik> korisnici, string Prezime)
        {
            List<Korisnik> filtrirani = new List<Korisnik>();
            foreach (var k in korisnici)
            {
                if (k.Prezime.ToLower().Contains(Prezime.ToLower()))
                {
                    filtrirani.Add(k);
                }
            }
            return filtrirani;
        }

        private List<Korisnik> pretragaPoUlozi(List<Korisnik> korisnici, Uloga uloga)
        {
            List<Korisnik> filtrirani = new List<Korisnik>();
            foreach (var k in korisnici)
            {
                if (k.Uloga == uloga)
                {
                    filtrirani.Add(k);
                }
            }
            return filtrirani;
        }
    }
}