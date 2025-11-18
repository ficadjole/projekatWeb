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
	public class AranzmanService
    {
        private readonly string filePath;
        private List<Aranzman> aranzmanRepository;

		public AranzmanService(string filePath) {
            this.filePath = filePath;
        }


		public void UcitajAranzman()
		{
            if (File.Exists(HostingEnvironment.MapPath(filePath)))
            {

                var jsonPodaci = File.ReadAllText(HostingEnvironment.MapPath(filePath));

                aranzmanRepository = JsonConvert.DeserializeObject<List<Aranzman>>(jsonPodaci);
                //deserializuje objekte iz jsona

            }
            else
            {
                aranzmanRepository = new List<Aranzman>();
            }
        }

        public void SacuvajAranzman()
        {
            if (File.Exists(HostingEnvironment.MapPath(filePath)))
            {
                var jsonPodaci = JsonConvert.SerializeObject(aranzmanRepository, Formatting.Indented); //Formatting.Indented mi omogucava da json bude formatiran
                                                                                                                //TypeNameHandling omogucava da mi se lepo ucitaju i Moderator i Turista kao i njhove liste
                File.WriteAllText(HostingEnvironment.MapPath(filePath), jsonPodaci);
            }
        }

        public List<Aranzman> GetAllAranzman()
		{
			return aranzmanRepository;
		}

		public Aranzman GetById(int id)
		{
			foreach (var item in aranzmanRepository)
			{
				if (item.Id == id)
					return item;
			}

			return new Aranzman();
		}

		public Aranzman DodajAranzman(Aranzman aranzman)
		{
			Aranzman postojeci = GetById(aranzman.Id);

			if(postojeci.Id == 0)
			{
				new Aranzman();
			}

			aranzmanRepository.Add(aranzman);
			SacuvajAranzman();
			return aranzman;
		}

		public Aranzman LogickiObrisiAranzman(Aranzman aranzman) {

            Aranzman postojeci = GetById(aranzman.Id);

            if (postojeci.Id == 0)
            {
                new Aranzman();
            }

			aranzman.LogickoBrisanje = true;

			AzurirajAranzman(aranzman);

			return aranzman;

        }

		public Aranzman AzurirajAranzman(Aranzman aranzman) { 
		
			Aranzman postojeci = GetById(aranzman.Id);

			if( postojeci.Id == 0) { return new Aranzman(); }

			for (int i = 0; i < aranzmanRepository.Count(); i++) {

				if (aranzmanRepository[i].Id == aranzman.Id) {

					aranzmanRepository[i] = aranzman;
					SacuvajAranzman();
					return aranzman;
				}

			}

			return new Aranzman();
		
		} 

		public List<Aranzman> GetByTipAranzmana(TipAranzmana tip)
		{
			List<Aranzman> aranzmani = new List<Aranzman>();

            foreach (var item in aranzmanRepository)
            {
				if (item.TipAranzmana.Equals(tip))
				{
					aranzmani.Add(item);
				}
            }

			return aranzmani;
        }

        public List<Aranzman> GetByTipPrevoza(TipPrevoza tip)
        {
            List<Aranzman> aranzmani = new List<Aranzman>();

            foreach (var item in aranzmanRepository)
            {
                if (item.TipAranzmana.Equals(tip))
                {
                    aranzmani.Add(item);
                }
            }

            return aranzmani;
        }

		public List<Aranzman> GetByContainsName(string naziv)
		{
			if (string.IsNullOrEmpty(naziv)) return new List<Aranzman>();

			List<Aranzman> aranzmani = new List<Aranzman>();
			foreach (var item in aranzmanRepository)
			{
				if (item.NazivAranzmana.ToLower().Contains(naziv.ToLower()))
				{
					aranzmani.Add(item);
				}
			}

			return aranzmani;
		}

		public Aranzman GetByNaziv(string naziv)
		{
			if (string.IsNullOrEmpty(naziv)) return new Aranzman();

            foreach (var item in aranzmanRepository)
            {
				if (item.NazivAranzmana.Equals(naziv))
				{
					return item;
				}
            }

			return new Aranzman();
        }

		public int GetNextId()
		{
			return aranzmanRepository.Count() + 1;
        }
    }
}