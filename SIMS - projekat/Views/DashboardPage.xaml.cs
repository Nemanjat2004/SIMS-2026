using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SIMS___projekat.Models;

namespace SIMS___projekat.Views
{
    public partial class DashboardPage : Page
    {
        private Korisnik _ulogovaniKorisnik;

        // Konstruktor prima ulogovanog korisnika
        public DashboardPage(Korisnik korisnik)
        {
            InitializeComponent();
            _ulogovaniKorisnik = korisnik;

            // Postavljamo tekst dobrodošlice
            txtDobrodoslica.Text = $"Dobrodošli, {_ulogovaniKorisnik.Ime} {_ulogovaniKorisnik.Prezime} ({_ulogovaniKorisnik.Tip})";

            PrilagodiMeni();
        }

        private void PrilagodiMeni()
        {
            if (_ulogovaniKorisnik.Tip == TipKorisnika.Administrator)
            {
                panelAdmin.Visibility = Visibility.Visible;
            }
            else if (_ulogovaniKorisnik.Tip == TipKorisnika.Upravnik)
            {
                panelUpravnik.Visibility = Visibility.Visible;
            }
            else if (_ulogovaniKorisnik.Tip == TipKorisnika.Stanar)
            {
                panelStanar.Visibility = Visibility.Visible;
            }
        }

        // --- STANAR OPCIJE ---
        private void btnPosaljiZahtev_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SlanjeZahtevaPage(_ulogovaniKorisnik));
        }

        private void btnMojiZahtevi_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MojiZahteviPage(_ulogovaniKorisnik));
        }

        private void btnMojeKomsije_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PrikazKomsijaPage(_ulogovaniKorisnik));
        }

        // --- UPRAVNIK OPCIJE ---
        private void btnMojeZgrade_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MojeZgradePage(_ulogovaniKorisnik));
        }

        private void btnMojiStanovi_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UnosStanaPage(_ulogovaniKorisnik));
        }

        private void btnZahteviStanara_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OdobravanjeZahtevaPage(_ulogovaniKorisnik));
        }

        // --- ADMINISTRATORSKE OPCIJE ---

        private void btnDodajUpravnika_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DodajUpravnikaPage());
        }

        private void btnDodajZgradu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DodajZgraduPage());
        }

        private void btnSviKorisnici_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PrikazKorisnikaPage());
        }

        // --- ZAJEDNIČKE OPCIJE ---

        private void btnPrikazZgrada_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PrikazZgradaPage(_ulogovaniKorisnik));
        }

        private void btnPretragaZgrada_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PretragaZgradaPage(_ulogovaniKorisnik));
        }

        // --- ODJAVA ---

        private void btnOdjava_Click(object sender, RoutedEventArgs e)
        {
            // ISPRAVLJENO: Odjava nas vraća na login ekran (AuthPage)
            NavigationService.Navigate(new AuthPage());
        }
    }
}