using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SIMS___projekat.Models;
using SIMS___projekat.Services;

namespace SIMS___projekat.Views
{
    public partial class MojeZgradePage : Page
    {
        private ZgradaService _zgradaService;
        private StanService _stanService; // Dodali smo servis za stanove
        private Korisnik _ulogovaniUpravnik;

        public MojeZgradePage(Korisnik ulogovaniKorisnik)
        {
            InitializeComponent();
            _zgradaService = new ZgradaService();
            _stanService = new StanService(); // Inicijalizacija servisa
            _ulogovaniUpravnik = ulogovaniKorisnik;

            OsveziTabelu();
        }

        private void OsveziTabelu()
        {
            dgMojeZgrade.ItemsSource = _zgradaService.DobaviZgradeZaUpravnika(_ulogovaniUpravnik.JMBG);
        }

        // --- NOVA LOGIKA ZA ŠIRENJE TABELE ---

        private void btnPrikaziStanove_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Zgrada odabranaZgrada = btn.DataContext as Zgrada;

            // Nalazimo tačan red u tabeli na koji je kliknuto
            DataGridRow row = (DataGridRow)dgMojeZgrade.ItemContainerGenerator.ContainerFromItem(odabranaZgrada);

            if (row != null)
            {
                // Palimo i gasimo rasklapanje reda
                row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void dgMojeZgrade_LoadingRowDetails(object sender, DataGridRowDetailsEventArgs e)
        {
            // Ova metoda se okida TIK PRE nego što se red rasklopi
            Zgrada zgrada = e.Row.Item as Zgrada;

            // Nalazimo onu malu tabelu unutar tog specifičnog reda
            DataGrid dgStanovi = e.DetailsElement.FindName("dgStanoviDetalji") as DataGrid;

            if (zgrada != null && dgStanovi != null)
            {
                // Učitavamo iz baze i filtriramo samo stanove za tu zgradu
                var sviStanovi = _stanService.DobaviSveStanove();
                dgStanovi.ItemsSource = sviStanovi.Where(s => s.SifraZgrade == zgrada.Sifra).ToList();
            }
        }

        // --- STARA LOGIKA ---

        private void btnPrihvati_Click(object sender, RoutedEventArgs e)
        {
            Zgrada odabranaZgrada = (sender as Button).DataContext as Zgrada;

            if (odabranaZgrada.Odobrena)
            {
                MessageBox.Show("Ova zgrada je već odobrena!", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _zgradaService.OdobriZgradu(odabranaZgrada.Sifra);
            MessageBox.Show("Uspešno ste odobrili zgradu. Ona je sada vidljiva u sistemu.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            OsveziTabelu();
        }

        private void btnOdbij_Click(object sender, RoutedEventArgs e)
        {
            Zgrada odabranaZgrada = (sender as Button).DataContext as Zgrada;

            if (odabranaZgrada.Odobrena)
            {
                MessageBox.Show("Već odobrene zgrade se ne mogu odbiti.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult rezultat = MessageBox.Show($"Da li ste sigurni da želite da odbijete (i obrišete) zgradu na adresi {odabranaZgrada.AdresaUlicaIBroj}?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (rezultat == MessageBoxResult.Yes)
            {
                _zgradaService.OdbijZgradu(odabranaZgrada.Sifra);
                MessageBox.Show("Zgrada je uspešno odbijena.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                OsveziTabelu();
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