using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SIMS___projekat.Models;
using SIMS___projekat.Services;

namespace SIMS___projekat.Views
{
    public partial class OdobravanjeZahtevaPage : Page
    {
        private ZahtevService _zahtevService;
        private ZgradaService _zgradaService;
        private Korisnik _ulogovaniUpravnik;

        // NOVO: Ovde čuvamo sve zahteve kako bismo mogli da ih filtriramo
        private List<Zahtev> _sviZahtevi;

        public OdobravanjeZahtevaPage(Korisnik ulogovaniKorisnik)
        {
            InitializeComponent();
            _zahtevService = new ZahtevService();
            _zgradaService = new ZgradaService();
            _ulogovaniUpravnik = ulogovaniKorisnik;

            OsveziTabelu();
        }

        private void OsveziTabelu()
        {
            // Učitavamo sve zahteve iz baze, a zatim zovemo filter koji će popuniti tabelu
            _sviZahtevi = _zahtevService.DobaviZahteveZaUpravnika(_ulogovaniUpravnik.JMBG, _zgradaService);
            PrimeniFilter();
        }

        // --- NOVA LOGIKA ZA FILTRIRANJE ---
        private void PrimeniFilter()
        {
            if (cmbFilter == null || _sviZahtevi == null) return;

            string izabraniFilter = (cmbFilter.SelectedItem as ComboBoxItem).Content.ToString();

            if (izabraniFilter == "Svi zahtevi")
            {
                dgZahtevi.ItemsSource = _sviZahtevi;
            }
            else
            {
                dgZahtevi.ItemsSource = _sviZahtevi.Where(z => z.Status == izabraniFilter).ToList();
            }
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PrimeniFilter();
        }
        // ----------------------------------

        private void btnOdobri_Click(object sender, RoutedEventArgs e)
        {
            Zahtev odabraniZahtev = (sender as Button).DataContext as Zahtev;

            if (odabraniZahtev.Status != "Na cekanju")
            {
                MessageBox.Show($"Ovaj zahtev je već procesiran i ima status: {odabraniZahtev.Status}", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _zahtevService.PromeniStatusZahteva(odabraniZahtev.Id, "Odobren");
            MessageBox.Show("Zahtev stanara je uspešno odobren!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            OsveziTabelu();
        }

        private void btnOdbij_Click(object sender, RoutedEventArgs e)
        {
            Zahtev odabraniZahtev = (sender as Button).DataContext as Zahtev;

            if (odabraniZahtev.Status != "Na cekanju")
            {
                MessageBox.Show($"Ovaj zahtev je već procesiran i ima status: {odabraniZahtev.Status}", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _zahtevService.PromeniStatusZahteva(odabraniZahtev.Id, "Odbijen", "Netačni podaci");
            MessageBox.Show("Zahtev stanara je odbijen.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            OsveziTabelu();
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