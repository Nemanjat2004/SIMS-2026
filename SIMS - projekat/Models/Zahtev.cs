namespace SIMS___projekat.Models
{
    public class Zahtev
    {
        public string Id { get; set; } = System.Guid.NewGuid().ToString(); // Jedinstveni ID zahteva
        public string JmbgStanara { get; set; }
        public string SifraZgrade { get; set; }
        public int BrojStana { get; set; }
        public string Status { get; set; } = "Na cekanju"; // Može biti: "Na cekanju", "Odobren", "Odbijen"
        public string RazlogOdbijanja { get; set; } = ""; // Dodato za odbijene zahteve
    }
}