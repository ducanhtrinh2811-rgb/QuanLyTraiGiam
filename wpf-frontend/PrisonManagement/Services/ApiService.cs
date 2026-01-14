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
                BaseAddress = new Uri("https://localhost:7000/api/")
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

        // Auth
        public async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", 
                new LoginRequest { Username = username, Password = password });
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponse>();
            }
            return null;
        }

        // Phạm nhân
        public async Task<List<PhamNhan>> GetPhamNhanAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PhamNhan>>("phamnhan") ?? new List<PhamNhan>();
        }

        public async Task<PhamNhan?> GetPhamNhanByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<PhamNhan>($"phamnhan/{id}");
        }

        public async Task<bool> CreatePhamNhanAsync(PhamNhan phamNhan)
        {
            var response = await _httpClient.PostAsJsonAsync("phamnhan", phamNhan);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePhamNhanAsync(int id, PhamNhan phamNhan)
        {
            var response = await _httpClient.PutAsJsonAsync($"phamnhan/{id}", phamNhan);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePhamNhanAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"phamnhan/{id}");
            return response.IsSuccessStatusCode;
        }

        // Cán bộ
        public async Task<List<CanBo>> GetCanBoAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CanBo>>("canbo") ?? new List<CanBo>();
        }

        public async Task<bool> CreateCanBoAsync(CanBo canBo)
        {
            var response = await _httpClient.PostAsJsonAsync("canbo", canBo);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCanBoAsync(int id, CanBo canBo)
        {
            var response = await _httpClient.PutAsJsonAsync($"canbo/{id}", canBo);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCanBoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"canbo/{id}");
            return response.IsSuccessStatusCode;
        }

        // Phòng giam
        public async Task<List<PhongGiam>> GetPhongGiamAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PhongGiam>>("phonggiam") ?? new List<PhongGiam>();
        }

        // Sức khỏe
        public async Task<List<SucKhoe>> GetSucKhoeAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<SucKhoe>>("suckhoe") ?? new List<SucKhoe>();
        }

        public async Task<bool> CreateSucKhoeAsync(SucKhoe sucKhoe)
        {
            var response = await _httpClient.PostAsJsonAsync("suckhoe", sucKhoe);
            return response.IsSuccessStatusCode;
        }

        // Thăm gặp
        public async Task<List<ThamGap>> GetThamGapAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ThamGap>>("thamgap") ?? new List<ThamGap>();
        }

        public async Task<bool> CreateThamGapAsync(ThamGap thamGap)
        {
            var response = await _httpClient.PostAsJsonAsync("thamgap", thamGap);
            return response.IsSuccessStatusCode;
        }

        // Lao động
        public async Task<List<LaoDong>> GetLaoDongAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<LaoDong>>("laodong") ?? new List<LaoDong>();
        }

        public async Task<bool> CreateLaoDongAsync(LaoDong laoDong)
        {
            var response = await _httpClient.PostAsJsonAsync("laodong", laoDong);
            return response.IsSuccessStatusCode;
        }

        // Khen thưởng
        public async Task<List<KhenThuong>> GetKhenThuongAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<KhenThuong>>("khenthuong") ?? new List<KhenThuong>();
        }

        public async Task<bool> CreateKhenThuongAsync(KhenThuong khenThuong)
        {
            var response = await _httpClient.PostAsJsonAsync("khenthuong", khenThuong);
            return response.IsSuccessStatusCode;
        }

        // Kỷ luật
        public async Task<List<KyLuat>> GetKyLuatAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<KyLuat>>("kyluat") ?? new List<KyLuat>();
        }

        public async Task<bool> CreateKyLuatAsync(KyLuat kyLuat)
        {
            var response = await _httpClient.PostAsJsonAsync("kyluat", kyLuat);
            return response.IsSuccessStatusCode;
        }

        // Sự cố
        public async Task<List<SuCo>> GetSuCoAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<SuCo>>("suco") ?? new List<SuCo>();
        }

        public async Task<bool> CreateSuCoAsync(SuCo suCo)
        {
            var response = await _httpClient.PostAsJsonAsync("suco", suCo);
            return response.IsSuccessStatusCode;
        }

        // Thống kê
        public async Task<ThongKeResponse?> GetThongKeAsync()
        {
            return await _httpClient.GetFromJsonAsync<ThongKeResponse>("thongke");
        }
    }
}
