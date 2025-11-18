using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projekatWeb.Models
{
    [Serializable]
    public enum TipAranzmana
    {
        nocenje_sa_doruckom,
        polupansion,
        pun_pansion,
        all_inclusive,
        najam_apartmana
    }
    [Serializable]
    public enum TipPrevoza
    {
        autobus,
        avion,
        autobus_i_avion,
        individualan,
        ostalo
    }
    [Serializable]
    public class Aranzman
	{
        public int Id { get; set; }
		public string NazivAranzmana { get; set; }
		
		public TipAranzmana TipAranzmana { get; set; }
    
        public TipPrevoza TipPrevoza { get; set; }

        public string Destinacija { get; set; }

        public string DatumPolaska { get; set; } //stavio sam string zbog formata dd/mm/yyyy
        public string DatumPovratka { get; set; } //stavio sam string zbog formata dd/mm/yyyy
    
        public int MaksBrPutnika { get; set; }
        public string OpisAranzmana { get; set; }
        public string ProgramAranzmana { get; set; }
        public string PosterAranzmanaUrl { get; set; } //url slike

        public int IdSmestaj { get; set; } //naziv smestaja

        public bool LogickoBrisanje { get; set; }

        public Aranzman(int id, string nazivAranzmana, TipAranzmana tipAranzmana, TipPrevoza tipPrevoza, string destinacija, string datumPolaska, string datumPovratka, int maksBrPutnika, string opisAranzmana, string programAranzmana, string posterAranzmanaUrl, int idSmestaj)
        {
            Id = id;
            NazivAranzmana = nazivAranzmana;
            TipAranzmana = tipAranzmana;
            TipPrevoza = tipPrevoza;
            Destinacija = destinacija;
            DatumPolaska = datumPolaska;
            DatumPovratka = datumPovratka;
            MaksBrPutnika = maksBrPutnika;
            OpisAranzmana = opisAranzmana;
            ProgramAranzmana = programAranzmana;
            PosterAranzmanaUrl = posterAranzmanaUrl;
            IdSmestaj = idSmestaj;
            LogickoBrisanje = false;
        }

        public Aranzman()
        {
            Id = 0;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}