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
	public class SmestajService
	{

		private List<Smestaj> smestajRepository;
        private readonly string filePath;

        public SmestajService(string filePath)
		{
			this.filePath = filePath;
		}

        public void UcitajSmestaj()
        {
            if (File.Exists(HostingEnvironment.MapPath(filePath)))
            {

                var jsonPodaci = File.ReadAllText(HostingEnvironment.MapPath(filePath));

                smestajRepository = JsonConvert.DeserializeObject<List<Smestaj>>(jsonPodaci);
                //deserializuje objekte iz jsona
               
            }
            else
            {
                smestajRepository = new List<Smestaj>();
            }
        }

        public void SacuvajSmestaj()
        {
            if (File.Exists(HostingEnvironment.MapPath(filePath)))
            {
                var jsonPodaci = JsonConvert.SerializeObject(smestajRepository, Formatting.Indented); //Formatting.Indented mi omogucava da json bude formatiran
                File.WriteAllText(HostingEnvironment.MapPath(filePath), jsonPodaci);
            }
        }

        public List<Smestaj> GetAll()
        {

            return smestajRepository;

        }

        public Smestaj GetById(int id)
		{
			foreach (var item in smestajRepository) { 
					
				if(item.Id == id)
				{
					return item;
				}
				
			}

			return new Smestaj();
		}

		public Smestaj DodajSmestaj(Smestaj smestaj) { 
		
			Smestaj postojeci = GetById(smestaj.Id);

			if(postojeci.Id != 0)
			{
				return new Smestaj();
			}

			smestajRepository.Add(smestaj);
			SacuvajSmestaj();
			return smestaj;

		}

		public Smestaj LogickiObrisi(int idSmestaja) { 
		
			Smestaj postojeci = GetById(idSmestaja);

			if (postojeci.Id == 0)
			{
				return new Smestaj();
			}

            postojeci.LogickoBrisanje = true;

			AzurirajSmestaj(postojeci);
            SacuvajSmestaj();
            return postojeci;

		}

		public Smestaj AzurirajSmestaj(Smestaj smestaj) {

            Smestaj postojeci = GetById(smestaj.Id);

            if (postojeci.Id == 0)
            {
                return new Smestaj();
            }

            for (int i = 0; i < smestajRepository.Count; i++)
            {
				if (smestajRepository[i].Id == smestaj.Id)
				{
					smestajRepository[i].LogickoBrisanje = smestaj.LogickoBrisanje;
                    smestajRepository[i].TipSmestaja = smestaj.TipSmestaja;
                    smestajRepository[i].NazivSmestaja = smestaj.NazivSmestaja;
                    smestajRepository[i].BrojZvezdica = smestaj.BrojZvezdica;
                    smestajRepository[i].Bazen = smestaj.Bazen;
                    smestajRepository[i].SpaCentar = smestaj.SpaCentar;
                    smestajRepository[i].OsobeSaInvaliditetom = smestaj.OsobeSaInvaliditetom;
                    smestajRepository[i].Wifi = smestaj.Wifi;

                    SacuvajSmestaj();
                    return smestajRepository[i];
				}
            }

			return new Smestaj();
        }

        public int GetNextId()
        {
            return smestajRepository.Count + 1;
        }


	}
}