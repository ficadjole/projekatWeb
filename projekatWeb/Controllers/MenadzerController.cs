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
    public class MenadzerController : Controller
    {
        // GET: Menadzer

        AranzmanService aranzmanService;
        SmestajService smestajService;
        SmestajnaJedinicaService smestajnaJedinicaService;
        KomentarService komentarService;
        RezervacijaService rezervacijaService;
        KorisnikService korisnikService;

        public MenadzerController()
        {
            korisnikService = (KorisnikService)System.Web.HttpContext.Current.Application["KorisniciService"];
            rezervacijaService = (RezervacijaService)System.Web.HttpContext.Current.Application["RezervacijaService"];
            aranzmanService = (AranzmanService)System.Web.HttpContext.Current.Application["AranzmaniService"];
            smestajnaJedinicaService = (SmestajnaJedinicaService)System.Web.HttpContext.Current.Application["SmestajnaJedinicaService"];
            komentarService = (KomentarService)System.Web.HttpContext.Current.Application["KomentarService"];
            smestajService = (SmestajService)System.Web.HttpContext.Current.Application["SmestajService"];
        }

        public ActionResult Panel()
        {

            MenadzerDto dto = kreirajDto();

            return View(dto);
        }

        private MenadzerDto kreirajDto()
        {
            Menadzer menadzer = (Menadzer)Session["LOGGEDIN"];

            var kreiraniAranzmaniId = menadzer.KreiraniAranzmani;

            List<Aranzman> kreiraniAranzmani = new List<Aranzman>();
            List<Rezervacija> rezervacijeZaAranzmane = new List<Rezervacija>();

            foreach (int id in kreiraniAranzmaniId)
            {
                var aranzman = aranzmanService.GetById(id);

                if (aranzman.Id != 0)
                {

                    kreiraniAranzmani.Add(aranzman);

                    List<Rezervacija> rez = rezervacijaService.GetByIdAranzmana(aranzman.Id);

                    rezervacijeZaAranzmane.AddRange(rez);//dodajemo svaki put elemnte iz liste 
                }

            }

            List<Korisnik> korisniciKojiSuRezervisali = new List<Korisnik>();

            foreach (Rezervacija rez in rezervacijeZaAranzmane)
            {
                var korisnik = korisnikService.GetById(rez.IdTurista);

                if (korisnik.Id != 0)
                {
                    korisniciKojiSuRezervisali.Add(korisnik);
                }
            }

            List<Smestaj> kreiraniSmestaji = new List<Smestaj>();
            List<Komentar> kreiraniKomentari = new List<Komentar>();

            foreach (Aranzman item in kreiraniAranzmani)
            {
                var smestaj = smestajService.GetById(item.IdSmestaj);

                if (smestaj.Id != 0)
                {
                    kreiraniSmestaji.Add(smestaj);

                    List<Komentar> kom = komentarService.SviKomentariSmestaja(smestaj.Id);

                    kreiraniKomentari.AddRange(kom);

                }
            }

            List<SmestajnaJedinica> keriranaSmestajnaJedinica = new List<SmestajnaJedinica>();

            foreach (Smestaj item in kreiraniSmestaji)
            {
                foreach (int id in item.SmestajneJedinice)
                {
                    var smestajnaJedinica = smestajnaJedinicaService.GetById(id);
                    if (smestajnaJedinica.Id != 0)
                    {
                        keriranaSmestajnaJedinica.Add(smestajnaJedinica);
                    }
                }
            }

            MenadzerDto dto = new MenadzerDto(rezervacijeZaAranzmane, kreiraniAranzmani, kreiraniSmestaji, keriranaSmestajnaJedinica, korisniciKojiSuRezervisali, kreiraniKomentari);

            return dto;
        }
    }
}