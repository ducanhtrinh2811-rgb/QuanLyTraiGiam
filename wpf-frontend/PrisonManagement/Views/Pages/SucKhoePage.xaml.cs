using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class SucKhoePage : Page
    {
        private readonly ApiService _apiService;

        public SucKhoePage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            Loaded += SucKhoePage_Loaded;
        }

        private async void SucKhoePage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = await _apiService.GetSucKhoeAsync();
                dgSucKhoe.ItemsSource = data;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
