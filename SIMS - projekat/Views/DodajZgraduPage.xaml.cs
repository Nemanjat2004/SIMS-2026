using System.Linq;
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
        private AuthService _authService; // Dodali smo AuthService za upravnike

        public DodajZgraduPage()
        {
            InitializeComponent();
            _zgradaService = new ZgradaService();
            _authService = new AuthService(); // Inicijalizacija

            UcitajUpravnike();
        }

        private void UcitajUpravnike()
        {
            var sviKorisnici = _authService.DobaviSveKorisnike();

            // Izvlačimo samo upravnike i pravimo lep format za padajući meni
            var upravnici = sviKorisnici
                .Where(k => k.Tip == TipKorisnika.Upravnik)
                .Select(k => new
                {
                    Prikaz = $"{k.Ime} {k.Prezime} (JMBG: {k.JMBG})", // Ovo korisnik vidi
                    Jmbg = k.JMBG // Ovo mi čuvamo u bazu
                }).ToList();

            cmbUpravnici.ItemsSource = upravnici;
            cmbUpravnici.DisplayMemberPath = "Prikaz";       // Šta se crta na ekranu
            cmbUpravnici.SelectedValuePath = "Jmbg";         // Šta se uzima kada uradimo .SelectedValue
        }

        private void btnUnesiZgradu_Click(object sender, RoutedEventArgs e)
        {
            // Provera da li su polja prazna i da li je izabran upravnik
            if (string.IsNullOrWhiteSpace(txtSifra.Text) ||
                string.IsNullOrWhiteSpace(txtAdresa.Text) ||
                cmbUpravnici.SelectedValue == null)
            {
                MessageBox.Show("Molimo vas popunite sva obavezna polja i izaberite upravnika sa padajućeg menija.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
                JmbgUpravnika = cmbUpravnici.SelectedValue.ToString(), // Uzimamo sakriveni JMBG iz ComboBox-a
                Odobrena = false // Administrator unosi, ali se uvek prvo dodaje kao neodobrena (po onoj tvojoj logici)
            };

            _zgradaService.DodajZgradu(novaZgrada);
            MessageBox.Show("Zgrada je uspešno uneta u sistem!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);

            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
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