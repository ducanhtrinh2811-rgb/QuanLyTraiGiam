using System;

namespace PrisonManagement.Models
{
    // Phạm nhân - đã có trong PhamNhan.cs
 public class PhamNhan
    {
        public int Id { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; } = string.Empty;
        public string QueQuan { get; set; } = string.Empty;
        public string ToiDanh { get; set; } = string.Empty;
        public DateTime NgayVaoTrai { get; set; }
        public DateTime? NgayRaTrai { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public int? PhongGiamId { get; set; }
        public PhongGiam? PhongGiam { get; set; }
        public string? GhiChu { get; set; }
    }

    // Cán bộ
    public class CanBo
    {
        public int Id { get; set; }
        public string MaCanBo { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? ChucVu { get; set; }
        public string? PhongPhuTrach { get; set; }
        public string? SDT { get; set; }
        public string? DiaChi { get; set; }
        public string TrangThai { get; set; } = "HoatDong";
    }

    // Phòng giam
    public class PhongGiam
    {
        public int Id { get; set; }
        public string MaPhong { get; set; } = string.Empty;
        public string TenPhong { get; set; } = string.Empty;
        public int SucChua { get; set; }
        public int SoLuongHienTai { get; set; }
        public string? LoaiPhong { get; set; }
        public string TrangThai { get; set; } = "HoatDong";
    }

    // Sức khỏe
    public class SucKhoe
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public DateTime NgayKham { get; set; }
        public string LoaiKham { get; set; } = "DinhKy";
        public string? ChanDoan { get; set; }
        public string? DieuTri { get; set; }
        public string? BacSi { get; set; }
        public string? GhiChu { get; set; }
        public PhamNhanSimple? PhamNhan { get; set; }
    }

    public class PhamNhanSimple
    {
        public int Id { get; set; }
        public string MaPhamNhan { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
    }

    // Thăm gặp
    public class ThamGap
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public DateTime NgayThamGap { get; set; }
        public string NguoiThamGap { get; set; } = string.Empty;
        public string? QuanHe { get; set; }
        public string? CMND { get; set; }
        public TimeSpan? ThoiGianBatDau { get; set; }
        public TimeSpan? ThoiGianKetThuc { get; set; }
        public string? NoiDungTiepTe { get; set; }
        public string? GhiChu { get; set; }
        public PhamNhanSimple? PhamNhan { get; set; }
    }

    // Lao động
    public class LaoDong
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public string LoaiHoatDong { get; set; } = "LaoDong";
        public string TenHoatDong { get; set; } = string.Empty;
        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string? KetQua { get; set; }
        public string? DanhGia { get; set; }
        public string? GhiChu { get; set; }
        public PhamNhanSimple? PhamNhan { get; set; }
    }

    // Khen thưởng
    public class KhenThuong
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public DateTime NgayKhenThuong { get; set; }
        public string LyDo { get; set; } = string.Empty;
        public string HinhThuc { get; set; } = string.Empty;
        public string NguoiKy { get; set; } = string.Empty;
        public string? GhiChu { get; set; }
        public PhamNhanSimple? PhamNhan { get; set; }
    }

    // Kỷ luật
    public class KyLuat
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public DateTime NgayKyLuat { get; set; }
        public string LyDo { get; set; } = string.Empty;
        public string HinhThuc { get; set; } = string.Empty;
        public string? ThoiHan { get; set; }
        public string NguoiKy { get; set; } = string.Empty;
        public string? GhiChu { get; set; }
        public PhamNhanSimple? PhamNhan { get; set; }
    }

    // Sự cố
    public class SuCo
    {
        public int Id { get; set; }
        public DateTime NgayXayRa { get; set; }
        public string LoaiSuCo { get; set; } = "Khac";
        public string MoTa { get; set; } = string.Empty;
        public string MucDo { get; set; } = "Nhe";
        public string? PhamNhanLienQuan { get; set; }
        public string? BienPhapXuLy { get; set; }
        public string? NguoiBaoCao { get; set; }
        public string TrangThai { get; set; } = "DangXuLy";
        public string? GhiChu { get; set; }
    }

    // Login models
    public class LoginRequest
    {
        public string Ten { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    // Thống kê
    public class ThongKeResponse
    {
        public int TongPhamNhan { get; set; }
        public int TongCanBo { get; set; }
        public int TongPhongGiam { get; set; }
        public int SuCoThang { get; set; }
    }
}