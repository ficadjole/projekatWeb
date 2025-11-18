using projekatWeb.DTOs;
using projekatWeb.Models;
using projekatWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace projekatWeb.Controllers
{
    public class RezervacijaController : Controller
    {
        KorisnikService korisnikService;
        RezervacijaService rezervacijaService;
        SmestajnaJedinicaService smestajnaJedinicaService;
        AranzmanService aranzmanService;
        SmestajService smestajService;

        public RezervacijaController()
        {
            korisnikService = (KorisnikService)System.Web.HttpContext.Current.Application["KorisniciService"];
            rezervacijaService = (RezervacijaService)System.Web.HttpContext.Current.Application["RezervacijaService"];
            smestajnaJedinicaService = (SmestajnaJedinicaService)System.Web.HttpContext.Current.Application["SmestajnaJedinicaService"];
            aranzmanService = (AranzmanService)System.Web.HttpContext.Current.Application["AranzmaniService"];
            smestajService = (SmestajService)System.Web.HttpContext.Current.Application["SmestajService"];
        }

        // GET: Rezervacija
        public ActionResult Index(int idAranzmana,int idSmestajneJedinice,int idSmestaja)
        {

            Aranzman aranzman = aranzmanService.GetById(idAranzmana);
            SmestajnaJedinica smestajnaJedinica = smestajnaJedinicaService.GetById(idSmestajneJedinice);
            Smestaj smestaj = smestajService.GetById(idSmestaja);

            RezervacijaDto rezervacijaDto = new RezervacijaDto(aranzman, smestajnaJedinica, smestaj);

            return View(rezervacijaDto);
        }

        public ActionResult PrikazRezervacije(int idAranzmana, int idSmestajneJedinice, int idSmestaja)
        {
            Aranzman aranzman = aranzmanService.GetById(idAranzmana);
            SmestajnaJedinica smestajnaJedinica = smestajnaJedinicaService.GetById(idSmestajneJedinice);
            Smestaj smestaj = smestajService.GetById(idSmestaja);

            RezervacijaDto rezervacijaDto = new RezervacijaDto(aranzman, smestajnaJedinica, smestaj);

            return View(rezervacijaDto);
        }

        [HttpPost]
        public ActionResult Rezervacija(int idAranzmana,int idSmestajneJedinice, int idSmestaja)
        {
            
            int idRez = rezervacijaService.GetNextId();

            var korisnik = (Turista)Session["LOGGEDIN"];

            //var sveRezervacije = rezervacijaService.SveRezervacijeKorisnika(korisnik.Id);

            //foreach(var item in sveRezervacije)
            //{
            //    if(item.IdAranzman == idAranzmana)
            //    {
            //        ViewBag.Poruka = "Vec ste otkazali ovaj aranzman, niste vise u mogucnosti da rezervisete";

            //        Aranzman aranzman = aranzmanService.GetById(idAranzmana);
            //        Smestaj smestaj = smestajService.GetById(idSmestaja);
            //        SmestajnaJedinica smestajnaJedinica = smestajnaJedinicaService.GetById(idSmestajneJedinice);
            //        RezervacijaDto rezervacijaDto = new RezervacijaDto(aranzman, smestajnaJedinica, smestaj);
            //        return View("Index", rezervacijaDto);
            //    }
            //}


            Rezervacija rezervacija = new Rezervacija(idRez, korisnik.Id, true, idAranzmana, idSmestajneJedinice);

            var rez = rezervacijaService.DodajRezervaciju(rezervacija);//dodao sam rezervaciju

            if(rez.IdRezervacije != 0)
            {
                var uspesnost = korisnikService.DodajRezervisaniAranzman(korisnik.Id, rez.IdRezervacije);//dodao sam rezervaciju u listu

                SmestajnaJedinica smestajnaJedinica = smestajnaJedinicaService.setDostupnost(idSmestajneJedinice, false);

                if (smestajnaJedinica.Id == 0)
                {
                    ViewBag.Poruka = "Doslo je do greske prilikom rezervacije!";
                    Aranzman aranzman = aranzmanService.GetById(idAranzmana);
                    Smestaj smestaj = smestajService.GetById(idSmestaja);
                    RezervacijaDto rezervacijaDto = new RezervacijaDto(aranzman, smestajnaJedinica, smestaj);
                    return View("Index", rezervacijaDto);
                }
                else
                {
                    TuristaDto turistaDto = kreirajDto();
                    ViewBag.Poruka = "Uspesno ste rezervisali aranzman!";
                    return View("~/Views/Turista/Panel.cshtml", turistaDto);
                }

            }
            else
            {
                ViewBag.Poruka = "Doslo je do greske prilikom rezervacije!";
                Aranzman aranzman = aranzmanService.GetById(idAranzmana);
                SmestajnaJedinica smestajnaJedinica = smestajnaJedinicaService.GetById(idSmestajneJedinice);
                Smestaj smestaj = smestajService.GetById(idSmestaja);

                RezervacijaDto rezervacijaDto = new RezervacijaDto(aranzman, smestajnaJedinica, smestaj);
                return View("Index", rezervacijaDto);
            }

        }

        [HttpPost]

        public ActionResult Redirekcija(int idAranzmana, int idSmestajneJedinice)
        {
            Korisnik korisnik = (Korisnik)(Session["LOGGEDIN"]);

            if (korisnik.Uloga.Equals(Uloga.Turista))
            {
                ViewBag.Korisnik = (Turista)(Session["LOGGEDIN"]);
            }
            else
            {
                ViewBag.Korisnik = (Menadzer)(Session["LOGGEDIN"]);
            }

            Aranzman aranzman = aranzmanService.GetById(idAranzmana);
            SmestajnaJedinica smestajnaJedinica = smestajnaJedinicaService.GetById(idSmestajneJedinice);
            Smestaj smestaj = smestajService.GetById(aranzman.IdSmestaj);

            RezervacijaDto rezervacijaDto = new RezervacijaDto(aranzman, smestajnaJedinica, smestaj);


            return View("PrikazRezervacije", rezervacijaDto);
        }

        private TuristaDto kreirajDto()
        {
            var turista = (Turista)Session["LOGGEDIN"];

            List<Rezervacija> rezervacije = new List<Rezervacija>();
            rezervacije = rezervacijaService.SveRezervacijeKorisnika(turista.Id);

            List<Aranzman> rezervisaniAranzmani = new List<Aranzman>();//rezervisani aranzmani korisnika
            List<SmestajnaJedinica> rezervisaneJedinice = new List<SmestajnaJedinica>();//rezervisane smestajne jedinice korisnika

            foreach (var item in rezervacije)
            {
                var aranzman = aranzmanService.GetById(item.IdAranzman);
                if (aranzman != null && aranzman.Id != 0)
                {
                    rezervisaniAranzmani.Add(aranzman);
                }
                var jedinica = smestajnaJedinicaService.GetById(item.IdSmestajneJedinice);
                if (jedinica != null && jedinica.Id != 0)
                {
                    rezervisaneJedinice.Add(jedinica);
                }
            }

            TuristaDto turistaDto = new TuristaDto(turista, rezervisaniAranzmani, rezervisaneJedinice, rezervacije);

            return turistaDto;
        }
    }
}