using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Models;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class ThamGapPage : Page
    {
        private readonly ApiService _apiService;
        private List<ThamGap> _allData = new();

        public ThamGapPage(ApiService apiService)
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
                _allData = await _apiService.GetThamGapAsync();
                dgThamGap.ItemsSource = _allData;
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
            dgThamGap.ItemsSource = string.IsNullOrWhiteSpace(kw) 
                ? _allData 
                : _allData.Where(x => (x.PhamNhan?.HoTen?.ToLower().Contains(kw) ?? false) || 
                                      (x.NguoiThamGap?.ToLower().Contains(kw) ?? false)).ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var d = new ThamGapDialog(_apiService);
            d.Owner = Window.GetWindow(this);
            if (d.ShowDialog() == true)
                _ = LoadData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var s = (sender as Button)?.DataContext as ThamGap;
            if (s != null)
            {
                var d = new ThamGapDialog(_apiService, s);
                d.Owner = Window.GetWindow(this);
                if (d.ShowDialog() == true)
                    _ = LoadData();
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var s = (sender as Button)?.DataContext as ThamGap;
            if (s != null && MessageBox.Show("Xóa?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _apiService.DeleteThamGapAsync(s.Id);
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
