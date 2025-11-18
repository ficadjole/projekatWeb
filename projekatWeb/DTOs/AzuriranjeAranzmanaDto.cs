using projekatWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projekatWeb.DTOs
{
	public class AzuriranjeAranzmanaDto
	{

        public Aranzman Aranzman { get; set; }
        public Smestaj Smestaj { get; set; }

        public List<SmestajnaJedinica> SmestajnaJedinica { get; set; }

        public AzuriranjeAranzmanaDto(Aranzman aranzman, Smestaj smestaj, List<SmestajnaJedinica> smestajnaJedinica)
        {
            Aranzman = aranzman;
            Smestaj = smestaj;
            SmestajnaJedinica = smestajnaJedinica;
        }

    }


}