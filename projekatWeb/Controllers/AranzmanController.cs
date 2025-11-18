using projekatWeb.DTOs;
using projekatWeb.Models;
using projekatWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebPages;

namespace projekatWeb.Controllers
{
    public class AranzmanController : Controller
    {
        private AranzmanService aranzmanService;
        private SmestajService smestajService;
        private SmestajnaJedinicaService smestajnaJedinicaService;
        private KomentarService komentarService;
        private KorisnikService korisnikService;
        private RezervacijaService rezervacijaService;
        public AranzmanController()
        {
            aranzmanService = (AranzmanService)System.Web.HttpContext.Current.Application["AranzmaniService"];
            smestajService = (SmestajService)System.Web.HttpContext.Current.Application["SmestajService"];
            smestajnaJedinicaService = (SmestajnaJedinicaService)System.Web.HttpContext.Current.Application["SmestajnaJedinicaService"];
            komentarService = (KomentarService)System.Web.HttpContext.Current.Application["KomentarService"];
            korisnikService = (KorisnikService)System.Web.HttpContext.Current.Application["KorisniciService"];
            rezervacijaService = (RezervacijaService)System.Web.HttpContext.Current.Application["RezervacijaService"];
        }

        public ActionResult Index()
        {

            return View();
        }
        #region Detalji
        public ActionResult Detalji(int id)
        {
            ViewBag.Korisnik = System.Web.HttpContext.Current.Session["LOGGEDIN"];

            Aranzman aran = aranzmanService.GetById(id);
            Smestaj smestaj = smestajService.GetById(aran.IdSmestaj);
            List<SmestajnaJedinica> smestajnaJedinica = new List<SmestajnaJedinica>();

            foreach (var item in smestaj.SmestajneJedinice)
            {
                smestajnaJedinica.Add(smestajnaJedinicaService.GetById(item));
            }

            List<Komentar> komentari = komentarService.SviKomentariSmestaja(smestaj.Id);

            List<Korisnik> korisniciKojiSuKomentarisali = new List<Korisnik>();

            foreach (var item in komentari)
            {
                korisniciKojiSuKomentarisali.Add(korisnikService.GetById(item.IdKorisnika));

            }

            List<Rezervacija> rezervacija = rezervacijaService.GetByIdAranzmana(aran.Id);

            AranzmanDetailsDto aranzmanDetailsDto = new AranzmanDetailsDto(aran, smestaj, smestajnaJedinica, komentari, korisniciKojiSuKomentarisali,rezervacija);

            return View(aranzmanDetailsDto);

        }

        [HttpPost]
        public ActionResult DodajKomentar(int idAranzmana,int idKorisnika,int idSmestaja,string tekst, int ocena)
        {
            int id = komentarService.GetNextId();

            Komentar noviKomentar = new Komentar(id, idKorisnika, idSmestaja, tekst, ocena, false);

           var komentar = komentarService.DodajKomentar(noviKomentar);

            if(komentar.Id == 0)
            {
                ViewBag.Poruka = "Doslo je do greske prilikom dodavanja komentara!";
            }
            else
            {
                ViewBag.Poruka = "Uspesno ste dodali komentar! Ceka se odobrenje od strane moderatora.";
            }

            Aranzman aran = aranzmanService.GetById(idAranzmana);
            Smestaj smestaj = smestajService.GetById(idSmestaja);
            List<SmestajnaJedinica> smestajnaJedinica = new List<SmestajnaJedinica>();

            foreach (var item in smestaj.SmestajneJedinice)
            {
                smestajnaJedinica.Add(smestajnaJedinicaService.GetById(item));
            }

            List<Komentar> komentari = komentarService.SviKomentariSmestaja(smestaj.Id);

            List<Korisnik> korisniciKojiSuKomentarisali = new List<Korisnik>();

            foreach (var item in komentari)
            {
                korisniciKojiSuKomentarisali.Add(korisnikService.GetById(item.IdKorisnika));

            }

            List<Rezervacija> rezervacija = rezervacijaService.GetByIdAranzmana(aran.Id);

            AranzmanDetailsDto aranzmanDetailsDto = new AranzmanDetailsDto(aran, smestaj, smestajnaJedinica, komentari, korisniciKojiSuKomentarisali, rezervacija);

            return View("Detalji",aranzmanDetailsDto);
        }

