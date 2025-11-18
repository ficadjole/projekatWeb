
using projekatWeb.Models;
using projekatWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace projekatWeb.Controllers
{
    public class HomeController : Controller
    {

        AranzmanService aranzmanService;
        SmestajService smestajService;
        SmestajnaJedinicaService smestajnaJedinicaService;
        RezervacijaService rezervacijaService;
        public HomeController()
        {
            aranzmanService = (AranzmanService)System.Web.HttpContext.Current.Application["AranzmaniService"];
            smestajService = (SmestajService)System.Web.HttpContext.Current.Application["SmestajService"];
            smestajnaJedinicaService = (SmestajnaJedinicaService)System.Web.HttpContext.Current.Application["SmestajnaJedinicaService"];
            rezervacijaService = (RezervacijaService)System.Web.HttpContext.Current.Application["RezervacijaService"];
        }

        public ActionResult Index()
        {
            var smestaji = aranzmanService.GetAllAranzman();

            var korsinik = Session["LOGGEDIN"];

            ViewBag.Korisnik = korsinik;

            return View(smestaji);
        }

        public ActionResult Prikazi() {

            AranzmanService aranzmanService = (AranzmanService)System.Web.HttpContext.Current.Application["AranzmaniService"];

            Aranzman aranzman = aranzmanService.GetByNaziv("Letovanje Grčka");
            ViewBag.Aranzman = aranzman;
            var smestaji = aranzmanService.GetAllAranzman();
            return View("Index",smestaji);
        }

        public ActionResult SortirajDatum(string datum, string vrsta)
        {
            var smestaji = aranzmanService.GetAllAranzman();
            List<Aranzman> soritrani = new List<Aranzman>();
            switch (datum)
            {
                case "DatumPolaska":

                    if(vrsta == "asc") {
                        soritrani = smestaji.OrderBy(a => DateTime.ParseExact(a.DatumPolaska, "dd/MM/yyyy", null)).ToList();
                    }
                    else
                    {
                        soritrani = smestaji.OrderByDescending(a => DateTime.ParseExact(a.DatumPolaska, "dd/MM/yyyy", null)).ToList();
                    }

                break;

                case "DatumPovratka":

                    if (vrsta == "asc")
                    {
                        soritrani = smestaji.OrderBy(a => DateTime.ParseExact(a.DatumPovratka, "dd/MM/yyyy", null)).ToList();
                    }
                    else
                    {
                        soritrani = smestaji.OrderByDescending(a => DateTime.ParseExact(a.DatumPovratka, "dd/MM/yyyy", null)).ToList();
                    }

                    break;
            }

            var korsinik = Session["LOGGEDIN"];

            ViewBag.Korisnik = korsinik;

            return View("Index", soritrani);
        }

        public ActionResult SortirajName(string vrsta)
        {
           var smestaji = aranzmanService.GetAllAranzman();
            List<Aranzman> soritrani = new List<Aranzman>();
            if (vrsta == "asc")
            {
                soritrani = smestaji.OrderBy(a => a.NazivAranzmana).ToList();
            }
            else
            {
                soritrani = smestaji.OrderByDescending(a => a.NazivAranzmana).ToList();
            }

            var korsinik = Session["LOGGEDIN"];

            ViewBag.Korisnik = korsinik;

            return View("Index", soritrani);
        }

        public ActionResult Auth()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PretragaAranzmana(DateTime? minDatumPolaska, DateTime? maxDatumPolaska, DateTime? minDatumPovratka, DateTime? maxDatumPovratka, TipPrevoza? tipPrevoza,TipAranzmana? tipAranzmana, string nazivAranzmana, string akcija)
        {
            List<Aranzman> aranzmani = aranzmanService.GetAllAranzman();
            var korsinik = Session["LOGGEDIN"];

            if (akcija == "osvezi")
            {
                ViewBag.Korisnik = korsinik;
                return View("Index", aranzmani);
            }

            if (minDatumPolaska != null)
            {
                aranzmani = pretragaMinDatumPolaska(aranzmani, minDatumPolaska.Value);
            }

            if (maxDatumPolaska != null)
            {
                aranzmani = pretragaMaxDatumPolaska(aranzmani, maxDatumPolaska.Value);
            }

            if (minDatumPovratka != null)
            {
                aranzmani = pretragaMinDatumPovratka(aranzmani, minDatumPovratka.Value);
            }

            if (maxDatumPovratka != null)
            {
                aranzmani = pretragaMaxDatumPovratka(aranzmani, maxDatumPovratka.Value);
            }

            if (!tipPrevoza.ToString().IsEmpty())
            {
                aranzmani = pretragaTipPrevoza(aranzmani, tipPrevoza.Value);
            }

            if (!tipAranzmana.ToString().IsEmpty())
            {
                aranzmani = pretragaTipAranzmana(aranzmani, tipAranzmana.Value);
            }

            if (!nazivAranzmana.IsEmpty())
            {
                aranzmani = pretragaNaziv(aranzmani, nazivAranzmana);
            }



            ViewBag.Korisnik = korsinik;

            return View("Index", aranzmani);
        }

        private List<Aranzman> pretragaMinDatumPolaska(List<Aranzman> aranzmani, DateTime minDatumPolaska)
        {
            List<Aranzman> filtrirani = new List<Aranzman>();

            foreach (var item in aranzmani)
            {
                if(item.DatumPolaska.AsDateTime() >= minDatumPolaska)
                {
                    filtrirani.Add(item);
                }
            }

            return filtrirani;
        }

        private List<Aranzman> pretragaMaxDatumPolaska(List<Aranzman> arazmani,DateTime maxDatumPolaska)
        {
            List<Aranzman> filtrirani = new List<Aranzman>();

            foreach (var item in arazmani)
            {
                if (item.DatumPolaska.AsDateTime() <= maxDatumPolaska)
                {
                    filtrirani.Add(item);
                }
            }

            return filtrirani;
        }

        private List<Aranzman> pretragaMinDatumPovratka(List<Aranzman> aranzmani, DateTime minDatumPovratka)
        {
            List<Aranzman> filtrirani = new List<Aranzman>();
            foreach (var item in aranzmani)
            {
                if (item.DatumPovratka.AsDateTime() >= minDatumPovratka)
                {
                    filtrirani.Add(item);
                }
            }
            return filtrirani;
        }

        private List<Aranzman> pretragaMaxDatumPovratka(List<Aranzman> aranzmani, DateTime maxDatumPovratka)
        {
            List<Aranzman> filtrirani = new List<Aranzman>();
            foreach (var item in aranzmani)
            {
                if (item.DatumPovratka.AsDateTime() <= maxDatumPovratka)
                {
                    filtrirani.Add(item);
                }
            }
            return filtrirani;
        }

        private List<Aranzman> pretragaTipPrevoza(List<Aranzman> aranzmani, TipPrevoza tipPrevoza)
        {
            List<Aranzman> filtrirani = new List<Aranzman>();
            foreach (var item in aranzmani)
            {
                if (item.TipPrevoza == tipPrevoza)
                {
                    filtrirani.Add(item);
                }
            }
            return filtrirani;
        }

        private List<Aranzman> pretragaTipAranzmana(List<Aranzman> aranzmani, TipAranzmana tipAranzmana)
        {
            List<Aranzman> filtrirani = new List<Aranzman>();
            foreach (var item in aranzmani)
            {
                if (item.TipAranzmana == tipAranzmana)
                {
                    filtrirani.Add(item);
                }
            }
            return filtrirani;
        }

        private List<Aranzman> pretragaNaziv(List<Aranzman> aranzmani, string naziv)
        {
            List<Aranzman> filtrirani = new List<Aranzman>();
            foreach (var item in aranzmani)
            {
                if (item.NazivAranzmana.ToLower().Contains(naziv.ToLower()))
                {
                    filtrirani.Add(item);
                }
            }
            return filtrirani;
        }

    }
}