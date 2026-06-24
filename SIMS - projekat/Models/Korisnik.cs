using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS___projekat.Models
{
    public class Korisnik
    {
        public string JMBG { get; set; }
        public string Email { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string MobilniTelefon { get; set; }
        public TipKorisnika Tip { get; set; }
    }
}
