using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projekatWeb.Models
{
    [Serializable]
    public class SmestajnaJedinica
	{
		public int Id { get; set; }
		public int MaksBrOsoba { get; set; }
		public bool PetFriendly { get; set; }
		public int CenaJedinice { get; set; } //ukupna cena

        public bool Dostupnost { get; set; } //da li je slobodna ili ne

        public bool LogickoBrisanje { get; set; }

        public SmestajnaJedinica(int id, int maksBrOsoba, bool petFriendly, int cenaJedinice, bool dostupnost)
        {
            Id = id;
            MaksBrOsoba = maksBrOsoba;
            PetFriendly = petFriendly;
            CenaJedinice = cenaJedinice;
            LogickoBrisanje = false;
            Dostupnost = dostupnost;
        }

        public SmestajnaJedinica()
        {
            Id = 0;
        }
    }
}