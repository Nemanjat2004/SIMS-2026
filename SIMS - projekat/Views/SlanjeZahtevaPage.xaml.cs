using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SIMS___projekat.Models;
using SIMS___projekat.Services;

namespace SIMS___projekat.Views
{
    public partial class SlanjeZahtevaPage : Page
    {
        private ZgradaService _zgradaService;
        private StanService _stanService;
        private ZahtevService _zahtevService;
        private Korisnik _ulogovaniStanar;

        public SlanjeZahtevaPage(Korisnik ulogovaniKorisnik)
        {
            InitializeComponent();
            _zgradaService = new ZgradaService();
            _stanService = new StanService();
            _zahtevService = new ZahtevService();
            _ulogovaniStanar = ulogovaniKorisnik;

            UcitajZgrade();
        }

        private void UcitajZgrade()
        {
            // Stanar može da traži pristup samo u zgradama koje su odobrene!
            List<Zgrada> odobreneZgrade = _zgradaService.DobaviOdobreneZgrade();
            cmbZgrade.ItemsSource = odobreneZgrade;
        }

        // Ova metoda se okida svaki put kada korisnik klikne na neku zgradu u padajućem meniju
        private void cmbZgrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbZgrade.SelectedValue != null)
            {
                string izabranaSifraZgrade = cmbZgrade.SelectedValue.ToString();

                // Učitavamo sve stanove i ostavljamo samo one koji su u izabranoj zgradi
                var sviStanovi = _stanService.DobaviSveStanove();
                var stanoviUZgradi = sviStanovi.Where(s => s.SifraZgrade == izabranaSifraZgrade).ToList();

                cmbStanovi.ItemsSource = stanoviUZgradi;

                // Opcija: Prikazujemo i mali opis stana u padajućem meniju radi lakšeg snalaženja
                cmbStanovi.DisplayMemberPath = "BrojStana";

                if (stanoviUZgradi.Count == 0)
                {
                    MessageBox.Show("U ovoj zgradi još uvek nema unetih stanova.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void btnPosalji_Click(object sender, RoutedEventArgs e)
        {
            if (cmbZgrade.SelectedValue == null || cmbStanovi.SelectedValue == null)
            {
                MessageBox.Show("Morate izabrati i zgradu i stan da biste poslali zahtev!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Zahtev noviZahtev = new Zahtev
            {
                JmbgStanara = _ulogovaniStanar.JMBG,
                SifraZgrade = cmbZgrade.SelectedValue.ToString(),
                BrojStana = (int)cmbStanovi.SelectedValue
            };

            bool uspesno = _zahtevService.PosaljiZahtev(noviZahtev);

            if (uspesno)
            {
                MessageBox.Show("Vaš zahtev je uspešno poslat upravniku na odobrenje!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                if (NavigationService.CanGoBack) NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Već ste poslali zahtev za ovaj stan!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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