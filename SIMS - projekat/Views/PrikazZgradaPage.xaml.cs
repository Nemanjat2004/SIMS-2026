using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SIMS___projekat.Models;
using SIMS___projekat.Services;

namespace SIMS___projekat.Views
{
    public partial class PrikazZgradaPage : Page
    {
        private ZgradaService _zgradaService;
        private Korisnik _ulogovaniKorisnik;
        private List<Zgrada> _svePrikazaneZgrade;

        public PrikazZgradaPage(Korisnik ulogovaniKorisnik)
        {
            InitializeComponent();
            _zgradaService = new ZgradaService();
            _ulogovaniKorisnik = ulogovaniKorisnik;

            UcitajZgrade();
        }

        private void UcitajZgrade()
        {
            // Ako je admin, vidi sve zgrade (pa i one neodobrene). Svi ostali vide samo odobrene.
            if (_ulogovaniKorisnik.Tip == TipKorisnika.Administrator)
            {
                _svePrikazaneZgrade = _zgradaService.DobaviSveZgrade();
            }
            else
            {
                _svePrikazaneZgrade = _zgradaService.DobaviOdobreneZgrade();
            }

            OsveziTabelu();
        }

        private void OsveziTabelu()
        {
            // Logika za sortiranje po broju spratova iz specifikacije
            if (chkSortirajSpratovi.IsChecked == true)
            {
                // Sortiramo od najveće ka najmanjoj
                dgZgrade.ItemsSource = _svePrikazaneZgrade.OrderByDescending(z => z.BrojSpratova).ToList();
            }
            else
            {
                // Originalni redosled
                dgZgrade.ItemsSource = _svePrikazaneZgrade;
            }
        }

        private void chkSortirajSpratovi_Changed(object sender, RoutedEventArgs e)
        {
            OsveziTabelu();
        }

        private void btnSpisakStanara_Click(object sender, RoutedEventArgs e)
        {
            // Kako bismo izvukli tačnu zgradu na koju je korisnik kliknuo u tabeli:
            Zgrada odabranaZgrada = (sender as Button).DataContext as Zgrada;

            MessageBox.Show($"Kliknuli ste na zgradu {odabranaZgrada.AdresaUlicaIBroj}.\n\nOva funkcija će prikazati listu stanara čim implementiramo Zahteve u sistemu!",
                            "Uskoro", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnNazad_Click(object sender, RoutedEventArgs e)
        {
            // Vraća nas na prethodnu stranicu (Dashboard)
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}