        [HttpPost]
        public ActionResult OdobriKomentar(int idKomentara, int idAranzmana,string status)
        {

            var komentar = komentarService.GetById(idKomentara);

            Komentar azuriranKomentar = new Komentar();

            if (status.Equals("Odobri"))
            {
                azuriranKomentar = komentarService.PromeniStatus(komentar.Id,true);
            }
            else
            {
                azuriranKomentar = komentarService.PromeniStatus(komentar.Id, false);
            }
            
            Aranzman aran = aranzmanService.GetById(idAranzmana);
            Smestaj smestaj = smestajService.GetById(aran.IdSmestaj);
            List<SmestajnaJedinica> smestajnaJedinica = new List<SmestajnaJedinica>();
            foreach (var item in smestaj.SmestajneJedinice)
            {
                smestajnaJedinica.Add(smestajnaJedinicaService.GetById(item));
            }
            List<Komentar> komentari = komentarService.SviKomentariSmestaja(smestaj.Id);
            List<Korisnik> korisniciKojiSuKomentarisali = new List<Korisnik>();
            foreach (var item in komentari)
            {
                korisniciKojiSuKomentarisali.Add(korisnikService.GetById(item.IdKorisnika));
            }

            List<Rezervacija> rezervacija = rezervacijaService.GetByIdAranzmana(aran.Id);

            AranzmanDetailsDto aranzmanDetailsDto = new AranzmanDetailsDto(aran, smestaj, smestajnaJedinica, komentari, korisniciKojiSuKomentarisali, rezervacija);
            return View("Detalji", aranzmanDetailsDto);

        }


        #region Pretraga jedinice
        [HttpPost]

        public ActionResult PretragaJedinice(int idAranzmana, int idSmestaja, int? minGostiju, int? maxGostiju, int? cena, bool? petFriendly, string akcija)
        {
            ViewBag.Korisnik = System.Web.HttpContext.Current.Session["LOGGEDIN"]; //ovo mi treba zbog stranice detalji

            Smestaj smestaj = smestajService.GetById(idSmestaja);

            List<SmestajnaJedinica> smestajnaJedinica = new List<SmestajnaJedinica>();

            foreach (var item in smestaj.SmestajneJedinice)
            {
                smestajnaJedinica.Add(smestajnaJedinicaService.GetById(item));
            }

            if(akcija.Equals("pretrazi"))
            {
                if (minGostiju != null)
                {
                    smestajnaJedinica = pretragaPoMinBrGostiju(smestajnaJedinica, (int)minGostiju);
                }

                if(maxGostiju != null)
                {
                    smestajnaJedinica = pretragaPoMaxBrGostiju(smestajnaJedinica, (int)maxGostiju);
                }

                if (cena != null)
                {
                    smestajnaJedinica = pretragaPoCeni(smestajnaJedinica, (int)cena);
                }
                if (petFriendly != null)
                {
                    smestajnaJedinica = pretragaPoPetFriendly(smestajnaJedinica, (bool)petFriendly);
                }
            }

            Aranzman aranzman = aranzmanService.GetById(idAranzmana);
            List<Komentar> komentari = komentarService.SviKomentariSmestaja(smestaj.Id);

            List<Korisnik> korisniciKojiSuKomentarisali = new List<Korisnik>();

            foreach (var item in komentari)
            {
                korisniciKojiSuKomentarisali.Add(korisnikService.GetById(item.IdKorisnika));

            }

            List<Rezervacija> rezervacija = rezervacijaService.GetByIdAranzmana(aranzman.Id);

            AranzmanDetailsDto aranzmanDetailsDto = new AranzmanDetailsDto(aranzman, smestaj, smestajnaJedinica, komentari, korisniciKojiSuKomentarisali, rezervacija);

            return View("Detalji",aranzmanDetailsDto);
        }

        private List<SmestajnaJedinica> pretragaPoMinBrGostiju(List<SmestajnaJedinica> jedinice,int minGostiju)
        {
            List<SmestajnaJedinica> filtriraneJedinice = new List<SmestajnaJedinica>();
            foreach (var item in jedinice)
            {
                if (item.MaksBrOsoba >= minGostiju)
                {
                    filtriraneJedinice.Add(item);
                }
            }
            return filtriraneJedinice;
        }

        private List<SmestajnaJedinica> pretragaPoMaxBrGostiju(List<SmestajnaJedinica> jedinice, int maxGostiju)
        {
            List<SmestajnaJedinica> filtriraneJedinice = new List<SmestajnaJedinica>();
            foreach (var item in jedinice)
            {
                if (item.MaksBrOsoba <= maxGostiju)
                {
                    filtriraneJedinice.Add(item);
                }
            }
            return filtriraneJedinice;
        }

        private List<SmestajnaJedinica> pretragaPoCeni(List<SmestajnaJedinica> jedinice,int cena)
        {
            List<SmestajnaJedinica> filtriraneJedinice = new List<SmestajnaJedinica>();
            foreach (var item in jedinice)
            {
                if (item.CenaJedinice <= cena) // ako je manja ili jednaka od cene koju je korisnik uneo
                {
                    filtriraneJedinice.Add(item);
                }
            }
            return filtriraneJedinice;
        }

