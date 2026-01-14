# WPF Prison Management System

Ứng dụng Windows desktop quản lý trại giam được xây dựng bằng C# WPF.

## Yêu cầu

- .NET 8.0 SDK
- Visual Studio 2022 (khuyến nghị)
- Windows 10/11

## Cài đặt

### 1. Mở project trong Visual Studio

1. Mở Visual Studio 2022
2. Chọn **File > Open > Project/Solution**
3. Chọn file `PrisonManagement.csproj`

### 2. Cấu hình API URL

Mở file `Services/ApiService.cs` và thay đổi URL nếu cần:

```csharp
BaseAddress = new Uri("https://localhost:7000/api/")
```

### 3. Chạy ứng dụng

Nhấn **F5** hoặc **Ctrl+F5** để chạy.

## Tính năng

- ✅ Đăng nhập/đăng xuất
- ✅ Dashboard thống kê
- ✅ Quản lý phạm nhân (CRUD đầy đủ)
- ✅ Quản lý cán bộ
- ✅ Quản lý phòng giam
- ✅ Quản lý sức khỏe
- ✅ Quản lý thăm gặp
- ✅ Quản lý lao động
- ✅ Quản lý khen thưởng
- ✅ Quản lý kỷ luật
- ✅ Quản lý sự cố

## Cấu trúc dự án

```
PrisonManagement/
├── Models/              # Data models
├── Services/            # API service
├── Views/               # Windows và Pages
│   ├── LoginWindow.xaml
│   ├── MainWindow.xaml
│   └── Pages/           # Các trang chức năng
├── App.xaml
└── PrisonManagement.csproj
```

## Lưu ý

- Cần chạy C# Backend API trước khi sử dụng app
- Backend API mặc định chạy ở `https://localhost:7000`
- Tài khoản demo: `admin` / `admin123`

## Build Production

```bash
dotnet publish -c Release
```

Output sẽ nằm trong folder `bin/Release/net8.0-windows/publish/`
