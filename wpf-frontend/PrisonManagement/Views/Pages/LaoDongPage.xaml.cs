using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class LaoDongPage : Page
    {
        private readonly ApiService _apiService;

        public LaoDongPage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            Loaded += LaoDongPage_Loaded;
        }

        private async void LaoDongPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = await _apiService.GetLaoDongAsync();
                dgLaoDong.ItemsSource = data;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