        private List<SmestajnaJedinica> pretragaPoPetFriendly(List<SmestajnaJedinica> jedinice, bool petFriendly)
        {
            List<SmestajnaJedinica> filtriraneJedinice = new List<SmestajnaJedinica>();
            foreach (var item in jedinice)
            {
                if(item.PetFriendly == petFriendly)
                {
                    filtriraneJedinice.Add(item);
                }
            }
            return filtriraneJedinice;
        }
        #endregion

        #region SortiranjeJedinice

        public ActionResult SortirajSmestaj(string kriterijum , string vrsta, int idAranzmana, int idSmestaja)
        {
            var aranzman = aranzmanService.GetById(idAranzmana);

            var smestaj = smestajService.GetById(idSmestaja);

            List<SmestajnaJedinica> smestajneJedinice = new List<SmestajnaJedinica>();

            foreach (var item in smestaj.SmestajneJedinice)
            {
                smestajneJedinice.Add(smestajnaJedinicaService.GetById(item));
            }

            if (kriterijum.Equals("cena"))
            {
                smestajneJedinice = SortirajPoCeni(smestajneJedinice, vrsta);
            }
            else if (kriterijum.Equals("brGostiju"))
            {
                smestajneJedinice = SortirajPoMaksBrGostiju(smestajneJedinice, vrsta);
            }

            List<Komentar> komentari = komentarService.SviKomentariSmestaja(smestaj.Id);

            List<Korisnik> korisniciKojiSuKomentarisali = new List<Korisnik>();

            foreach (var item in komentari)
            {
                korisniciKojiSuKomentarisali.Add(korisnikService.GetById(item.IdKorisnika));
            }

            List<Rezervacija> rezervacija = rezervacijaService.GetByIdAranzmana(aranzman.Id);

            AranzmanDetailsDto aranzmanDetailsDto = new AranzmanDetailsDto(aranzman, smestaj, smestajneJedinice, komentari, korisniciKojiSuKomentarisali, rezervacija);

            return View("Detalji",aranzmanDetailsDto);
        }

        private List<SmestajnaJedinica> SortirajPoCeni(List<SmestajnaJedinica> jedinice, string vrsta)
        {
            if (vrsta.Equals("asc"))
            {
                return jedinice.OrderBy(j => j.CenaJedinice).ToList();
            }
            else
            {
                return jedinice.OrderByDescending(j => j.CenaJedinice).ToList();
            }
        }

        private List<SmestajnaJedinica> SortirajPoMaksBrGostiju(List<SmestajnaJedinica> jedinice, string vrsta)
        {
            if (vrsta.Equals("asc"))
            {
                return jedinice.OrderBy(j => j.MaksBrOsoba).ToList();
            }
            else
            {
                return jedinice.OrderByDescending(j => j.MaksBrOsoba).ToList();
            }
        }

        #endregion

        #endregion
        #region Azuriranje aranzmana
        public ActionResult Uredi(int id)
        {
            Aranzman aran = aranzmanService.GetById(id);
            Smestaj smestaj = smestajService.GetById(aran.IdSmestaj);
            List<SmestajnaJedinica> smestajnaJedinica = new List<SmestajnaJedinica>();

            foreach (var item in smestaj.SmestajneJedinice)
            {
                smestajnaJedinica.Add(smestajnaJedinicaService.GetById(item));
            }

            AzuriranjeAranzmanaDto aranzmanDetailsDto = new AzuriranjeAranzmanaDto(aran, smestaj, smestajnaJedinica);

            return View(aranzmanDetailsDto);
        }
        
