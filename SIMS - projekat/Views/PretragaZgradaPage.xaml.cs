using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SIMS___projekat.Models;
using SIMS___projekat.Services;

namespace SIMS___projekat.Views
{
    public partial class PretragaZgradaPage : Page
    {
        private ZgradaService _zgradaService;
        private Korisnik _ulogovaniKorisnik;
        private List<Zgrada> _sveDostupneZgrade;

        public PretragaZgradaPage(Korisnik ulogovaniKorisnik)
        {
            InitializeComponent();
            _zgradaService = new ZgradaService();
            _ulogovaniKorisnik = ulogovaniKorisnik;

            // Učitavamo bazu zgrada pri otvaranju (Admin vidi sve, ostali samo odobrene)
            if (_ulogovaniKorisnik.Tip == TipKorisnika.Administrator)
            {
                _sveDostupneZgrade = _zgradaService.DobaviSveZgrade();
            }
            else
            {
                _sveDostupneZgrade = _zgradaService.DobaviOdobreneZgrade();
            }

            // Inicijalno prikazujemo sve
            dgRezultati.ItemsSource = _sveDostupneZgrade;
        }

        private void btnTrazi_Click(object sender, RoutedEventArgs e)
        {
            string upit = txtUpit.Text.Trim().ToLower(); // Spuštamo na mala slova zbog specifikacije
            int selektovaniKriterijum = cmbKriterijum.SelectedIndex;

            List<Zgrada> rezultati = new List<Zgrada>();

            // Ako je polje prazno, prikazujemo sve zgrade
            if (string.IsNullOrWhiteSpace(upit))
            {
                dgRezultati.ItemsSource = _sveDostupneZgrade;
                return;
            }

            switch (selektovaniKriterijum)
            {
                case 0: // Adresa
                    rezultati = _sveDostupneZgrade.Where(z => z.AdresaUlicaIBroj.ToLower().Contains(upit)).ToList();
                    break;

                case 1: // Naselje
                    rezultati = _sveDostupneZgrade.Where(z => z.Naselje.ToLower().Contains(upit)).ToList();
                    break;

                case 2: // Broj spratova
                    if (int.TryParse(upit, out int brojSpratova))
                    {
                        rezultati = _sveDostupneZgrade.Where(z => z.BrojSpratova == brojSpratova).ToList();
                    }
                    else
                    {
                        MessageBox.Show("Za pretragu po spratovima morate uneti broj!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    break;

                case 3: // Stanovi
                    MessageBox.Show("Pretraga po stanovima i logičkim operatorima će biti dostupna nakon unosa stanova od strane upravnika.", "Uskoro", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
            }

            // Prikazujemo filtrirane rezultate
            dgRezultati.ItemsSource = rezultati;

            if (rezultati.Count == 0)
            {
                MessageBox.Show("Nema zgrada koje odgovaraju vašem upitu.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnPonisti_Click(object sender, RoutedEventArgs e)
        {
            // Resetujemo polja
            txtUpit.Text = string.Empty;
            cmbKriterijum.SelectedIndex = 0;

            // Vraćamo tabelu na početno stanje (prikazuje sve dostupne zgrade)
            dgRezultati.ItemsSource = _sveDostupneZgrade;
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