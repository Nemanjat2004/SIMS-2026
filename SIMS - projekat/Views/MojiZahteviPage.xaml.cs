using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SIMS___projekat.Models;
using SIMS___projekat.Services;

namespace SIMS___projekat.Views
{
    public partial class MojiZahteviPage : Page
    {
        private ZahtevService _zahtevService;
        private Korisnik _ulogovaniStanar;
        private List<Zahtev> _sviMojiZahtevi;

        public MojiZahteviPage(Korisnik ulogovaniKorisnik)
        {
            InitializeComponent();
            _zahtevService = new ZahtevService();
            _ulogovaniStanar = ulogovaniKorisnik;

            UcitajZahteve();
        }

        private void UcitajZahteve()
        {
            _sviMojiZahtevi = _zahtevService.DobaviZahteveZaStanara(_ulogovaniStanar.JMBG);
            PrimeniFilter();
        }

        private void PrimeniFilter()
        {
            if (cmbFilter == null || _sviMojiZahtevi == null) return;

            string izabraniFilter = (cmbFilter.SelectedItem as ComboBoxItem).Content.ToString();

            if (izabraniFilter == "Svi zahtevi")
            {
                dgMojiZahtevi.ItemsSource = _sviMojiZahtevi;
            }
            else
            {
                // Filtriramo listu po statusu ("Na cekanju", "Odobren", "Odbijen")
                dgMojiZahtevi.ItemsSource = _sviMojiZahtevi.Where(z => z.Status == izabraniFilter).ToList();
            }
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PrimeniFilter();
        }

        private void btnPovuci_Click(object sender, RoutedEventArgs e)
        {
            Zahtev odabraniZahtev = (sender as Button).DataContext as Zahtev;

            if (odabraniZahtev.Status != "Na cekanju")
            {
                MessageBox.Show("Samo zahtevi koji su 'Na cekanju' se mogu povući!", "Zabranjeno", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni da želite da povučete (obrišete) ovaj zahtev?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (rezultat == MessageBoxResult.Yes)
            {
                _zahtevService.ObrisiZahtev(odabraniZahtev.Id);
                MessageBox.Show("Zahtev je uspešno povučen.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                UcitajZahteve(); // Osvežavamo tabelu nakon brisanja
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