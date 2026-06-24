using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SIMS___projekat.Models;
using SIMS___projekat.Services;

namespace SIMS___projekat.Views
{
    public partial class DodajZgraduPage : Page
    {
        private ZgradaService _zgradaService;

        public DodajZgraduPage()
        {
            InitializeComponent();
            _zgradaService = new ZgradaService();
        }

        private void btnUnesiZgradu_Click(object sender, RoutedEventArgs e)
        {
            // Validacija
            if (string.IsNullOrWhiteSpace(txtSifra.Text) || string.IsNullOrWhiteSpace(txtAdresa.Text) ||
                string.IsNullOrWhiteSpace(txtNaselje.Text) || string.IsNullOrWhiteSpace(txtLokacija.Text) ||
                string.IsNullOrWhiteSpace(txtJmbgUpravnika.Text))
            {
                MessageBox.Show("Sva polja su obavezna!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Pokušaj parsiranja broja spratova
            if (!int.TryParse(txtBrojSpratova.Text, out int spratovi))
            {
                MessageBox.Show("Broj spratova mora biti validan broj!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Zgrada novaZgrada = new Zgrada
            {
                Sifra = txtSifra.Text,
                AdresaUlicaIBroj = txtAdresa.Text,
                Naselje = txtNaselje.Text,
                LokacijaGradIDrzava = txtLokacija.Text,
                BrojSpratova = spratovi,
                JmbgUpravnika = txtJmbgUpravnika.Text
            };

            bool uspesno = _zgradaService.DodajZgradu(novaZgrada);

            if (uspesno)
            {
                MessageBox.Show("Zgrada je uneta! Čeka se odobrenje upravnika.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                if (NavigationService.CanGoBack) NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Zgrada sa ovom šifrom već postoji!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnNazad_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }
    }
}