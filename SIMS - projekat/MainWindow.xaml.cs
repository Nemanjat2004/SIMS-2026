using SIMS___projekat.Models;
using SIMS___projekat.Services;

using System.Windows;
using SIMS___projekat.Views;

namespace SIMS___projekat
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Prilikom pokretanja, učitavamo stranicu za autentifikaciju
            MainFrame.Navigate(new AuthPage());
        }
    }
}