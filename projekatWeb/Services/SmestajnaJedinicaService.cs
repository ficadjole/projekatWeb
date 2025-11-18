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
	public class SmestajnaJedinicaService
	{
        private readonly string filePath;
        private List<SmestajnaJedinica> smestajnaJedinicaRepository;
		public SmestajnaJedinicaService(string filePath) {

			this.filePath = filePath;

		}

		public void UcitajSmestajneJedinice()
		{
            if (File.Exists(HostingEnvironment.MapPath(filePath)))
            {

                var jsonPodaci = File.ReadAllText(HostingEnvironment.MapPath(filePath));

                smestajnaJedinicaRepository = JsonConvert.DeserializeObject<List<SmestajnaJedinica>>(jsonPodaci);
                //deserializuje objekte iz jsona

            }
            else
            {
                smestajnaJedinicaRepository = new List<SmestajnaJedinica>();
            }
        }

        public void SacuvajSmestajneJedinice()
        {
            if (File.Exists(HostingEnvironment.MapPath(filePath)))
            {
                var jsonPodaci = JsonConvert.SerializeObject(smestajnaJedinicaRepository, Formatting.Indented); //Formatting.Indented mi omogucava da json bude formatiran
                                                                                             //TypeNameHandling omogucava da mi se lepo ucitaju i Moderator i Turista kao i njhove liste
                File.WriteAllText(HostingEnvironment.MapPath(filePath), jsonPodaci);
            }
        }

        public List<SmestajnaJedinica> GetAll()
        {
            return smestajnaJedinicaRepository;
        }

        public SmestajnaJedinica GetById(int id)
        {
            foreach (var item in smestajnaJedinicaRepository)
            {
                if(item.Id == id)
                {
                    return item;
                }
            }
            return new SmestajnaJedinica();
        }

        public SmestajnaJedinica DodajSmestajnuJedinicu(SmestajnaJedinica smestajnaJedinica)
        {
            SmestajnaJedinica postojeci = GetById(smestajnaJedinica.Id);
            if(postojeci.Id != 0) { return new SmestajnaJedinica(); }

            smestajnaJedinicaRepository.Add(smestajnaJedinica);

            SacuvajSmestajneJedinice(); //novi podaci se odmah ubacuju u json

            return smestajnaJedinica;
        }

        public SmestajnaJedinica LogickiObrisi(int idJedinice)
        {
            SmestajnaJedinica postojeca = GetById(idJedinice);

            if (postojeca.Id == 0) { return new SmestajnaJedinica(); }

            postojeca.LogickoBrisanje = true;

            ArurirajSmestajnuJed(postojeca);

            return postojeca;

        }

        public SmestajnaJedinica setDostupnost(int idJedinice, bool dostupnost)
        {
            SmestajnaJedinica postojeca = GetById(idJedinice);

            if (postojeca.Id == 0)
            {
                return new SmestajnaJedinica();
            }

            postojeca.Dostupnost = dostupnost;
            ArurirajSmestajnuJed(postojeca);
            return postojeca;
        }

        public SmestajnaJedinica ArurirajSmestajnuJed(SmestajnaJedinica smestajnaJedinica)
        {
            SmestajnaJedinica postojeca = GetById(smestajnaJedinica.Id);

            if (postojeca.Id == 0) { return new SmestajnaJedinica(); }

            for (int i = 0; i < smestajnaJedinicaRepository.Count; i++)
            {
                if (smestajnaJedinicaRepository[i].Id == smestajnaJedinica.Id)
                {
                    smestajnaJedinicaRepository[i] = smestajnaJedinica;

                    SacuvajSmestajneJedinice(); //novi podaci se odmah ubacuju u json

                    return smestajnaJedinica;
                }
            }

            return new SmestajnaJedinica();
        }

        public int GetNextId()
        {
            return smestajnaJedinicaRepository.Count + 1;
        }

    }
}