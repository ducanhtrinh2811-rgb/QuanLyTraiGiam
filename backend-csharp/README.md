# Prison Management API - C# ASP.NET Core

## Hướng dẫn cài đặt

### Yêu cầu
- .NET 8 SDK
- SQL Server (LocalDB hoặc SQL Server Express)
- Visual Studio 2022 hoặc VS Code

### Các bước cài đặt

1. **Copy toàn bộ folder `backend-csharp` sang máy tính**

2. **Mở project trong Visual Studio**
   - Mở file `PrisonManagement.csproj`

3. **Cấu hình Connection String**
   - Mở `appsettings.json`
   - Sửa `DefaultConnection` cho phù hợp với SQL Server của bạn:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=PrisonManagement;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

4. **Tạo Database Migration**
   ```bash
   # Trong Package Manager Console
   Add-Migration InitialCreate
   Update-Database
   ```

   Hoặc dùng CLI:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Chạy project**
   ```bash
   dotnet run
   ```
   
   API sẽ chạy tại: `https://localhost:7xxx`
   Swagger UI: `https://localhost:7xxx/swagger`

### Cấu trúc thư mục

```
backend-csharp/
├── Controllers/         # API Controllers
│   ├── AuthController.cs
│   ├── CanBoController.cs
│   ├── PhamNhanController.cs
│   ├── PhongGiamController.cs
│   ├── KhenThuongController.cs
│   ├── KyLuatController.cs
│   ├── SucKhoeController.cs
│   ├── ThamGapController.cs
│   ├── LaoDongController.cs
│   ├── SuCoController.cs
│   └── ThongKeController.cs
├── Data/
│   └── PrisonDbContext.cs   # Entity Framework DbContext
├── DTOs/                # Data Transfer Objects
│   ├── AuthDTOs.cs
│   ├── CanBoDTOs.cs
│   ├── PhamNhanDTOs.cs
│   └── ...
├── Models/              # Entity Models
│   ├── Quyen.cs
│   ├── TaiKhoan.cs
│   ├── CanBo.cs
│   ├── PhamNhan.cs
│   └── ...
├── Program.cs           # Application entry point
├── appsettings.json     # Configuration
└── PrisonManagement.csproj
```

### API Endpoints

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| POST | /api/auth/login | Đăng nhập |
| GET | /api/canbo | Danh sách cán bộ |
| POST | /api/canbo | Thêm cán bộ |
| PUT | /api/canbo/{id} | Cập nhật cán bộ |
| DELETE | /api/canbo/{id} | Xóa cán bộ |
| GET | /api/phamnhan | Danh sách phạm nhân |
| POST | /api/phamnhan | Thêm phạm nhân |
| PUT | /api/phamnhan/{id} | Cập nhật phạm nhân |
| DELETE | /api/phamnhan/{id} | Xóa phạm nhân |
| GET | /api/phonggiam | Danh sách phòng giam |
| GET | /api/phonggiam/available | Phòng còn chỗ |
| GET | /api/thongke/dashboard | Thống kê tổng hợp |
| GET | /api/thongke/phonggiam | Thống kê phòng giam |
| ... | ... | ... |

### Kết nối với React Frontend

Trong frontend React, cập nhật API base URL:

```typescript
// src/api/config.ts
export const API_BASE_URL = 'https://localhost:7xxx/api';
```

### Bảo mật

⚠️ **QUAN TRỌNG:**
- Thay đổi JWT Key trong `appsettings.json` trước khi deploy
- Sử dụng BCrypt để hash password (hiện tại đang dùng plain text để demo)
- Cấu hình CORS phù hợp với domain production

### Seed Data

Chạy script SQL sau để tạo dữ liệu mẫu:

```sql
-- Insert Quyen (đã có trong migration)
-- Insert sample accounts
INSERT INTO TaiKhoan (Ten, MatKhauHash, QuyenId, IsActive) VALUES
('admin', 'admin123', 1, 1),
('canbo1', 'canbo123', 2, 1),
('giamsat1', 'giamsat123', 3, 1);

-- Insert sample PhongGiam
INSERT INTO PhongGiam (MaPhong, TenPhong, SucChua, SoLuongHienTai, LoaiPhong, TrangThai) VALUES
('PG-A01', 'Phòng A01', 20, 15, N'Thường', 'HoatDong'),
('PG-A02', 'Phòng A02', 20, 18, N'Thường', 'HoatDong'),
('PG-B01', 'Phòng B01', 15, 12, N'Đặc biệt', 'HoatDong');
```
