using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class ThamGapPage : Page
    {
        private readonly ApiService _apiService;

        public ThamGapPage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            Loaded += ThamGapPage_Loaded;
        }

        private async void ThamGapPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = await _apiService.GetThamGapAsync();
                dgThamGap.ItemsSource = data;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
