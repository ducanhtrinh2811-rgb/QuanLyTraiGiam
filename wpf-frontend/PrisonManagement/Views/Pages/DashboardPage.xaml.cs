using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class DashboardPage : Page
    {
        private readonly ApiService _apiService;

        public DashboardPage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            Loaded += DashboardPage_Loaded;
        }

        private async void DashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var thongKe = await _apiService.GetThongKeAsync();
                
                if (thongKe != null)
                {
                    txtTongPhamNhan.Text = thongKe.TongPhamNhan.ToString();
                    txtTongCanBo.Text = thongKe.TongCanBo.ToString();
                    txtTongPhongGiam.Text = thongKe.TongPhongGiam.ToString();
                    txtSuCoThang.Text = thongKe.SuCoTrongThang.ToString();
                }

                txtLoading.Visibility = Visibility.Collapsed;
            }
            catch (System.Exception ex)
            {
                txtLoading.Text = $"Lỗi tải dữ liệu: {ex.Message}";
            }
        }
    }
}
