using projekatWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projekatWeb.DTOs
{
    public class MenadzerDto
    {
        public List<Rezervacija> Rezervacije { get; set; }
        public List<Aranzman> KreiraniAranzmani { get; set; }
        public List<Smestaj> KreiraniSmestaji { get; set; }
        public List<SmestajnaJedinica> KreiraneSmestajneJedinice { get; set; }
        public List<Korisnik> KorisniciRez {  get; set; }

        public List<Komentar> Komentar { get; set; }

        public MenadzerDto(List<Rezervacija> rezervacije, List<Aranzman> kreiraniAranzmani, List<Smestaj> kreiraniSmestaji, List<SmestajnaJedinica> kreiraneSmestajneJedinice, List<Korisnik> korisniciRez, List<Komentar> komentar)
        {
            Rezervacije = rezervacije;
            KreiraniAranzmani = kreiraniAranzmani;
            KreiraniSmestaji = kreiraniSmestaji;
            KreiraneSmestajneJedinice = kreiraneSmestajneJedinice;
            KorisniciRez = korisniciRez;
            Komentar = komentar;
        }
    }
}