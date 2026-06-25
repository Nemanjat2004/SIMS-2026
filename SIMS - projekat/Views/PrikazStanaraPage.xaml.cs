using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SIMS___projekat.Models;
using SIMS___projekat.Services;

namespace SIMS___projekat.Views
{
    public partial class PrikazStanaraPage : Page
    {
        private Zgrada _odabranaZgrada;
        private ZahtevService _zahtevService;
        private AuthService _authService;

        public PrikazStanaraPage(Zgrada zgrada)
        {
            InitializeComponent();
            _odabranaZgrada = zgrada;
            _zahtevService = new ZahtevService();
            _authService = new AuthService();

            txtNaslov.Text = $"Spisak stanara za zgradu: {_odabranaZgrada.AdresaUlicaIBroj}";
            UcitajStanare();
        }

        private void UcitajStanare()
        {
            // 1. Nalazimo JMBG-ove svih stanara koji imaju ODOBREN zahtev u ovoj zgradi
            List<string> jmbgoviStanara = _zahtevService.DobaviJmbgoveStanaraUZgradi(_odabranaZgrada.Sifra);

            // 2. Izvlačimo njihove profile iz baze
            var sviKorisnici = _authService.DobaviSveKorisnike();
            var stanari = sviKorisnici.Where(k => jmbgoviStanara.Contains(k.JMBG)).ToList();

            dgStanari.ItemsSource = stanari;

            if (stanari.Count == 0)
            {
                MessageBox.Show("Trenutno nema odobrenih stanara u ovoj zgradi.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
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