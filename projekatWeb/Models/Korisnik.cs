using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projekatWeb.Models
{
    [Serializable]
    public enum Uloga
    {
        Administrator,
        Menadzer,
        Turista
    }
    [Serializable]
    public enum Pol
    {
        Muski,
        Zenski
    }
    [Serializable]
    public class Korisnik
	{
        public Korisnik()
        {
            Id = 0;
        }

        public int Id { get; set; }
		public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public Pol Pol { get; set; }
        public string Email { get; set; }
        public string DatumRodjenja { get; set; } //stavio sam string zbog formata dd/mm/yyyy
        public Uloga Uloga { get; set; }

        public Korisnik(int id, string korisnickoIme, string lozinka, string ime, string prezime, Pol pol, string email, string datumRodjenja, Uloga uloga)
        {
            Id = id;
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            Email = email;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
        }
    }

    [Serializable]
    public class Turista : Korisnik
    {
        public List<int> RezervisaniAranazmani { get; set; }//za turistu

        public Turista()
        {
            Id = 0;
            Uloga = Uloga.Turista;
            RezervisaniAranazmani = new List<int>();
        }
        public Turista(int id, string korisnickoIme, string lozinka, string ime, string prezime, Pol pol, string email, string datumRodjenja, Uloga uloga, List<int> rezervisaniAranazmani) : base(id,korisnickoIme, lozinka, ime, prezime, pol, email, datumRodjenja, uloga)
        {
            RezervisaniAranazmani = rezervisaniAranazmani ?? new List<int>(); //ako je rezervisaniAranzmani null onda ce samo napraviti praznu listu
        }

    }

    [Serializable]
    public class Menadzer : Korisnik
    {
        public List<int> KreiraniAranzmani { get; set; }//za turistu

        public Menadzer()
        {
            Id = 0;
            Uloga = Uloga.Menadzer;
            KreiraniAranzmani = new List<int>();
        }
        public Menadzer(int id, string korisnickoIme, string lozinka, string ime, string prezime, Pol pol, string email, string datumRodjenja, Uloga uloga, List<int> kreiraniAranzmani) : base(id, korisnickoIme, lozinka, ime, prezime, pol, email, datumRodjenja, uloga)
        {
            KreiraniAranzmani = kreiraniAranzmani ?? new List<int>(); //isto kao i gore
        }

    }
}