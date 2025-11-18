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
    public class KorisnikController : Controller
    {

        KorisnikService korisnikService;
        RezervacijaService rezervacijaService;
        AranzmanService aranzmanService;

        public KorisnikController()
        {
            korisnikService = (KorisnikService)System.Web.HttpContext.Current.Application["KorisniciService"];
            rezervacijaService = (RezervacijaService)System.Web.HttpContext.Current.Application["RezervacijaService"];
            aranzmanService = (AranzmanService)System.Web.HttpContext.Current.Application["AranzmaniService"];
        }

        // GET: Turista
        public ActionResult Profil()
        {
            Korisnik korisnik = (Korisnik)Session["LOGGEDIN"];

            return View(korisnik);
        }

        public ActionResult Azuriraj(Korisnik azuriraniKorisnik)
        {
            DateTime dateTime = azuriraniKorisnik.DatumRodjenja.AsDateTime();

            azuriraniKorisnik.DatumRodjenja = dateTime.ToString("dd/MM/yyyy");

            Korisnik azuriran = korisnikService.AzurirajKorisnika(azuriraniKorisnik);

            ViewBag.Uspesno = true;
            ViewBag.Poruka = "Uspesno ste azurirali podatke!";

            if (azuriran.Uloga.Equals(Uloga.Turista))
            {
                Session["LOGGEDIN"] = (Turista)azuriran;
            }
            else if (azuriran.Uloga.Equals(Uloga.Menadzer))
            {
                Session["LOGGEDIN"] = (Menadzer)azuriran;
            }
            else
            {
                Session["LOGGEDIN"] = azuriran;
            }

                

            return View("Profil", azuriran);
        }
    }
}