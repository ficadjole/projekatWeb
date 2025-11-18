using projekatWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projekatWeb.DTOs
{
	public class RezervacijaDto
	{
		public Aranzman Aranzman { get; set; }
        public SmestajnaJedinica SmestajnaJedinica { get; set; }

        public Smestaj Smestaj { get; set; }

        public RezervacijaDto(Aranzman aranzman, SmestajnaJedinica smestajnaJedinica, Smestaj smestaj)
        {
            Aranzman = aranzman;
            SmestajnaJedinica = smestajnaJedinica;
            Smestaj = smestaj;
        }
    }
}