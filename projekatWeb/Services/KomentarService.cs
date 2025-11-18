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
	public class KomentarService
	{
		private List<Komentar> komentarRepository;
		private readonly string filePath;

		public KomentarService(string filePath)
		{
			this.filePath = filePath;
		}

        public void UcitajKomentar()
        {
            if (File.Exists(HostingEnvironment.MapPath(filePath)))
            {

                var jsonPodaci = File.ReadAllText(HostingEnvironment.MapPath(filePath));

                komentarRepository = JsonConvert.DeserializeObject<List<Komentar>>(jsonPodaci);
                //deserializuje objekte iz jsona

            }
            else
            {
                komentarRepository = new List<Komentar>();
            }
        }

        public void SacuvajKomenar()
        {
            if (File.Exists(HostingEnvironment.MapPath(filePath)))
            {
                var jsonPodaci = JsonConvert.SerializeObject(komentarRepository, Formatting.Indented); //Formatting.Indented mi omogucava da json bude formatiran
                File.WriteAllText(HostingEnvironment.MapPath(filePath), jsonPodaci);
            }
        }

        public List<Komentar> GetAll()
        {
            return komentarRepository;
        }

        public Komentar GetById(int id)
        {
            foreach (var item in komentarRepository)
            {
                if(item.Id  == id)
                {
                    return item;
                }

            }

            return new Komentar();
        }

        public Komentar DodajKomentar(Komentar komentar)
        {
            Komentar postojeci = GetById(komentar.Id);

            if (postojeci.Id != 0)
            {
                return new Komentar();
            }

            komentarRepository.Add(komentar);
            SacuvajKomenar();
            return komentar;
        }

        public int GetNextId()
        {
            return komentarRepository.Count + 1;
        }
        public List<Komentar> SviKomentariSmestaja(int idSmestaja)
        {
            List<Komentar> komentari = new List<Komentar>();

            foreach (var item in komentarRepository)
            {
                if(item.IdSmestaja == idSmestaja)
                {
                    komentari.Add(item);
                }
            }

            return komentari;
        }

        public List<Komentar> SviKomentariKorisnika(int idKorisnika)
        {
            List<Komentar> komentari = new List<Komentar>();

            foreach (var item in komentarRepository)
            {
                if (item.IdKorisnika == idKorisnika)
                {
                    komentari.Add(item);
                }
            }

            return komentari;
        }

        public Komentar PromeniStatus(int idKomentara, bool odobren)
        {
            Komentar komentar = GetById(idKomentara);
            if (komentar.Id == 0)
            {
                return new Komentar();
            }

            for(int i = 0; i < komentarRepository.Count; i++)
            {
                if (komentarRepository[i].Id == idKomentara)
                {
                    komentarRepository[i].Odobren = odobren;
                    SacuvajKomenar();
                    return komentarRepository[i];
                }
            }

            return new Komentar();
        }
    }
}