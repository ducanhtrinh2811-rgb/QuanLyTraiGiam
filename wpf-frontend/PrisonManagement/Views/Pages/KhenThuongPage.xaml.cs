using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class KhenThuongPage : Page
    {
        private readonly ApiService _apiService;

        public KhenThuongPage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            Loaded += KhenThuongPage_Loaded;
        }

        private async void KhenThuongPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = await _apiService.GetKhenThuongAsync();
                dgKhenThuong.ItemsSource = data;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
