using Microsoft.Ajax.Utilities;
using projekatWeb.DTOs;
using projekatWeb.Models;
using projekatWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projekatWeb.Controllers
{
    public class TuristaController : Controller
    {
        KorisnikService korisnikService;
        AranzmanService aranzmanService;
        SmestajnaJedinicaService smestajnaJedinicaService;
        SmestajService smestajService;
        RezervacijaService rezervacijaService;

        public TuristaController()
        {
            korisnikService = (KorisnikService)System.Web.HttpContext.Current.Application["KorisniciService"];
            aranzmanService = (AranzmanService)System.Web.HttpContext.Current.Application["AranzmaniService"];
            smestajnaJedinicaService = (SmestajnaJedinicaService)System.Web.HttpContext.Current.Application["SmestajnaJedinicaService"];
            smestajService = (SmestajService)System.Web.HttpContext.Current.Application["SmestajService"];
            rezervacijaService = (RezervacijaService)System.Web.HttpContext.Current.Application["RezervacijaService"];
        }

        // GET: Turista
        public ActionResult Panel()
        {

            TuristaDto turistaDto = kreirajDto();

            return View(turistaDto);
        }

        public ActionResult Otkazi(int idRezervacije)
        {
            var rezervacija = rezervacijaService.GetById(idRezervacije); //dobio sam ovu rezervaciju koja se otkazuje

            var uspesno = rezervacijaService.OtkaziRezervaciju(rezervacija.IdRezervacije); //stavljamo status rez na false

            var smestajnaJed = smestajnaJedinicaService.setDostupnost(rezervacija.IdSmestajneJedinice, true); //stavljamo da jedinica bude dostpna

            if(uspesno && smestajnaJed.Id != 0)
            {
                ViewBag.Poruka = "Uspesno ste otkazali rezervaciju.";
                TuristaDto turistaDto = kreirajDto();
                return View("Panel", turistaDto);
            }
            else
            {
                ViewBag.Neuspesno = "Doslo je do greske prilikom otkazivanja rezervacije.";
                TuristaDto turistaDto = kreirajDto();
                return View("Panel", turistaDto);
            }

        }
        [HttpPost]
        public ActionResult Pretraga(string idRezervacije,string nazivAranzmana, string status, string akcija)
        {
            Turista turista = (Turista)Session["LOGGEDIN"];
            if (akcija.Equals("osvezi"))//ako treba da se osvezi samo ce uzeti sve podatke i tjt
            {
                TuristaDto turistaDto = kreirajDto();
                return View("Panel", turistaDto);
            }


            //string status ako nije cekirano bice null onda znam da hoce one koji su prosli
            int id;

            List<Aranzman> listaZaDto = new List<Aranzman>();
            List<Rezervacija> rezervacijeZaDto = new List<Rezervacija>();

            List<Aranzman> listaAranzmanaIme = new List<Aranzman>();
            if (!nazivAranzmana.Trim().IsNullOrWhiteSpace())
            {
                listaAranzmanaIme = aranzmanService.GetByContainsName(nazivAranzmana);
            }

            if(listaAranzmanaIme.Count == 0 && !nazivAranzmana.Trim().IsNullOrWhiteSpace() && idRezervacije.Length != 0 && (status == null || status.Length != 0))
            {
                ViewBag.Neuspesno = "Nema aranžmana sa unetim nazivom.";
                TuristaDto turistaDto = kreirajDto();
                return View("Panel", turistaDto);
            }

            if (status == "true")
            {

                //ovo moram ovako jer nemam u aranzmanu status
                rezervacijeZaDto = rezervacijaService.GetByStatus(true, turista.Id);

                if (!idRezervacije.Trim().IsNullOrWhiteSpace() && Int32.TryParse(idRezervacije, out id))
                {
                    for (int i = 0; i < rezervacijeZaDto.Count; i++)
                    {
                        if (rezervacijeZaDto[i].IdRezervacije != id)
                        {
                            rezervacijeZaDto.Remove(rezervacijeZaDto[i]);//izbacujemo sve koji nisu taj
                            i--;
                        }
                    }
                }

                if (rezervacijeZaDto.Count == 0)
                {
                    ViewBag.Neuspesno = "Nema aktivnih rezervacija.";
                    TuristaDto turistaDto = kreirajDto();
                    return View("Panel", turistaDto);
                }
                else
                {

                    //moguce da je uneo samo naziv
                    if (listaAranzmanaIme.Count != 0)
                    {
                        foreach (var item in rezervacijeZaDto)
                        {
                            foreach (var ar in listaAranzmanaIme)
                            {
                                if (ar.Id == item.IdAranzman)
                                {
                                    var aranzman = aranzmanService.GetById(item.IdAranzman);
                                    listaZaDto.Add(aranzman); //ovde smo gledali da je korisnik uneo samo naziv i da je rezervacija otkazana
                                }
                            }
                        }
                    }
                    else
                    {
                        //nije uneo ni id ni naziv nego samo status
                        foreach (var item in rezervacijeZaDto)
                        {
                            var aranzman = aranzmanService.GetById(item.IdAranzman);
                            listaZaDto.Add(aranzman);
                        }
                    }
                }

            }
            else if (status == "false")
            {
                rezervacijeZaDto = rezervacijaService.GetByStatus(false, turista.Id);

                if (!idRezervacije.Trim().IsNullOrWhiteSpace() && Int32.TryParse(idRezervacije, out id))
                {
                    for (int i = 0; i < rezervacijeZaDto.Count; i++)
                    {
                        if (rezervacijeZaDto[i].IdRezervacije != id)
                        {
                            rezervacijeZaDto.Remove(rezervacijeZaDto[i]);//izbacujemo sve koji nisu taj
                            i--;
                        }
                    }
                }

                if (rezervacijeZaDto.Count == 0)
                {
                    ViewBag.Neuspesno = "Nema otkazanih rezervacija.";
                    TuristaDto turistaDto = kreirajDto();
                    return View("Panel", turistaDto);
                }
                else
                {
                   
                        //moguce da je uneo samo naziv
                        if (listaAranzmanaIme.Count != 0)
                        {
                            foreach (var item in rezervacijeZaDto)
                            {
                                foreach (var ar in listaAranzmanaIme)
                                {
                                    if (ar.Id == item.IdAranzman)
                                    {
                                        var aranzman = aranzmanService.GetById(item.IdAranzman);
                                        listaZaDto.Add(aranzman); //ovde smo gledali da je korisnik uneo samo naziv i da je rezervacija otkazana
                                    }
                                }
                            }
                        }
                        else
                        {
                            //nije uneo ni id ni naziv nego samo status
                            foreach (var item in rezervacijeZaDto)
                            {
                                var aranzman = aranzmanService.GetById(item.IdAranzman);
                                listaZaDto.Add(aranzman);
                            }
                        }
                    
                }

            }
            else
            {
                rezervacijeZaDto = rezervacijaService.SveRezervacijeKorisnika(turista.Id);

                if (!idRezervacije.Trim().IsNullOrWhiteSpace() && Int32.TryParse(idRezervacije, out id))
                {

                    for (int i = 0; i < rezervacijeZaDto.Count; i++)
                    {
                        if (rezervacijeZaDto[i].IdRezervacije != id)
                        {
                            rezervacijeZaDto.Remove(rezervacijeZaDto[i]);//izbacujemo sve koji nisu taj
                            i--;
                        }
                    }

                }

                if (rezervacijeZaDto.Count == 0)
                {
                    ViewBag.Neuspesno = "Nema otkazanih rezervacija.";
                    return View("Panel");
                }
                else
                {
                    
                        //moguce da je uneo samo naziv
                        if (listaAranzmanaIme.Count != 0)
                        {
                            foreach (var item in rezervacijeZaDto)
                            {
                                foreach (var ar in listaAranzmanaIme)
                                {
                                    if (ar.Id == item.IdAranzman)
                                    {
                                        var aranzman = aranzmanService.GetById(item.IdAranzman);
                                        listaZaDto.Add(aranzman); //ovde smo gledali da je korisnik uneo samo naziv i da je rezervacija otkazana
                                    }
                                }
                            }
                        }
                        else
                        {
                            //nije uneo ni id ni naziv nego samo status
                            foreach (var item in rezervacijeZaDto)
                            {
                                var aranzman = aranzmanService.GetById(item.IdAranzman);
                                listaZaDto.Add(aranzman);
                            }
                        }
                    
                }
            }


                List<SmestajnaJedinica> rezervisaneJedinice = new List<SmestajnaJedinica>();//rezervisane smestajne jedinice korisnika

            foreach (var item in rezervacijeZaDto)
            {
                var jedinica = smestajnaJedinicaService.GetById(item.IdSmestajneJedinice);
                if (jedinica != null && jedinica.Id != 0)
                {
                    rezervisaneJedinice.Add(jedinica);
                }
            }

            TuristaDto turistaDto1 = new TuristaDto((Turista)Session["LOGGEDIN"], listaZaDto, rezervisaneJedinice, rezervacijeZaDto);

            return View("Panel",turistaDto1);
        }

        
        public ActionResult SortirajName(string vrsta)
        {
            Turista turista = (Turista)Session["LOGGEDIN"];

            // sve rezervacije korisnika
            var rezervacije = rezervacijaService.SveRezervacijeKorisnika(turista.Id);

            // sortiraj rezervacije po nazivu aranžmana
            IEnumerable<Rezervacija> sortiraneRezervacije;
            if (vrsta == "asc")
            {
                sortiraneRezervacije = rezervacije
                    .OrderBy(r => aranzmanService.GetById(r.IdAranzman).NazivAranzmana);
            }
            else
            {
                sortiraneRezervacije = rezervacije
                    .OrderByDescending(r => aranzmanService.GetById(r.IdAranzman).NazivAranzmana);
            }

            // iz sortirane liste rezervacija dobijaš ostale podatke
            List<Aranzman> smestaji = new List<Aranzman>();
            List<SmestajnaJedinica> rezervisaneJedinice = new List<SmestajnaJedinica>();

            foreach (var rez in sortiraneRezervacije)
            {
                var aranzman = aranzmanService.GetById(rez.IdAranzman);
                if (aranzman != null && aranzman.Id != 0)
                    smestaji.Add(aranzman);

                var jedinica = smestajnaJedinicaService.GetById(rez.IdSmestajneJedinice);
                if (jedinica != null && jedinica.Id != 0)
                    rezervisaneJedinice.Add(jedinica);
            }

            TuristaDto turistaDto = new TuristaDto(turista, smestaji, rezervisaneJedinice, sortiraneRezervacije.ToList());

            return View("Panel", turistaDto);
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