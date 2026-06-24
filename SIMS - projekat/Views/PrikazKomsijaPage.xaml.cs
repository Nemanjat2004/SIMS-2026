using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SIMS___projekat.Models;
using SIMS___projekat.Services;

namespace SIMS___projekat.Views
{
    public partial class PrikazKomsijaPage : Page
    {
        private ZahtevService _zahtevService;
        private AuthService _authService;
        private Korisnik _ulogovaniStanar;

        public PrikazKomsijaPage(Korisnik ulogovaniKorisnik)
        {
            InitializeComponent();
            _zahtevService = new ZahtevService();
            _authService = new AuthService();
            _ulogovaniStanar = ulogovaniKorisnik;

            UcitajKomsije();
        }

        private void UcitajKomsije()
        {
            // 1. Saznajemo u kojim zgradama ovaj stanar ima odobren stan
            List<string> sifreZgrada = _zahtevService.DobaviSifreZgradaZaStanara(_ulogovaniStanar.JMBG);

            if (sifreZgrada.Count == 0)
            {
                MessageBox.Show("Nemate pristup nijednoj zgradi. Spisak komšija će biti dostupan tek kada upravnik odobri vaš zahtev!", "Pristup odbijen", MessageBoxButton.OK, MessageBoxImage.Warning);
                dgKomsije.IsEnabled = false;
                return;
            }

            // Uzimamo prvu zgradu u kojoj je odobren (za slučaj da ima samo jednu)
            string primarnaSifraZgrade = sifreZgrada.First();
            txtNaslovKomsije.Text = $"Moje komšije u zgradi: {primarnaSifraZgrade}";

            // 2. Izvlačimo JMBG-ove svih ljudi koji imaju odobren stan u toj zgradi
            List<string> jmbgoviKomsija = _zahtevService.DobaviJmbgoveStanaraUZgradi(primarnaSifraZgrade);

            // 3. Izvlačimo pune profile tih korisnika iz baze (osim samog sebe)
            var sviKorisnici = _authService.DobaviSveKorisnike();
            var komsije = sviKorisnici.Where(k => jmbgoviKomsija.Contains(k.JMBG) && k.JMBG != _ulogovaniStanar.JMBG).ToList();

            dgKomsije.ItemsSource = komsije;
        }

        private void btnNazad_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}