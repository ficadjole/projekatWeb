using projekatWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projekatWeb.DTOs
{
	public class TuristaDto
	{

		public Korisnik Korisnik { get; set; }
        public List<Aranzman> Aranzmani { get; set; }

        public List<SmestajnaJedinica> SmestajneJedinice { get; set; }

        public List<Rezervacija> Rezervacije { get; set; }

        public TuristaDto(Korisnik korisnik, List<Aranzman> aranzmani,  List<SmestajnaJedinica> smestajneJedinice, List<Rezervacija> rezervacije)
        {
            Korisnik = korisnik;
            Aranzmani = aranzmani;
            SmestajneJedinice = smestajneJedinice;
            Rezervacije = rezervacije;
        }
    }
}