        [HttpPost]
        public ActionResult AzurirajAranzman(Aranzman azuriraniAranzman,string noviPoster) {

            //ovo moram ovako jer je moj format dd/MM/yyyy a on mi vraca mm-dd-yyyy

            DateTime datumPolaska = azuriraniAranzman.DatumPolaska.AsDateTime();

            azuriraniAranzman.DatumPolaska = datumPolaska.ToString("dd/MM/yyyy");

            DateTime datumPovratka = azuriraniAranzman.DatumPovratka.AsDateTime();

            azuriraniAranzman.DatumPovratka = datumPovratka.ToString("dd/MM/yyyy");

            var provera = aranzmanService.GetById(azuriraniAranzman.Id);

            if (azuriraniAranzman.NazivAranzmana.IsEmpty() || azuriraniAranzman.DatumPolaska.IsEmpty() || azuriraniAranzman.DatumPovratka.IsEmpty() || azuriraniAranzman.Destinacija.IsEmpty() || azuriraniAranzman.OpisAranzmana.IsEmpty() || azuriraniAranzman.ProgramAranzmana.IsEmpty())
            {
                ViewBag.Poruka = "Sva polja su obavezna!";
                Aranzman aran = aranzmanService.GetById(azuriraniAranzman.Id);
                Smestaj smestajj = smestajService.GetById(azuriraniAranzman.IdSmestaj);
                List<SmestajnaJedinica> smestajnaJedinice = new List<SmestajnaJedinica>();
                foreach (var item in smestajj.SmestajneJedinice)
                {
                    smestajnaJedinice.Add(smestajnaJedinicaService.GetById(item));
                }
                AzuriranjeAranzmanaDto aranzmanDetailsDto2 = new AzuriranjeAranzmanaDto(azuriraniAranzman, smestajj, smestajnaJedinice);
                return View("Uredi", aranzmanDetailsDto2);
            }else if (Proveri(provera,azuriraniAranzman) && noviPoster.IsEmpty())
            {
                ViewBag.Poruka = "Niste nista promenili!";
                Aranzman aran = aranzmanService.GetById(azuriraniAranzman.Id);
                Smestaj smestajj = smestajService.GetById(azuriraniAranzman.IdSmestaj);
                List<SmestajnaJedinica> smestajnaJedinice = new List<SmestajnaJedinica>();
                foreach (var item in smestajj.SmestajneJedinice)
                {
                    smestajnaJedinice.Add(smestajnaJedinicaService.GetById(item));
                }
                AzuriranjeAranzmanaDto aranzmanDetailsDto2 = new AzuriranjeAranzmanaDto(azuriraniAranzman, smestajj, smestajnaJedinice);
                return View("Uredi", aranzmanDetailsDto2);
            }

            if (!noviPoster.IsEmpty() &&!azuriraniAranzman.PosterAranzmanaUrl.Equals(noviPoster))
            {
                //slike ce da rade samo ako su iz istog foldera u kome su bile i ranije
                azuriraniAranzman.PosterAranzmanaUrl = "~/Content/Images/Aranzmani/" + noviPoster;
            }

            var aranzman = aranzmanService.AzurirajAranzman(azuriraniAranzman);

            if(aranzman.Id == 0)
            {
                ViewBag.Poruka = "Doslo je do greske prilikom azuriranja aranzmana!";
            }
            else
            {
                ViewBag.Poruka = "Uspesno ste azurirali aranzman!";
            }
            Smestaj smestaj = smestajService.GetById(aranzman.IdSmestaj);
            List<SmestajnaJedinica> smestajnaJedinica = new List<SmestajnaJedinica>();

            foreach (var item in smestaj.SmestajneJedinice)
            {
                smestajnaJedinica.Add(smestajnaJedinicaService.GetById(item));
            }

            AzuriranjeAranzmanaDto aranzmanDetailsDto = new AzuriranjeAranzmanaDto(aranzman, smestaj, smestajnaJedinica);

            return View("Uredi", aranzmanDetailsDto);
        }

        [HttpPost]
        public ActionResult AzurirajSmestaj(Smestaj azuriraniSmestaj, int IdAranzmana)
        {
            var postoji = smestajService.GetById(azuriraniSmestaj.Id);

            if (ProveriSmestaj(postoji, azuriraniSmestaj)) {
            
                ViewBag.PorukaSmestaj = "Niste nista promenili!";

                var aranzmanok = aranzmanService.GetById(IdAranzmana);
                List<SmestajnaJedinica> smestajnaJedinicaok = new List<SmestajnaJedinica>();

                foreach (var item in postoji.SmestajneJedinice)
                {
                    smestajnaJedinicaok.Add(smestajnaJedinicaService.GetById(item));
                }

                AzuriranjeAranzmanaDto aranzmanDetailsDtook = new AzuriranjeAranzmanaDto(aranzmanok, postoji, smestajnaJedinicaok);

                return View("Uredi", aranzmanDetailsDtook);

            }
            else if(azuriraniSmestaj.NazivSmestaja.IsEmpty())
            {
                ViewBag.PorukaSmestaj = "Naziv smestaja ne sme biti prazan!";

                var aranzmanok = aranzmanService.GetById(IdAranzmana);
                List<SmestajnaJedinica> smestajnaJedinicaok = new List<SmestajnaJedinica>();

                foreach (var item in postoji.SmestajneJedinice)
                {
                    smestajnaJedinicaok.Add(smestajnaJedinicaService.GetById(item));
                }

                AzuriranjeAranzmanaDto aranzmanDetailsDtook = new AzuriranjeAranzmanaDto(aranzmanok, postoji, smestajnaJedinicaok);

                return View("Uredi", aranzmanDetailsDtook);
            }

            var smestaj = smestajService.AzurirajSmestaj(azuriraniSmestaj);

            if(smestaj.Id == 0)
            {
                ViewBag.PorukaSmestaj = "Doslo je do greske prilikom azuriranja smestaja!";
            }
            else
            {
                ViewBag.PorukaSmestaj = "Uspesno ste azurirali smestaj!";
            }

            var aranzman = aranzmanService.GetById(IdAranzmana);
            List<SmestajnaJedinica> smestajnaJedinica = new List<SmestajnaJedinica>();

            foreach (var item in smestaj.SmestajneJedinice)
            {
                smestajnaJedinica.Add(smestajnaJedinicaService.GetById(item));
            }

            AzuriranjeAranzmanaDto aranzmanDetailsDto = new AzuriranjeAranzmanaDto(aranzman, smestaj, smestajnaJedinica);

            return View("Uredi", aranzmanDetailsDto);
        }

