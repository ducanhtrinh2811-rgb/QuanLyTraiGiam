using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using PrisonManagement.Models;

namespace PrisonManagement.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private string? _token;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/api/")
            };
        }

        public void SetToken(string token)
        {
            _token = token;
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }

        public void ClearToken()
        {
            _token = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        // Helper để kiểm tra response và xử lý lỗi 401
        private async Task<T> HandleResponse<T>(Task<HttpResponseMessage> responseTask)
        {
            var response = await responseTask;
            
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                HandleUnauthorized();
                throw new UnauthorizedAccessException("Phiên đăng nhập đã hết hạn");
            }
            
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>() 
                ?? throw new Exception("Không thể đọc dữ liệu từ server");
        }

        private void HandleUnauthorized()
        {
            ClearToken();
            
            System.Windows.Application.Current?.Dispatcher.Invoke(() =>
            {
                System.Windows.MessageBox.Show(
                    "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại!",
                    "Hết phiên",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);

                var loginWindow = new Views.LoginWindow();
                loginWindow.Show();
                System.Windows.Application.Current.MainWindow?.Close();
            });
        }

        // ==================== AUTH ====================
        public async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/login", 
                    new LoginRequest { Ten = username, MatKhau = password });
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LoginResponse>();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ==================== PHẠM NHÂN ====================
        public async Task<List<PhamNhan>> GetPhamNhanAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("phamnhan");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return new List<PhamNhan>();
                }
                
                return await response.Content.ReadFromJsonAsync<List<PhamNhan>>() 
                    ?? new List<PhamNhan>();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<PhamNhan>();
            }
        }

        public async Task<PhamNhan?> GetPhamNhanByIdAsync(int id)
        {
            try
            {
                return await HandleResponse<PhamNhan>(_httpClient.GetAsync($"phamnhan/{id}"));
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        public async Task<bool> CreatePhamNhanAsync(PhamNhan phamNhan)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("phamnhan", phamNhan);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> UpdatePhamNhanAsync(int id, PhamNhan phamNhan)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"phamnhan/{id}", phamNhan);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> DeletePhamNhanAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"phamnhan/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        // ==================== CÁN BỘ ====================
        public async Task<List<CanBo>> GetCanBoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("canbo");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return new List<CanBo>();
                }
                
                return await response.Content.ReadFromJsonAsync<List<CanBo>>() 
                    ?? new List<CanBo>();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<CanBo>();
            }
        }

        public async Task<CanBo?> GetCanBoByIdAsync(int id)
        {
            try
            {
                return await HandleResponse<CanBo>(_httpClient.GetAsync($"canbo/{id}"));
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        public async Task<bool> CreateCanBoAsync(CanBo canBo)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("canbo", canBo);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCanBoAsync(int id, CanBo canBo)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"canbo/{id}", canBo);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCanBoAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"canbo/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        // ==================== PHÒNG GIAM ====================
        public async Task<List<PhongGiam>> GetPhongGiamAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("phonggiam");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return new List<PhongGiam>();
                }
                
                return await response.Content.ReadFromJsonAsync<List<PhongGiam>>() 
                    ?? new List<PhongGiam>();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<PhongGiam>();
            }
        }

        public async Task<List<PhongGiam>> GetAllPhongGiamAsync()
        {
            return await GetPhongGiamAsync();
        }

        public async Task<PhongGiam?> GetPhongGiamByIdAsync(int id)
        {
            try
            {
                return await HandleResponse<PhongGiam>(_httpClient.GetAsync($"phonggiam/{id}"));
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        public async Task<bool> CreatePhongGiamAsync(PhongGiam phongGiam)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("phonggiam", phongGiam);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> UpdatePhongGiamAsync(int id, PhongGiam phongGiam)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"phonggiam/{id}", phongGiam);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> DeletePhongGiamAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"phonggiam/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        // ==================== SỨC KHỎE ====================
        public async Task<List<SucKhoe>> GetSucKhoeAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("suckhoe");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return new List<SucKhoe>();
                }
                
                return await response.Content.ReadFromJsonAsync<List<SucKhoe>>() 
                    ?? new List<SucKhoe>();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<SucKhoe>();
            }
        }

        public async Task<SucKhoe?> GetSucKhoeByIdAsync(int id)
        {
            try
            {
                return await HandleResponse<SucKhoe>(_httpClient.GetAsync($"suckhoe/{id}"));
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        public async Task<bool> CreateSucKhoeAsync(SucKhoe sucKhoe)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("suckhoe", sucKhoe);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateSucKhoeAsync(int id, SucKhoe sucKhoe)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"suckhoe/{id}", sucKhoe);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteSucKhoeAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"suckhoe/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        // ==================== THĂM GẶP ====================
        public async Task<List<ThamGap>> GetThamGapAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("thamgap");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return new List<ThamGap>();
                }
                
                return await response.Content.ReadFromJsonAsync<List<ThamGap>>() 
                    ?? new List<ThamGap>();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<ThamGap>();
            }
        }

        public async Task<ThamGap?> GetThamGapByIdAsync(int id)
        {
            try
            {
                return await HandleResponse<ThamGap>(_httpClient.GetAsync($"thamgap/{id}"));
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        public async Task<bool> CreateThamGapAsync(ThamGap thamGap)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("thamgap", thamGap);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateThamGapAsync(int id, ThamGap thamGap)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"thamgap/{id}", thamGap);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteThamGapAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"thamgap/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        // ==================== LAO ĐỘNG ====================
        public async Task<List<LaoDong>> GetLaoDongAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("laodong");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return new List<LaoDong>();
                }
                
                return await response.Content.ReadFromJsonAsync<List<LaoDong>>() 
                    ?? new List<LaoDong>();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<LaoDong>();
            }
        }

        public async Task<LaoDong?> GetLaoDongByIdAsync(int id)
        {
            try
            {
                return await HandleResponse<LaoDong>(_httpClient.GetAsync($"laodong/{id}"));
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        public async Task<bool> CreateLaoDongAsync(LaoDong laoDong)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("laodong", laoDong);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateLaoDongAsync(int id, LaoDong laoDong)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"laodong/{id}", laoDong);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteLaoDongAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"laodong/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        // ==================== KHEN THƯỞNG ====================
        public async Task<List<KhenThuong>> GetKhenThuongAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("khenthuong");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return new List<KhenThuong>();
                }
                
                return await response.Content.ReadFromJsonAsync<List<KhenThuong>>() 
                    ?? new List<KhenThuong>();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<KhenThuong>();
            }
        }

        public async Task<KhenThuong?> GetKhenThuongByIdAsync(int id)
        {
            try
            {
                return await HandleResponse<KhenThuong>(_httpClient.GetAsync($"khenthuong/{id}"));
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        public async Task<bool> CreateKhenThuongAsync(KhenThuong khenThuong)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("khenthuong", khenThuong);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateKhenThuongAsync(int id, KhenThuong khenThuong)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"khenthuong/{id}", khenThuong);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteKhenThuongAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"khenthuong/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        // ==================== KỶ LUẬT ====================
        public async Task<List<KyLuat>> GetKyLuatAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("kyluat");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return new List<KyLuat>();
                }
                
                return await response.Content.ReadFromJsonAsync<List<KyLuat>>() 
                    ?? new List<KyLuat>();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<KyLuat>();
            }
        }

        public async Task<KyLuat?> GetKyLuatByIdAsync(int id)
        {
            try
            {
                return await HandleResponse<KyLuat>(_httpClient.GetAsync($"kyluat/{id}"));
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        public async Task<bool> CreateKyLuatAsync(KyLuat kyLuat)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("kyluat", kyLuat);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateKyLuatAsync(int id, KyLuat kyLuat)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"kyluat/{id}", kyLuat);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteKyLuatAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"kyluat/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        // ==================== SỰ CỐ ====================
        public async Task<List<SuCo>> GetSuCoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("suco");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return new List<SuCo>();
                }
                
                return await response.Content.ReadFromJsonAsync<List<SuCo>>() 
                    ?? new List<SuCo>();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<SuCo>();
            }
        }

        public async Task<SuCo?> GetSuCoByIdAsync(int id)
        {
            try
            {
                return await HandleResponse<SuCo>(_httpClient.GetAsync($"suco/{id}"));
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        public async Task<bool> CreateSuCoAsync(SuCo suCo)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("suco", suCo);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateSuCoAsync(int id, SuCo suCo)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"suco/{id}", suCo);
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteSuCoAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"suco/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return false;
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        // ==================== THỐNG KÊ ====================
        public async Task<ThongKeResponse?> GetThongKeAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("thongke");
                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HandleUnauthorized();
                    return null;
                }
                
                return await response.Content.ReadFromJsonAsync<ThongKeResponse>();
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }
    }
}