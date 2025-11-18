using projekatWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace projekatWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AranzmanService aranzmanService = new AranzmanService("~/App_Data/aranzmani.json");
            aranzmanService.UcitajAranzman();//ucitava podatke iz fajla
            HttpContext.Current.Application["AranzmaniService"] = aranzmanService;
            HttpContext.Current.Application["Aranzmani"] = aranzmanService.GetAllAranzman();

            KorisnikService korisnikService = new KorisnikService("~/App_Data/korisnici.json");
            korisnikService.UcitajKorisnike();
            HttpContext.Current.Application["KorisniciService"] = korisnikService;
            HttpContext.Current.Application["Korisnici"] = korisnikService.GetAll();

            SmestajService smestajService = new SmestajService("~/App_Data/smestaj.json");
            smestajService.UcitajSmestaj();
            HttpContext.Current.Application["SmestajService"] = smestajService;
            HttpContext.Current.Application["Smestaji"] = smestajService.GetAll();

            SmestajnaJedinicaService smestajnaJedinicaService = new SmestajnaJedinicaService("~/App_Data/smestajneJedinice.json");
            smestajnaJedinicaService.UcitajSmestajneJedinice();
            HttpContext.Current.Application["SmestajnaJedinicaService"] = smestajnaJedinicaService;
            HttpContext.Current.Application["SmestajneJedinice"] = smestajnaJedinicaService.GetAll();

            KomentarService komentarService = new KomentarService("~/App_Data/komentari.json");
            komentarService.UcitajKomentar();
            HttpContext.Current.Application["KomentarService"] = komentarService;
            HttpContext.Current.Application["Komentar"] = komentarService.GetAll();

            RezervacijaService rezervacijaService = new RezervacijaService("~/App_Data/rezervacija.json");
            rezervacijaService.UcitajRezervacija();
            HttpContext.Current.Application["RezervacijaService"] = rezervacijaService;
            HttpContext.Current.Application["Rezervacije"] = rezervacijaService.GetAll();

        }
    }
}
