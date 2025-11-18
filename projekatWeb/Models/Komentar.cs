using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projekatWeb.Models
{
    [Serializable]
    public class Komentar
	{
        public Komentar()
        {
            Id = 0;
        }

        public int Id { get; set; }
		public int IdKorisnika { get; set; }//mora da je ucestvoao na putovanju
		public int IdSmestaja { get; set; }//na koji se odnosi komentar
        public string TekstKomentara { get; set; }
        public int Ocena { get; set; } //1-5
        public bool Odobren { get; set; } //da li je komentar odobren od strane moderatora

        public Komentar(int id, int idKorisnika, int idSmestaj, string tekstKomentara, int ocena, bool odobren)
        {
            Id = id;
            IdKorisnika = idKorisnika;
            IdSmestaja = idSmestaj;
            TekstKomentara = tekstKomentara;
            Ocena = ocena;
            Odobren = odobren;
        }
    }
}