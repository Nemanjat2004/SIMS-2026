using System.Collections.Generic;
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
            // Upravniku nudimo samo njegove zgrade da u njih unosi stanove
            List<Zgrada> njegoveZgrade = _zgradaService.DobaviZgradeZaUpravnika(_ulogovaniUpravnik.JMBG);
            cmbZgrade.ItemsSource = njegoveZgrade;

            if (njegoveZgrade.Count > 0)
            {
                cmbZgrade.SelectedIndex = 0; // Selektujemo prvu po defaultu
            }
            else
            {
                MessageBox.Show("Trenutno nemate nijednu zgradu u sistemu. Prvo vam admin mora dodeliti zgradu.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                btnUnesi.IsEnabled = false; // Gasimo dugme ako nema zgrada
            }
        }

        private void btnUnesi_Click(object sender, RoutedEventArgs e)
        {
            // 1. Provera da li su polja prazna
            if (cmbZgrade.SelectedValue == null || string.IsNullOrWhiteSpace(txtBrojStana.Text) ||
                string.IsNullOrWhiteSpace(txtOpis.Text) || string.IsNullOrWhiteSpace(txtBrojSoba.Text) ||
                string.IsNullOrWhiteSpace(txtMaxStanara.Text))
            {
                MessageBox.Show("Sva polja su obavezna!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
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