        [HttpPost]
        public ActionResult AzurirajSmestajnuJedinicu(SmestajnaJedinica azuriranaJedinica,int IdAranzmana,int idSmestaja)
        {
            var postoji = smestajnaJedinicaService.GetById(azuriranaJedinica.Id);

            var rezervacijeJedinice = rezervacijaService.GetByIdSmestajneJedinice(azuriranaJedinica.Id);

            if (ProveriJedinicu(postoji, azuriranaJedinica))
            {
                ViewBag.PorukaJedinica = "Niste promenili jedinicu!";
                ViewBag.IdJedinice = postoji.Id;

                var aranzman = aranzmanService.GetById(IdAranzmana);

                var smestaj = smestajService.GetById(idSmestaja);

                List<SmestajnaJedinica> smestajnaJedinica = new List<SmestajnaJedinica>();

                foreach (var item in smestaj.SmestajneJedinice)
                {
                    smestajnaJedinica.Add(smestajnaJedinicaService.GetById(item));
                }

                AzuriranjeAranzmanaDto aranzmanDetailsDto = new AzuriranjeAranzmanaDto(aranzman, smestaj, smestajnaJedinica);

                return View("Uredi", aranzmanDetailsDto);
            }else if(rezervacijeJedinice.Count > 0)
            {
                ViewBag.PorukaJedinica = "Ne mozete menjati jedinicu koja ima aktivne rezervacije!";
                ViewBag.IdJedinice = postoji.Id;
                var aranzman = aranzmanService.GetById(IdAranzmana);
                var smestaj = smestajService.GetById(idSmestaja);
                List<SmestajnaJedinica> smestajnaJedinica = new List<SmestajnaJedinica>();
                foreach (var item in smestaj.SmestajneJedinice)
                {
                    smestajnaJedinica.Add(smestajnaJedinicaService.GetById(item));
                }
                AzuriranjeAranzmanaDto aranzmanDetailsDto = new AzuriranjeAranzmanaDto(aranzman, smestaj, smestajnaJedinica);
                return View("Uredi", aranzmanDetailsDto);
            }

                var smestajnaJed = smestajnaJedinicaService.ArurirajSmestajnuJed(azuriranaJedinica);

            if(smestajnaJed.Id == 0)
            {
                ViewBag.PorukaJedinica = "Doslo je do greske prilikom azuriranja jedinice";
                ViewBag.IdJedinice = azuriranaJedinica.Id;
            }
            else
            {
                ViewBag.PorukaJedinica = "Uspesno ste azurirali smestajnu jedinicu!";
                ViewBag.IdJedinice = azuriranaJedinica.Id;
            }

            var Aranzman = aranzmanService.GetById(IdAranzmana);

            var Smestaj = smestajService.GetById(idSmestaja);

            List<SmestajnaJedinica> SmestajnaJedinica = new List<SmestajnaJedinica>();

            foreach (var item in Smestaj.SmestajneJedinice)
            {
                SmestajnaJedinica.Add(smestajnaJedinicaService.GetById(item));
            }

            AzuriranjeAranzmanaDto AranzmanDetailsDto = new AzuriranjeAranzmanaDto(Aranzman, Smestaj, SmestajnaJedinica);

            return View("Uredi", AranzmanDetailsDto);
        }
        private bool ProveriJedinicu(SmestajnaJedinica provera,SmestajnaJedinica azuriranaJedinica)
        {
            return provera.Id == azuriranaJedinica.Id 
                && provera.LogickoBrisanje == azuriranaJedinica.LogickoBrisanje 
                && provera.CenaJedinice == azuriranaJedinica.CenaJedinice
                && provera.MaksBrOsoba == azuriranaJedinica.MaksBrOsoba
                && provera.PetFriendly == azuriranaJedinica.PetFriendly;
        }

