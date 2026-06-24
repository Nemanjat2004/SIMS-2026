using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SIMS___projekat.Repositories
{
    public class JsonRepository<T>
    {
        private readonly string _putanjaDoFajla;

        // Konstruktor prima ime fajla u koji želimo da čuvamo podatke (npr. "korisnici.json")
        public JsonRepository(string imeFajla)
        {
            _putanjaDoFajla = imeFajla;
        }

        // Metoda za učitavanje svih entiteta iz fajla
        public List<T> UcitajSve()
        {
            // Ako fajl još ne postoji, vraćamo praznu listu umesto da program pukne
            if (!File.Exists(_putanjaDoFajla))
            {
                return new List<T>();
            }

            string json = File.ReadAllText(_putanjaDoFajla);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        // Metoda za čuvanje cele liste entiteta u fajl
        public void SacuvajSve(List<T> entiteti)
        {
            // PROVERA FOLDERA: Izvuci ime foldera iz putanje i napravi ga ako ne postoji
            var direktorijum = Path.GetDirectoryName(_putanjaDoFajla);
            if (!string.IsNullOrEmpty(direktorijum) && !Directory.Exists(direktorijum))
            {
                Directory.CreateDirectory(direktorijum);
            }

            var opcije = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(entiteti, opcije);
            File.WriteAllText(_putanjaDoFajla, json);
        }
    }
}