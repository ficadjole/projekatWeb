using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projekatWeb.Models
{
    [Serializable]
    public class Rezervacija
	{
		public int IdRezervacije { get; set; }
		public int IdTurista { get; set; }

		public bool Status { get; set; } //true - aktivna, false - otkazana

        public int IdAranzman { get; set; } //izabrani aranzman

        public int IdSmestajneJedinice { get; set; } //izabrana smestajna jedinica

        public Rezervacija(int idRezervacije, int idTurista, bool status, int idAranzman, int idSmestajneJedinice)
        {
            IdRezervacije = idRezervacije;
            IdTurista = idTurista;
            Status = status;
            IdAranzman = idAranzman;
            IdSmestajneJedinice = idSmestajneJedinice;
        }

        public Rezervacija()
        {
            IdRezervacije = 0;
        }
    }
}