        private bool ProveriSmestaj(Smestaj provera,Smestaj azuriraniSmestaj)
        {
            return provera.Id == azuriraniSmestaj.Id &&
                    provera.TipSmestaja == azuriraniSmestaj.TipSmestaja &&
                    provera.NazivSmestaja == azuriraniSmestaj.NazivSmestaja &&
                    provera.BrojZvezdica == azuriraniSmestaj.BrojZvezdica &&
                    provera.Bazen == azuriraniSmestaj.Bazen &&
                    provera.SpaCentar == azuriraniSmestaj.SpaCentar &&
                    provera.OsobeSaInvaliditetom == azuriraniSmestaj.OsobeSaInvaliditetom &&
                    provera.Wifi == azuriraniSmestaj.Wifi &&
                    provera.LogickoBrisanje == azuriraniSmestaj.LogickoBrisanje;
        }

        private bool Proveri(Aranzman provera, Aranzman azuriraniAranzman)
        {
            return provera.NazivAranzmana == azuriraniAranzman.NazivAranzmana &&
                    provera.TipAranzmana == azuriraniAranzman.TipAranzmana &&
                    provera.TipPrevoza == azuriraniAranzman.TipPrevoza &&
                    provera.Destinacija == azuriraniAranzman.Destinacija &&
                    provera.DatumPolaska == azuriraniAranzman.DatumPolaska &&
                    provera.DatumPovratka == azuriraniAranzman.DatumPovratka &&
                    provera.MaksBrPutnika == azuriraniAranzman.MaksBrPutnika &&
                    provera.OpisAranzmana == azuriraniAranzman.OpisAranzmana &&
                    provera.ProgramAranzmana == azuriraniAranzman.ProgramAranzmana &&
                    provera.PosterAranzmanaUrl == azuriraniAranzman.PosterAranzmanaUrl &&
                    provera.IdSmestaj == azuriraniAranzman.IdSmestaj &&
                    provera.LogickoBrisanje == azuriraniAranzman.LogickoBrisanje;
        }

        #endregion

        #region Brisanje aranzmana

        public ActionResult Obrisi(int id)
        {
            var smestaji = aranzmanService.GetAllAranzman();

            var korsinik = Session["LOGGEDIN"];

            var postoji = aranzmanService.GetById(id);

            if (postoji.Id == 0)
            {
                ViewBag.Poruka = "Doslo je do greske prilikom brisanja aranzmana";

                ViewBag.Korisnik = korsinik;

                return View("~/Views/Home/Index.cshtml", smestaji);
            }

            List<Rezervacija> rezervacije = rezervacijaService.GetByIdAranzmana(id);

            if (rezervacije.Count > 0)
            {
                ViewBag.Poruka = "Nije moguce obrisati ovaj aranzman jer ga je neko rezervisao";
                ViewBag.Korisnik = korsinik;

                return View("~/Views/Home/Index.cshtml", smestaji);
            }

            var obrisan = aranzmanService.LogickiObrisiAranzman(postoji);
            var smestaj = smestajService.LogickiObrisi(obrisan.IdSmestaj);

            List<SmestajnaJedinica> jediniceZaBrisanje = new List<SmestajnaJedinica>();
            foreach (var item in smestaj.SmestajneJedinice)
            {
                jediniceZaBrisanje.Add(smestajnaJedinicaService.GetById(item));
            }

            foreach (var item in jediniceZaBrisanje)
            {
                smestajnaJedinicaService.LogickiObrisi(item.Id);
            }

            if (!obrisan.LogickoBrisanje && !smestaj.LogickoBrisanje)
            {
                ViewBag.Poruka = "Doslo je do greske prilikom brisanja aranzmana";

                ViewBag.Korisnik = korsinik;

                return View("~/Views/Home/Index.cshtml", smestaji);
            }
            ;

            ViewBag.Poruka = "Uspesno ste obrisali aranzman!";
            ViewBag.Korisnik = korsinik;

            return View("~/Views/Home/Index.cshtml", smestaji);

        }

