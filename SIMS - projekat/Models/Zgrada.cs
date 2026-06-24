using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS___projekat.Models
{
    public class Zgrada
    {
        public string Sifra { get; set; }
        public string AdresaUlicaIBroj { get; set; }
        public string Naselje { get; set; }
        public string LokacijaGradIDrzava { get; set; }
        public int BrojSpratova { get; set; }
        public string JmbgUpravnika { get; set; }
        public bool Odobrena { get; set; } = false;
    }
}
