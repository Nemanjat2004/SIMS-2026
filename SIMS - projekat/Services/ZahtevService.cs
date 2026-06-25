using System.Collections.Generic;
using System.Linq;
using SIMS___projekat.Models;
using SIMS___projekat.Repositories;

namespace SIMS___projekat.Services
{
    public class ZahtevService
    {
        private readonly JsonRepository<Zahtev> _zahtevRepo;

        public ZahtevService()
        {
            _zahtevRepo = new JsonRepository<Zahtev>("Data/zahtevi.json");
        }

        public bool PosaljiZahtev(Zahtev noviZahtev)
        {
            var sviZahtevi = _zahtevRepo.UcitajSve();

            // Sprečavamo da stanar spama zahteve za isti stan ako je već poslao
            bool vecPostoji = sviZahtevi.Any(z => z.JmbgStanara == noviZahtev.JmbgStanara &&
                                                  z.SifraZgrade == noviZahtev.SifraZgrade &&
                                                  z.BrojStana == noviZahtev.BrojStana);

            if (vecPostoji)
            {
                return false;
            }

            sviZahtevi.Add(noviZahtev);
            _zahtevRepo.SacuvajSve(sviZahtevi);
            return true;
        }

        // Metoda koja će nam trebati kasnije da proverimo da li stanar ima odobren pristup
        public bool DaLiJeStanarOdobrenUZgradi(string jmbgStanara, string sifraZgrade)
        {
            return _zahtevRepo.UcitajSve().Any(z => z.JmbgStanara == jmbgStanara &&
                                                    z.SifraZgrade == sifraZgrade &&
                                                    z.Status == "Odobren");
        }

        // Dobavlja sve zahteve koji su poslati za zgrade kojima upravlja prosleđeni upravnik
        public List<Zahtev> DobaviZahteveZaUpravnika(string jmbgUpravnika, ZgradaService zgradaService)
        {
            var sveZgradeUpravnika = zgradaService.DobaviZgradeZaUpravnika(jmbgUpravnika);
            var sviZahtevi = _zahtevRepo.UcitajSve();

            // Filtriramo zahteve: ostaju samo oni gde se ŠifraZgrade nalazi među šiframa zgrada ovog upravnika
            return sviZahtevi.Where(z => sveZgradeUpravnika.Any(b => b.Sifra == z.SifraZgrade)).ToList();
        }

        public void PromeniStatusZahteva(string idZahteva, string noviStatus, string razlog = "")
        {
            var sviZahtevi = _zahtevRepo.UcitajSve();
            var zahtev = sviZahtevi.FirstOrDefault(z => z.Id == idZahteva);

            if (zahtev != null)
            {
                zahtev.Status = noviStatus;

                if (noviStatus == "Odbijen")
                {
                    zahtev.RazlogOdbijanja = razlog;
                }

                _zahtevRepo.SacuvajSve(sviZahtevi);
            }
        }

        // Vraća listu šifara zgrada u kojima ovaj stanar ima odobren zahtev
        public List<string> DobaviSifreZgradaZaStanara(string jmbgStanara)
        {
            return _zahtevRepo.UcitajSve()
                .Where(z => z.JmbgStanara == jmbgStanara && z.Status == "Odobren")
                .Select(z => z.SifraZgrade)
                .Distinct()
                .ToList();
        }

        // Vraća sve JMBG-ove stanara koji imaju odobren pristup u zadatoj zgradi
        public List<string> DobaviJmbgoveStanaraUZgradi(string sifraZgrade)
        {
            return _zahtevRepo.UcitajSve()
                .Where(z => z.SifraZgrade == sifraZgrade && z.Status == "Odobren")
                .Select(z => z.JmbgStanara)
                .Distinct()
                .ToList();
        }

        public List<Zahtev> DobaviZahteveZaStanara(string jmbgStanara)
        {
            return _zahtevRepo.UcitajSve().Where(z => z.JmbgStanara == jmbgStanara).ToList();
        }

        // NOVA METODA: Brisanje (povlačenje) zahteva
        public void ObrisiZahtev(string idZahteva)
        {
            var sviZahtevi = _zahtevRepo.UcitajSve();
            var zahtev = sviZahtevi.FirstOrDefault(z => z.Id == idZahteva);

            if (zahtev != null)
            {
                sviZahtevi.Remove(zahtev);
                _zahtevRepo.SacuvajSve(sviZahtevi);
            }
        }
    }
}