using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SIMS___projekat.Services;

namespace SIMS___projekat.Views
{
    public partial class PrikazKorisnikaPage : Page
    {
        private AuthService _authService;

        public PrikazKorisnikaPage()
        {
            InitializeComponent();
            _authService = new AuthService();
            dgKorisnici.ItemsSource = _authService.DobaviSveKorisnike();
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