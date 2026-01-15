using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Models;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class PhongGiamPage : Page
    {
        private readonly ApiService _apiService;
        private List<PhongGiam> _allData = new();

        public PhongGiamPage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            Loaded += async (s, e) => await LoadData();
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            try
            {
                loadingOverlay.Visibility = Visibility.Visible;
                _allData = await _apiService.GetPhongGiamAsync();
                dgPhongGiam.ItemsSource = _allData;
            }
            catch { }
            finally
            {
                loadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var kw = txtSearch.Text.ToLower();
            dgPhongGiam.ItemsSource = string.IsNullOrWhiteSpace(kw) 
                ? _allData 
                : _allData.Where(x => (x.TenPhong?.ToLower().Contains(kw) ?? false) || 
                                      (x.MaPhong?.ToLower().Contains(kw) ?? false)).ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var d = new PhongGiamDialog(_apiService);
            d.Owner = Window.GetWindow(this);
            if (d.ShowDialog() == true)
                _ = LoadData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var s = (sender as Button)?.DataContext as PhongGiam;
            if (s != null)
            {
                var d = new PhongGiamDialog(_apiService, s);
                d.Owner = Window.GetWindow(this);
                if (d.ShowDialog() == true)
                    _ = LoadData();
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var s = (sender as Button)?.DataContext as PhongGiam;
            if (s != null && MessageBox.Show("Xóa?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _apiService.DeletePhongGiamAsync(s.Id);
                await LoadData();
            }
        }

        private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            await LoadData();
        }
    }
}
