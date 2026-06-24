using SIMS___projekat.Models;
using SIMS___projekat.Services;
using System.Windows;
using System.Windows.Controls;


namespace SIMS___projekat.Views
{
    public partial class AuthPage : Page
    {
        private AuthService _authService;

        public AuthPage()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        // --- NAVIGACIJA (Prebacivanje prikaza) ---
        private void btnPrebaciNaRegistraciju_Click(object sender, RoutedEventArgs e)
        {
            panelPrijava.Visibility = Visibility.Collapsed;
            panelRegistracija.Visibility = Visibility.Visible;
        }

        private void btnPrebaciNaPrijavu_Click(object sender, RoutedEventArgs e)
        {
            panelRegistracija.Visibility = Visibility.Collapsed;
            panelPrijava.Visibility = Visibility.Visible;
        }

        // --- LOGIKA ZA PRIJAVU ---
        private void btnPrijava_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmailPrijava.Text;
            string lozinka = txtLozinkaPrijava.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(lozinka))
            {
                MessageBox.Show("Molimo vas da unesete email i lozinku.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Korisnik prijavljeniKorisnik = _authService.Prijava(email, lozinka);

            if (prijavljeniKorisnik != null)
            {
                // Umesto MessageBox-a, sada radimo navigaciju
                NavigationService.Navigate(new DashboardPage(prijavljeniKorisnik));
            }
            else
            {
                MessageBox.Show("Pogrešan email ili lozinka.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- LOGIKA ZA REGISTRACIJU ---
        private void btnRegistruj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtJmbgReg.Text) || string.IsNullOrWhiteSpace(txtImeReg.Text) ||
                string.IsNullOrWhiteSpace(txtPrezimeReg.Text) || string.IsNullOrWhiteSpace(txtEmailReg.Text) ||
                string.IsNullOrWhiteSpace(txtLozinkaReg.Password) || string.IsNullOrWhiteSpace(txtTelefonReg.Text))
            {
                MessageBox.Show("Sva polja su obavezna za registraciju!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Korisnik noviStanar = new Korisnik
            {
                JMBG = txtJmbgReg.Text,
                Ime = txtImeReg.Text,
                Prezime = txtPrezimeReg.Text,
                MobilniTelefon = txtTelefonReg.Text,
                Email = txtEmailReg.Text,
                Lozinka = txtLozinkaReg.Password
            };

            bool uspesno = _authService.RegistracijaStanara(noviStanar);

            if (uspesno)
            {
                MessageBox.Show("Uspešno ste se registrovali! Sada se možete prijaviti.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);

                txtJmbgReg.Clear();
                txtImeReg.Clear();
                txtPrezimeReg.Clear();
                txtTelefonReg.Clear();
                txtEmailReg.Clear();
                txtLozinkaReg.Password = "";

                btnPrebaciNaPrijavu_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Korisnik sa ovim JMBG-om, email-om ili lozinkom već postoji!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}