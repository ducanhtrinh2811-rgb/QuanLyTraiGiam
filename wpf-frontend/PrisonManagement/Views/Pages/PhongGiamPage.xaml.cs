using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class PhongGiamPage : Page
    {
        private readonly ApiService _apiService;

        public PhongGiamPage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            Loaded += PhongGiamPage_Loaded;
        }

        private async void PhongGiamPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = await _apiService.GetPhongGiamAsync();
                dgPhongGiam.ItemsSource = data;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
