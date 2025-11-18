using projekatWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace projekatWeb.DTOs
{
	public class AranzmanDetailsDto
	{
        public AranzmanDetailsDto()
        {
        }

        public Aranzman Aranzman { get; set; }
		public Smestaj Smestaj { get; set; }

		public List<SmestajnaJedinica> SmestajnaJedinica { get; set; }

		public List<Komentar> Komentari { get; set; }

        public List<Korisnik> Korisnici { get; set; }

        public List<Rezervacija> Rezervacije { get; set; }
        public AranzmanDetailsDto(Aranzman aranzman, Smestaj smestaj, List<SmestajnaJedinica> smestajnaJedinica, List<Komentar> komentari, List<Korisnik> korisnici, List<Rezervacija> rezervacije)
        {
            Aranzman = aranzman;
            Smestaj = smestaj;
            SmestajnaJedinica = smestajnaJedinica;
            Komentari = komentari;
            Korisnici = korisnici;
            Rezervacije = rezervacije;
        }
    }
}