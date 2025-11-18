using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projekatWeb.Models
{
    [Serializable]
    public enum TipSmestaja
	{
        hotel,
		motel,
		vila
    }
    [Serializable]
    public class Smestaj
	{
        public int Id { get; set; }
		public TipSmestaja TipSmestaja { get; set; }
        public string NazivSmestaja { get; set; }
        public int BrojZvezdica { get; set; } //1-5

		public bool Bazen { get; set; }
		public bool SpaCentar { get; set; }
		public bool OsobeSaInvaliditetom { get; set; }
		public bool Wifi { get; set; }

        public bool LogickoBrisanje { get; set; }

        public List<int> SmestajneJedinice { get; set; }
        public Smestaj(int id,TipSmestaja tipSmestaja, string nazivSmestaja, int brojZvezdica, bool bazen, bool spaCentar, bool osobeSaInvaliditetom, bool wifi, List<int> smestajneJedinice)
        {
            Id = id;
            TipSmestaja = tipSmestaja;
            NazivSmestaja = nazivSmestaja;
            BrojZvezdica = brojZvezdica;
            Bazen = bazen;
            SpaCentar = spaCentar;
            OsobeSaInvaliditetom = osobeSaInvaliditetom;
            Wifi = wifi;
            LogickoBrisanje = false;
            SmestajneJedinice = smestajneJedinice ?? new List<int>();
        }

        public Smestaj()
        {
            Id = 0;
            SmestajneJedinice = new List<int>();
        }

        

    }
}