using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class KyLuatPage : Page
    {
        private readonly ApiService _apiService;

        public KyLuatPage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            Loaded += KyLuatPage_Loaded;
        }

        private async void KyLuatPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = await _apiService.GetKyLuatAsync();
                dgKyLuat.ItemsSource = data;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
