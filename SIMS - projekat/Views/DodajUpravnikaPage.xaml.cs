using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SIMS___projekat.Models;
using SIMS___projekat.Services;

namespace SIMS___projekat.Views
{
    public partial class DodajUpravnikaPage : Page
    {
        private AuthService _authService;

        public DodajUpravnikaPage()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void btnRegistruj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtJmbg.Text) || string.IsNullOrWhiteSpace(txtIme.Text) ||
                string.IsNullOrWhiteSpace(txtPrezime.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtLozinka.Password) || string.IsNullOrWhiteSpace(txtTelefon.Text))
            {
                MessageBox.Show("Sva polja su obavezna!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Korisnik noviUpravnik = new Korisnik
            {
                JMBG = txtJmbg.Text,
                Ime = txtIme.Text,
                Prezime = txtPrezime.Text,
                MobilniTelefon = txtTelefon.Text,
                Email = txtEmail.Text,
                Lozinka = txtLozinka.Password
            };

            bool uspesno = _authService.RegistracijaUpravnika(noviUpravnik);

            if (uspesno)
            {
                MessageBox.Show("Upravnik je uspešno dodat u sistem!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                // Vraćamo se na prethodnu stranicu (Dashboard)
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
            else
            {
                MessageBox.Show("Korisnik sa unetim JMBG-om ili email-om već postoji!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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