using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class SuCoPage : Page
    {
        private readonly ApiService _apiService;

        public SuCoPage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            Loaded += SuCoPage_Loaded;
        }

        private async void SuCoPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = await _apiService.GetSuCoAsync();
                dgSuCo.ItemsSource = data;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
