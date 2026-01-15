using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Models;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class PhamNhanPage : Page
    {
        private readonly ApiService _apiService;
        private List<PhamNhan> _allPhamNhan = new();

        public PhamNhanPage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            Loaded += PhamNhanPage_Loaded;
        }

        private async void PhamNhanPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            loadingOverlay.Visibility = Visibility.Visible;

            try
            {
                _allPhamNhan = await _apiService.GetPhamNhanAsync();
                dgPhamNhan.ItemsSource = _allPhamNhan;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                loadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = txtSearch.Text.ToLower();
            
            if (string.IsNullOrWhiteSpace(searchText))
            {
                dgPhamNhan.ItemsSource = _allPhamNhan;
            }
            else
            {
                dgPhamNhan.ItemsSource = _allPhamNhan.Where(p =>
                    p.HoTen.ToLower().Contains(searchText) ||
                    p.ToiDanh.ToLower().Contains(searchText) ||
                    (p.QueQuan?.ToLower().Contains(searchText) ?? false)
                ).ToList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new PhamNhanDialog(_apiService);
            if (dialog.ShowDialog() == true)
            {
                _ = LoadData();
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            _ = LoadData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var phamNhan = button?.DataContext as PhamNhan;
            
            if (phamNhan != null)
            {
                var dialog = new PhamNhanDialog(_apiService, phamNhan);
                if (dialog.ShowDialog() == true)
                {
                    _ = LoadData();
                }
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var phamNhan = button?.DataContext as PhamNhan;

            if (phamNhan != null)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa phạm nhân '{phamNhan.HoTen}'?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var success = await _apiService.DeletePhamNhanAsync(phamNhan.Id);
                    
                    if (success)
                    {
                        MessageBox.Show("Đã xóa thành công!", "Thông báo", 
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại!", "Lỗi", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
