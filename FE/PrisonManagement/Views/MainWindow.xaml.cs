using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Services;
using PrisonManagement.Views.Pages;

namespace PrisonManagement.Views
{
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService;
        private readonly string _hoTen;
        private readonly string _chucVu;

        public MainWindow(ApiService apiService, string hoTen, string chucVu)
        {
            InitializeComponent();
            _apiService = apiService;
            _hoTen = hoTen;
            _chucVu = chucVu;

            txtUserInfo.Text = $"👤 {hoTen}\n📋 {chucVu}";
            
            // Load PhamNhan by default
            MainFrame.Navigate(new PhamNhanPage(_apiService));
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tag = button?.Tag?.ToString();

            switch (tag)
            {
                case "PhamNhan":
                    MainFrame.Navigate(new PhamNhanPage(_apiService));
                    break;
                case "CanBo":
                    MainFrame.Navigate(new CanBoPage(_apiService));
                    break;
                case "PhongGiam":
                    MainFrame.Navigate(new PhongGiamPage(_apiService));
                    break;
                case "SucKhoe":
                    MainFrame.Navigate(new SucKhoePage(_apiService));
                    break;
                case "ThamGap":
                    MainFrame.Navigate(new ThamGapPage(_apiService));
                    break;
                case "LaoDong":
                    MainFrame.Navigate(new LaoDongPage(_apiService));
                    break;
                case "KhenThuong":
                    MainFrame.Navigate(new KhenThuongPage(_apiService));
                    break;
                case "KyLuat":
                    MainFrame.Navigate(new KyLuatPage(_apiService));
                    break;
                case "SuCo":
                    MainFrame.Navigate(new SuCoPage(_apiService));
                    break;
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", 
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                _apiService.ClearToken();
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}