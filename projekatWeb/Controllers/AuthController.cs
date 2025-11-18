using projekatWeb.Models;
using projekatWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projekatWeb.Controllers
{
    public class AuthController : Controller
    {
        KorisnikService korisnikService;
        public AuthController()
        {

            korisnikService = (KorisnikService)System.Web.HttpContext.Current.Application["KorisniciService"];
        }

        // GET: Auth
        public ActionResult Index()
        {
            var prijavljen = System.Web.HttpContext.Current.Session["LOGGEDIN"];

            if(prijavljen != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Prijava(string username,string password)
        {
            Korisnik prijavljen = korisnikService.AuthKorisnika(username, password);

            if (prijavljen.Id == 0)
            {
                ViewBag.Poruka = "Ne postoji korisnik za unete podatke!";
                return View("Index");
            }
            else
            {
                System.Web.HttpContext.Current.Session["LOGGEDIN"] = prijavljen;
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Odjava()
        {
            Session["LOGGEDIN"] = null;
            return RedirectToAction("Index", "Home");
        }

        // GET: Auth/Registracija
        public ActionResult RegistracijaRedirekcija()
        {
            if(Session["LOGGEDIN"] != null)
            {
                ViewBag.Korisnik = (Korisnik)Session["LOGGEDIN"];
            }

            return View();
        }

        [HttpPost]
        public ActionResult RegistracijaAkcija(string Ime,string Prezime,string KorisnickoIme, string Lozinka, string Email, DateTime DatumRodjenja, string Pol)
        {
            Korisnik korisnik = korisnikService.GetByKorisnickoIme(KorisnickoIme);

            if (korisnik.Id != 0)
            {
                ViewBag.Poruka = "Korisnik sa unetim korisnickim imenom vec postoji!";
                return View("RegistracijaRedirekcija");
            }

            int noviId = korisnikService.GetNextIndex();

            Pol pol = Pol.Equals("Muski") ? Models.Pol.Muski : Models.Pol.Zenski;

            string datum = DatumRodjenja.ToString("dd/MM/yyyy");

            Korisnik noviKorisnik;

            if (Session["LOGGEDIN"] != null)
            {
                noviKorisnik = new Menadzer(noviId, KorisnickoIme, Lozinka, Ime, Prezime, pol, Email, datum, Uloga.Menadzer, null);
            }
            else {

                noviKorisnik = new Turista(noviId, KorisnickoIme, Lozinka, Ime, Prezime, pol, Email, datum, Uloga.Turista, null);
            }

            var dodat = korisnikService.DodajKorisnika(noviKorisnik);

            if (dodat.Id != 0)
            {
                if (Session["LOGGEDIN"] != null)
                {
                    return RedirectToAction("Panel", "Admin");
                }
                else
                {
                    System.Web.HttpContext.Current.Session["LOGGEDIN"] = dodat;
                    return RedirectToAction("Index", "Home");
                }


            }
            else
            {
                ViewBag.Poruka = "Doslo je do greske prilikom registracije, pokusajte ponovo!";
                return View("RegistracijaRedirekcija");
            }
               
        }
    }
}