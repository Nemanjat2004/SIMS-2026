using System.Collections.Generic;
using System.Linq;
using SIMS___projekat.Models;
using SIMS___projekat.Repositories;

namespace SIMS___projekat.Services
{
    public class StanService
    {
        private readonly JsonRepository<Stan> _stanRepo;

        public StanService()
        {
            _stanRepo = new JsonRepository<Stan>("Data/stanovi.json");
        }

        public List<Stan> DobaviSveStanove()
        {
            return _stanRepo.UcitajSve();
        }

        public bool DodajStan(Stan noviStan)
        {
            var sviStanovi = _stanRepo.UcitajSve();

            // Specifikacija: Broj stana mora biti jedinstven U OKVIRU ZGRADE
            bool vecPostoji = sviStanovi.Any(s => s.SifraZgrade == noviStan.SifraZgrade && s.BrojStana == noviStan.BrojStana);

            if (vecPostoji)
            {
                return false; // Stan sa tim brojem već postoji u toj zgradi
            }

            sviStanovi.Add(noviStan);
            _stanRepo.SacuvajSve(sviStanovi);

            return true;
        }
    }
}