        [HttpPost]
        public ActionResult ObrisiSmestaj(int id, int idAranzmana)
        {
            var postoji = smestajService.GetById(id);

            if (postoji.Id == 0)
            {
                ViewBag.PorukaSmestaj = "Doslo je do greske prilikom brisanja smestaja";
                AzuriranjeAranzmanaDto az = keirajDto(idAranzmana);
                return View("Uredi",az);
            }

            var aranzman = aranzmanService.GetById(idAranzmana);

            if(aranzman.DatumPolaska.AsDateTime() >= DateTime.Now)
            {
                ViewBag.Poruka = "Nije moguce obrisati smestaj jer je u sklopu aranzmana koji nije izvrsen!";
                AzuriranjeAranzmanaDto az = keirajDto(idAranzmana);
                return View("Uredi", az);
            }

            foreach (var jedinicaId in postoji.SmestajneJedinice)
            {
                var rezervacijeJedinice = rezervacijaService.GetByIdSmestajneJedinice(jedinicaId);

                foreach (var rez in rezervacijeJedinice)
                {
                    if (rez.Status)
                    {
                        ViewBag.PorukaSmestaj = "Nije moguće obrisati smeštaj jer postoje aktivne rezervacije!";
                        AzuriranjeAranzmanaDto az = keirajDto(idAranzmana);
                        return View("Uredi", az);
                    }
                }
            }

            var obrisan = smestajService.LogickiObrisi(id);//brisemo smestaj logicki

            if (!obrisan.LogickoBrisanje)
            {
                ViewBag.PorukaSmestaj = "Doslo je do greske prilikom brisanja smestaja";
                AzuriranjeAranzmanaDto az = keirajDto(idAranzmana);
                return View("Uredi", az);
            }

            //jer je obrisan smestaj moramo da obrisemo i sve njegove jedinice
            List<SmestajnaJedinica> jediniceZaBrisanje = new List<SmestajnaJedinica>();
            foreach (var item in postoji.SmestajneJedinice)
            {
                jediniceZaBrisanje.Add(smestajnaJedinicaService.GetById(item));
            }

            //obrisali smo i smestajne jedinice
            foreach (var item in jediniceZaBrisanje)
            {
                smestajnaJedinicaService.LogickiObrisi(item.Id);
            }

            //moram onda i da obrisem i id smestaja iz aranzmana

            aranzman.IdSmestaj = 0;
            var azuriranAranzman = aranzmanService.AzurirajAranzman(aranzman);

            ViewBag.PorukaSmestaj = "Uspesno ste obrisali smestaj!";

            AzuriranjeAranzmanaDto azuziranjeDto = keirajDto(idAranzmana);
            return View("Uredi", azuziranjeDto);
        }

        [HttpPost]
        
        public ActionResult ObrisiSmestajnuJedinicu(int id,int idSmestaja ,int idAranzmana)
        {

            var rezervacijeJedinice = rezervacijaService.GetByIdSmestajneJedinice(id);

            foreach (var rez in rezervacijeJedinice)
            {
                if (rez.Status)
                {
                    ViewBag.PorukaJedinica = "Nije moguće obrisati jedinicu jer postoje aktivne rezervacije!";
                    AzuriranjeAranzmanaDto az = keirajDto(idAranzmana);
                    return View("Uredi", az);
                }
            }

            var obrisana = smestajnaJedinicaService.LogickiObrisi(id);

            if (!obrisana.LogickoBrisanje)
            {
                ViewBag.PorukaJedinica = "Doslo je do greske prilikom brisanja smestajne jedinice";
                AzuriranjeAranzmanaDto az = keirajDto(idAranzmana);
                return View("Uredi", az);
            }

            ViewBag.PorukaJedinica = "Uspesno ste obrisali smestajnu jedinicu!";

            AzuriranjeAranzmanaDto azuriranjeAranzmana = keirajDto(idAranzmana);
            return View("Uredi", azuriranjeAranzmana);
        }

        private AzuriranjeAranzmanaDto keirajDto(int id)
        {
            Aranzman aran = aranzmanService.GetById(id);
            Smestaj smestaj = smestajService.GetById(aran.IdSmestaj);
            List<SmestajnaJedinica> smestajnaJedinica = new List<SmestajnaJedinica>();

            foreach (var item in smestaj.SmestajneJedinice)
            {
                smestajnaJedinica.Add(smestajnaJedinicaService.GetById(item));
            }

            AzuriranjeAranzmanaDto aranzmanDetailsDto = new AzuriranjeAranzmanaDto(aran, smestaj, smestajnaJedinica);

            return aranzmanDetailsDto;
        }

        #endregion

        #region Kreiranje aranzmana

        public ActionResult DodajAranzman()
        {
            return View();
        }

