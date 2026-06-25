using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SIMS___projekat.Models;
using SIMS___projekat.Services;

namespace SIMS___projekat.Views
{
    public partial class UnosStanaPage : Page
    {
        private StanService _stanService;
        private ZgradaService _zgradaService;
        private Korisnik _ulogovaniUpravnik;

        public UnosStanaPage(Korisnik ulogovaniKorisnik)
        {
            InitializeComponent();
            _stanService = new StanService();
            _zgradaService = new ZgradaService();
            _ulogovaniUpravnik = ulogovaniKorisnik;

            UcitajZgradeUpravnika();
        }

        private void UcitajZgradeUpravnika()
        {
            // Dobavljamo sve zgrade upravnika i odmah FILTRIRAMO samo one koje su odobrene
            List<Zgrada> njegoveOdobreneZgrade = _zgradaService.DobaviZgradeZaUpravnika(_ulogovaniUpravnik.JMBG)
                                                               .Where(z => z.Odobrena == true)
                                                               .ToList();

            cmbZgrade.ItemsSource = njegoveOdobreneZgrade;

            if (njegoveOdobreneZgrade.Count > 0)
            {
                cmbZgrade.SelectedIndex = 0; // Selektujemo prvu po defaultu
                btnUnesi.IsEnabled = true;   // Palimo dugme
            }
            else
            {
                MessageBox.Show("Trenutno nemate nijednu odobrenu zgradu. Prvo morate prihvatiti neku zgradu da biste u nju dodavali stanove.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                btnUnesi.IsEnabled = false; // Gasimo dugme da ne bi mogao da klikne i unese u "prazno"
            }
        }

        private void btnUnesi_Click(object sender, RoutedEventArgs e)
        {
            // 1. Provera da li su polja prazna i da li je ZGRADA IZABRANA
            if (cmbZgrade.SelectedValue == null || string.IsNullOrWhiteSpace(txtBrojStana.Text) ||
                string.IsNullOrWhiteSpace(txtOpis.Text) || string.IsNullOrWhiteSpace(txtBrojSoba.Text) ||
                string.IsNullOrWhiteSpace(txtMaxStanara.Text))
            {
                MessageBox.Show("Sva polja su obavezna i zgrada mora biti izabrana!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Parsiranje brojeva (broj stana, broj soba, max stanara)
            if (!int.TryParse(txtBrojStana.Text, out int brojStana) ||
                !int.TryParse(txtBrojSoba.Text, out int brojSoba) ||
                !int.TryParse(txtMaxStanara.Text, out int maxStanara))
            {
                MessageBox.Show("Broj stana, broj soba i maksimalan broj stanara moraju biti validni brojevi!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 3. Pravljenje objekta stana
            Stan noviStan = new Stan
            {
                BrojStana = brojStana,
                Opis = txtOpis.Text,
                BrojSoba = brojSoba,
                MaxBrojStanara = maxStanara,
                SifraZgrade = cmbZgrade.SelectedValue.ToString() // Uzimamo šifru iz ComboBox-a
            };

            // 4. Čuvanje stana
            bool uspesno = _stanService.DodajStan(noviStan);

            if (uspesno)
            {
                MessageBox.Show("Stan je uspešno unet u zgradu!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);

                // Čistimo polja za eventualni novi unos
                txtBrojStana.Clear();
                txtOpis.Clear();
                txtBrojSoba.Clear();
                txtMaxStanara.Clear();
            }
            else
            {
                MessageBox.Show("Stan sa ovim brojem već postoji u izabranoj zgradi!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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