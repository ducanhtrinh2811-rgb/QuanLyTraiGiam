using System.Windows;
using PrisonManagement.Services;

namespace PrisonManagement.Views
{
    public partial class LoginWindow : Window
    {
        private readonly ApiService _apiService;

        public LoginWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
            txtUsername.Focus();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtError.Text = "Vui lòng nhập tên đăng nhập và mật khẩu";
                return;
            }

            btnLogin.IsEnabled = false;
            btnLogin.Content = "Đang đăng nhập...";
            txtError.Text = "";

            try
            {
                var result = await _apiService.LoginAsync(username, password);

                if (result != null)
                {
                    _apiService.SetToken(result.Token);
                    var mainWindow = new MainWindow(_apiService, result.HoTen, result.ChucVu);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    txtError.Text = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            catch (System.Exception ex)
            {
                txtError.Text = $"Lỗi kết nối: {ex.Message}";
            }
            finally
            {
                btnLogin.IsEnabled = true;
                btnLogin.Content = "Đăng nhập";
            }
        }
    }
}
