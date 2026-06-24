using System.Collections.Generic;
using System.Linq;
using SIMS___projekat.Models;
using SIMS___projekat.Repositories;

namespace SIMS___projekat.Services
{
    public class ZgradaService
    {
        private readonly JsonRepository<Zgrada> _zgradaRepo;

        public ZgradaService()
        {
            _zgradaRepo = new JsonRepository<Zgrada>("Data/zgrade.json");
        }

        public bool DodajZgradu(Zgrada novaZgrada)
        {
            var sveZgrade = _zgradaRepo.UcitajSve();

            // Specifikacija kaže da je šifra jedinstvena
            if (sveZgrade.Any(z => z.Sifra == novaZgrada.Sifra))
            {
                return false;
            }

            novaZgrada.Odobrena = false; // Inicijalno čeka upravnika
            sveZgrade.Add(novaZgrada);
            _zgradaRepo.SacuvajSve(sveZgrade);

            return true;
        }

        public List<Zgrada> DobaviOdobreneZgrade()
        {
            var sveZgrade = _zgradaRepo.UcitajSve();
            // Zgrada je vidljiva tek kada se odobri, prema specifikaciji
            return sveZgrade.Where(z => z.Odobrena == true).ToList();
        }

        public List<Zgrada> DobaviSveZgrade()
        {
            return _zgradaRepo.UcitajSve();
        }

        // Dobavlja sve zgrade vezane za JMBG ulogovanog upravnika
        public List<Zgrada> DobaviZgradeZaUpravnika(string jmbgUpravnika)
        {
            return _zgradaRepo.UcitajSve().Where(z => z.JmbgUpravnika == jmbgUpravnika).ToList();
        }

        public void OdobriZgradu(string sifraZgrade)
        {
            var sveZgrade = _zgradaRepo.UcitajSve();
            var zgrada = sveZgrade.FirstOrDefault(z => z.Sifra == sifraZgrade);

            if (zgrada != null)
            {
                zgrada.Odobrena = true;
                _zgradaRepo.SacuvajSve(sveZgrade);
            }
        }

        public void OdbijZgradu(string sifraZgrade)
        {
            var sveZgrade = _zgradaRepo.UcitajSve();
            var zgrada = sveZgrade.FirstOrDefault(z => z.Sifra == sifraZgrade);

            if (zgrada != null)
            {
                sveZgrade.Remove(zgrada); // Brišemo pogrešno unetu zgradu
                _zgradaRepo.SacuvajSve(sveZgrade);
            }
        }
    }
}