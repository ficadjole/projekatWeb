using Newtonsoft.Json;
using projekatWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace projekatWeb.Services
{
	public class KorisnikService
	{

		private readonly string filePath; //ovo je globalno za servis jer ce nam trebati kasnije prilikom upisivanja podataka nazad u fajl 
		private List<Korisnik> korisnikRepository;

		public KorisnikService(string filePath)
		{
			this.filePath = filePath;

		}

		public void UcitajKorisnike()
		{
			if (File.Exists(HostingEnvironment.MapPath(filePath))) {

				var jsonPodaci = File.ReadAllText(HostingEnvironment.MapPath(filePath));

				korisnikRepository = JsonConvert.DeserializeObject<List<Korisnik>>(jsonPodaci, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                //deserializuje objekte iz jsona
				//TypeNameHandling omogucava da mi se lepo ucitaju i Moderator i Turista kao i njhove liste

            }
            else
			{
				korisnikRepository = new List<Korisnik>();
			}
		}

		//ovo ce se desavati nakon svake dodele novog korisnika ili kada se izloguje to cu videti jos
		public void SacuvajKorisnike()
		{
			if (File.Exists(HostingEnvironment.MapPath(filePath)))
			{
				var jsonPodaci = JsonConvert.SerializeObject(korisnikRepository,Formatting.Indented,
					new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }); //Formatting.Indented mi omogucava da json bude formatiran
                                                                                             //TypeNameHandling omogucava da mi se lepo ucitaju i Moderator i Turista kao i njhove liste
                File.WriteAllText(HostingEnvironment.MapPath(filePath), jsonPodaci);
			}
		}

		public Korisnik GetById(int id)
		{
			foreach (var item in korisnikRepository) { 
				if (item.Id == id) 
					return item; 
			} 
			return new Korisnik(); //necemo vracati null zbog null exception
		}

		public Korisnik AuthKorisnika(string korisnickoIme, string password)
        {
            foreach (var item in korisnikRepository)
            {
				if(item.KorisnickoIme.Equals(korisnickoIme) && item.Lozinka.Equals(password))
                {
                    return item;
                }
            }
            return new Korisnik(); //necemo vracati null zbog null exception
        }

		public Korisnik GetByKorisnickoIme(string korisnickoIme)
		{
            foreach (var item in korisnikRepository)
            {
                if (item.KorisnickoIme.Equals(korisnickoIme))
                {
                    return item;
                }
            }
            return new Korisnik(); //necemo vracati null zbog null exception
        }

        public Korisnik DodajKorisnika(Korisnik korisnik) { 
		
			Korisnik postojeci = GetById(korisnik.Id);

			if(postojeci.Id != 0) { return new Korisnik(); }

			korisnikRepository.Add(korisnik);
            SacuvajKorisnike();
            return korisnik;

		}

		public List<Korisnik> GetAll()
		{
			return korisnikRepository;
		}
		
		public int GetNextIndex()
		{
			return korisnikRepository.Count + 1;
        }

		//kada dodajem rezervisan aranzman mora da postoji 
		public bool DodajRezervisaniAranzman(int idKorisnika, int idAranzmana)
		{
			Korisnik korisnik = GetById(idKorisnika);
			if (korisnik.Id == 0 || korisnik.Uloga != Uloga.Turista)
			{
				return false;
			}

			Turista turista = (Turista)korisnik;

            if (turista.RezervisaniAranazmani.Contains(idAranzmana))
            {
                return false;
            }


            turista.RezervisaniAranazmani.Add(idAranzmana);

			SacuvajKorisnike();

			return true;
		}

		public Korisnik AzurirajKorisnika(Korisnik azuriraniKorisnik)
		{
			Korisnik korisnik = GetById(azuriraniKorisnik.Id);
            //proveravamo da li postoji korisnik sa tim id
            if (korisnik.Id == 0)
            {
                return new Korisnik();
            }

            for (int i = 0; i < korisnikRepository.Count; i++)
            {
                if(korisnikRepository[i].Id == azuriraniKorisnik.Id)
                {
                    korisnikRepository[i].Ime = azuriraniKorisnik.Ime;
                    korisnikRepository[i].Prezime = azuriraniKorisnik.Prezime;
                    korisnikRepository[i].Email = azuriraniKorisnik.Email;
                    korisnikRepository[i].KorisnickoIme = azuriraniKorisnik.KorisnickoIme;
                    korisnikRepository[i].Lozinka = azuriraniKorisnik.Lozinka;
                    korisnikRepository[i].DatumRodjenja = azuriraniKorisnik.DatumRodjenja;

                    SacuvajKorisnike();

					return korisnikRepository[i];

                }
            }

            return new Korisnik();
        }

    }
}