        public ActionResult KreirajAranzman(string NazivAranzmana,DateTime DatumPolaska, DateTime DatumPovratka,string Destinacija, string OpisAranzmana, string ProgramAranzmana, int MaksBrPutnika, string noviPoster, TipAranzmana TipAranzmana, TipPrevoza TipPrevoza  )
        {
            var aranzmani = aranzmanService.GetByNaziv(NazivAranzmana);

            if(aranzmani.Id != 0)
            {
                ViewBag.Poruka = "Postoji vec aranzman sa istim imenom molim Vas promenite ime";
                return View("DodajAranzman");

            }

            int id = aranzmanService.GetNextId();

            Aranzman noviAranzman = new Aranzman(id, NazivAranzmana, TipAranzmana, TipPrevoza, Destinacija, DatumPolaska.ToString("dd/MM/yyyy"), DatumPovratka.ToString("dd/MM/yyyy"), MaksBrPutnika, OpisAranzmana, ProgramAranzmana, "~/Content/Images/Aranzmani/" + noviPoster,0);

            var aranzman = aranzmanService.DodajAranzman(noviAranzman);

            if (aranzman.Id == 0)
            {
                ViewBag.Poruka = "Doslo je do greske prilikom dodavanja aranzmana!";
                return View("DodajAranzman");
            }

            var menadzer = Session["LOGGEDIN"] as Menadzer;

            menadzer.KreiraniAranzmani.Add(aranzman.Id);

            var azuriranMenadzer = korisnikService.AzurirajKorisnika(menadzer);


            ViewBag.Poruka = "Uspesno ste dodali aranzman! Sada dodajte smestaj.";

            Smestaj smestaj = smestajService.GetById(aranzman.IdSmestaj);
            List<SmestajnaJedinica> smestajnaJedinica = new List<SmestajnaJedinica>();

            foreach (var item in smestaj.SmestajneJedinice)
            {
                smestajnaJedinica.Add(smestajnaJedinicaService.GetById(item));
            }

            AzuriranjeAranzmanaDto aranzmanDetailsDto = new AzuriranjeAranzmanaDto(aranzman, smestaj, smestajnaJedinica);

            return View("Uredi",aranzmanDetailsDto);
        }

        #endregion

        #region Kreiraj smestaj
        public ActionResult DodajSmestaj(int id)
        {
            return View(id);
        }

        [HttpPost]
        public ActionResult KreirajSmestaj(int IdAranzmana,string NazivSmestaja,TipSmestaja TipSmestaja,int BrojZvezdica, bool OsobeSaInvaliditetom,bool SpaCentar, bool Bazen,bool Wifi)
        {
            int idSmestaja = smestajService.GetNextId();

            Smestaj noviSmestaj = new Smestaj(idSmestaja, TipSmestaja, NazivSmestaja, BrojZvezdica, Bazen,  SpaCentar, OsobeSaInvaliditetom, Wifi, new List<int>());

            var smestaj = smestajService.DodajSmestaj(noviSmestaj);

            if (smestaj.Id == 0)
            {
                ViewBag.Poruka = "Doslo je do greske prilikom dodavanja smestaja!";
                return View("DodajSmestaj", IdAranzmana);
            }

            var aranzman = aranzmanService.GetById(IdAranzmana);

            aranzman.IdSmestaj = smestaj.Id;

            var azuriranAranzman = aranzmanService.AzurirajAranzman(aranzman);


            AzuriranjeAranzmanaDto azuziranjeDto = keirajDto(IdAranzmana);

            ViewBag.PorukaSmestaj = "Uspesno ste dodali smestaj!";

            return View("Uredi", azuziranjeDto);
        }

        #endregion

        #region Kreiraj smestajnu jedinicu

        public ActionResult DodajJedinicu(int id, int idAranzmana)
        {

            DodajJedinicuDto dodajJedinicuDto = new DodajJedinicuDto(id, idAranzmana);

            return View(dodajJedinicuDto);
        }

        [HttpPost]

        public ActionResult KreirajJedinicu(int idSmestaja, int idAranzmana,int CenaJedinice, int MaksBrOsoba, bool PetFriendly)
        {
            int idJedinice = smestajnaJedinicaService.GetNextId();

            SmestajnaJedinica novaJedinica = new SmestajnaJedinica(idJedinice, MaksBrOsoba, PetFriendly, CenaJedinice, true);

            var jedinica = smestajnaJedinicaService.DodajSmestajnuJedinicu(novaJedinica);

            if (jedinica.Id == 0)
            {
                ViewBag.Poruka = "Doslo je do greske prilikom dodavanja smestajne jedinice!";
                DodajJedinicuDto dodajJedinicuDto = new DodajJedinicuDto(idSmestaja, idAranzmana);
                return View("DodajJedinicu", dodajJedinicuDto);
            }

            var smestaj = smestajService.GetById(idSmestaja);

            smestaj.SmestajneJedinice.Add(jedinica.Id);

            var azuriranSmestaj = smestajService.AzurirajSmestaj(smestaj);

            AzuriranjeAranzmanaDto azuziranjeDto = keirajDto(idAranzmana);
            ViewBag.PorukaJedinica = "Uspesno ste dodali smestajnu jedinicu!";
            return View("Uredi", azuziranjeDto);


        }
        #endregion
    }
}