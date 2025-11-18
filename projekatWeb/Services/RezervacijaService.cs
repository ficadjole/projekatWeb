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
	public class RezervacijaService
	{
		private List<Rezervacija> rezervacijaRepository;
		private readonly string filePath;

		public RezervacijaService(string filePath)
		{
			this.filePath = filePath;
		}

		public void UcitajRezervacija()
		{
            if (File.Exists(HostingEnvironment.MapPath(filePath)))
            {

                var jsonPodaci = File.ReadAllText(HostingEnvironment.MapPath(filePath));

                rezervacijaRepository = JsonConvert.DeserializeObject<List<Rezervacija>>(jsonPodaci);
                //deserializuje objekte iz jsona

            }
            else
            {
                rezervacijaRepository = new List<Rezervacija>();
            }
        }

        public void SacuvajRezervacija()
        {

            if (File.Exists(HostingEnvironment.MapPath(filePath)))
            {
                var jsonPodaci = JsonConvert.SerializeObject(rezervacijaRepository, Formatting.Indented); //Formatting.Indented mi omogucava da json bude formatiran
                File.WriteAllText(HostingEnvironment.MapPath(filePath), jsonPodaci);
            }

        }

        public List<Rezervacija> GetAll()
        {
            return rezervacijaRepository;
        }

        public Rezervacija GetById(int id)
        {
            foreach (var item in rezervacijaRepository)
            {
                if(item.IdRezervacije == id)
                {
                    return item;
                }
            }

            return new Rezervacija();
        }

        public Rezervacija DodajRezervaciju(Rezervacija rezervacija)
        {

            Rezervacija postojeci = GetById(rezervacija.IdRezervacije);

            if(postojeci.IdRezervacije != 0)
            {
                return new Rezervacija();
            }

            rezervacijaRepository.Add(rezervacija);
            SacuvajRezervacija();

            return rezervacija;
        }

        public List<Rezervacija> SveRezervacijeKorisnika(int idKorisnika) { 
        
            List<Rezervacija> rezervacije = new List<Rezervacija>();

            foreach (var item in rezervacijaRepository)
            {
                if(item.IdTurista == idKorisnika)
                {
                    rezervacije.Add(item);
                }
            }

            return rezervacije;

        }

        public int GetNextId()
        {
            return rezervacijaRepository.Count + 1;
        }

        public bool OtkaziRezervaciju(int idRezervacije)
        {
            var rezervacija = GetById(idRezervacije);
            if (rezervacija.IdRezervacije != 0)
            {
                rezervacija.Status = false; //otkazana
                SacuvajRezervacija();
                return true;
            }

            return false;
        }

        public List<Rezervacija> GetByStatus(bool status, int idTuriste)
        {
            List<Rezervacija> rezervacije = new List<Rezervacija>();
            foreach (var item in rezervacijaRepository)
            {
                if (item.Status == status && item.IdTurista == idTuriste)
                {
                    rezervacije.Add(item);
                }
            }
            return rezervacije;
        }

        public List<Rezervacija> GetByIdAranzmana(int idAranzmana)
        {
            List<Rezervacija> rezervacije = new List<Rezervacija>();

            foreach(var item in rezervacijaRepository)
            {
                if(item.IdAranzman == idAranzmana)
                {
                    rezervacije.Add(item);
                }
            }

            return rezervacije;
        }

        public List<Rezervacija> GetByIdSmestajneJedinice(int idSmestajneJedinice)
        {
            List<Rezervacija> rezervacije = new List<Rezervacija>();
            foreach (var item in rezervacijaRepository)
            {
                if (item.IdSmestajneJedinice == idSmestajneJedinice)
                {
                    rezervacije.Add(item);
                }
            }
            return rezervacije;
        }

    }
}