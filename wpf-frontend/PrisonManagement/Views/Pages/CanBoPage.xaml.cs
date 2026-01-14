using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class CanBoPage : Page
    {
        private readonly ApiService _apiService;

        public CanBoPage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            Loaded += CanBoPage_Loaded;
        }

        private async void CanBoPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            try
            {
                var data = await _apiService.GetCanBoAsync();
                dgCanBo.ItemsSource = data;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            _ = LoadData();
        }
    }
}
