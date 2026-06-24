using SIMS___projekat.Models;
using SIMS___projekat.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace SIMS___projekat.Services
{
    public class AuthService
    {
        private readonly JsonRepository<Korisnik> _korisnikRepo;

        public AuthService()
        {
            // Sada putanja jasno pokazuje na Data folder
            _korisnikRepo = new JsonRepository<Korisnik>("Data/korisnici.json");
        }

        // Metoda za prijavu na sistem
        public Korisnik Prijava(string email, string lozinka)
        {
            // 1. Učitaj sve korisnike iz fajla (trenutno imamo samo onog ručno unetog admina)
            var sviKorisnici = _korisnikRepo.UcitajSve();

            // 2. Tražimo korisnika sa poklapanjem email-a i lozinke
            Korisnik pronadjeniKorisnik = sviKorisnici.FirstOrDefault(k => k.Email == email && k.Lozinka == lozinka);

            // 3. Vraćamo korisnika ako postoji (uspešna prijava), u suprotnom vraćamo null (pogrešni podaci)
            return pronadjeniKorisnik;
        }

        // Metoda za registraciju novog stanara
        public bool RegistracijaStanara(Korisnik noviStanar)
        {
            var sviKorisnici = _korisnikRepo.UcitajSve();

            // Provera jedinstvenosti: JMBG, Email i Lozinka moraju biti jedinstveni prema specifikaciji
            bool vecPostoji = sviKorisnici.Any(k =>
                k.Email == noviStanar.Email ||
                k.Lozinka == noviStanar.Lozinka ||
                k.JMBG == noviStanar.JMBG);

            if (vecPostoji)
            {
                return false; // Vraćamo false ako podaci nisu jedinstveni
            }

            // Osiguravamo da tip korisnika bude Stanar
            noviStanar.Tip = TipKorisnika.Stanar;

            // Dodajemo novog stanara u listu i snimamo u fajl
            sviKorisnici.Add(noviStanar);
            _korisnikRepo.SacuvajSve(sviKorisnici);

            return true; // Uspešna registracija
        }

        public bool RegistracijaUpravnika(Korisnik noviUpravnik)
        {
            var sviKorisnici = _korisnikRepo.UcitajSve();

            // Provera jedinstvenosti po specifikaciji (JMBG i Email)
            bool vecPostoji = sviKorisnici.Any(k =>
                k.Email == noviUpravnik.Email ||
                k.JMBG == noviUpravnik.JMBG);

            if (vecPostoji)
            {
                return false; // Već postoji korisnik sa ovim podacima
            }

            // Osiguravamo da tip korisnika bude Upravnik
            noviUpravnik.Tip = TipKorisnika.Upravnik;

            sviKorisnici.Add(noviUpravnik);
            _korisnikRepo.SacuvajSve(sviKorisnici);

            return true;
        }

        public List<Korisnik> DobaviSveKorisnike()
        {
            return _korisnikRepo.UcitajSve();
        }
    }
}