using System;

namespace PrisonManagement.Models
{
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
        public string MucDoNguyHiem { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty;
        public int? PhongGiamId { get; set; }
        public string? PhongGiamTen { get; set; }
        public string? GhiChu { get; set; }
    }

    public class CanBo
    {
        public int Id { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string ChucVu { get; set; } = string.Empty;
        public string PhongBan { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime NgayVaoLam { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }

    public class PhongGiam
    {
        public int Id { get; set; }
        public string TenPhong { get; set; } = string.Empty;
        public string Khu { get; set; } = string.Empty;
        public int SucChua { get; set; }
        public int SoLuongHienTai { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }

    public class SucKhoe
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public string PhamNhanTen { get; set; } = string.Empty;
        public DateTime NgayKham { get; set; }
        public string TinhTrangSucKhoe { get; set; } = string.Empty;
        public string ChanDoan { get; set; } = string.Empty;
        public string? DieuTri { get; set; }
        public string? GhiChu { get; set; }
    }

    public class ThamGap
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public string PhamNhanTen { get; set; } = string.Empty;
        public string NguoiThamGap { get; set; } = string.Empty;
        public string QuanHe { get; set; } = string.Empty;
        public DateTime NgayThamGap { get; set; }
        public string ThoiGian { get; set; } = string.Empty;
        public string? GhiChu { get; set; }
    }

    public class LaoDong
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public string PhamNhanTen { get; set; } = string.Empty;
        public string CongViec { get; set; } = string.Empty;
        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public string? KetQuaDanhGia { get; set; }
    }

    public class KhenThuong
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public string PhamNhanTen { get; set; } = string.Empty;
        public DateTime NgayKhenThuong { get; set; }
        public string LoaiKhenThuong { get; set; } = string.Empty;
        public string LyDo { get; set; } = string.Empty;
        public int? SoNgayGiam { get; set; }
    }

    public class KyLuat
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public string PhamNhanTen { get; set; } = string.Empty;
        public DateTime NgayViPham { get; set; }
        public string LoaiViPham { get; set; } = string.Empty;
        public string HinhThucXuLy { get; set; } = string.Empty;
        public string? GhiChu { get; set; }
    }

    public class SuCo
    {
        public int Id { get; set; }
        public DateTime NgayGio { get; set; }
        public string LoaiSuCo { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public string MucDoNghiemTrong { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty;
        public string? BiенPháp { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string ChucVu { get; set; } = string.Empty;
    }

    public class ThongKeResponse
    {
        public int TongPhamNhan { get; set; }
        public int TongCanBo { get; set; }
        public int TongPhongGiam { get; set; }
        public int SuCoTrongThang { get; set; }
    }
}
