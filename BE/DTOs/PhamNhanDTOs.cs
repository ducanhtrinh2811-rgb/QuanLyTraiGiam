namespace PrisonManagement.DTOs
{
    public class PhamNhanDTO
    {
        public int Id { get; set; }
        public string MaPhamNhan { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? QueQuan { get; set; }
        public string? ToiDanh { get; set; }
        public DateTime NgayVaoTrai { get; set; }
        public DateTime? NgayRaTrai { get; set; }
        public int PhongGiamId { get; set; }
        public string TrangThai { get; set; } = "DangGiam";
        public PhongGiamDTO? PhongGiam { get; set; }
    }

    public class CreatePhamNhanDTO
    {
        public string MaPhamNhan { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? QueQuan { get; set; }
        public string? ToiDanh { get; set; }
        public DateTime NgayVaoTrai { get; set; }
        public int PhongGiamId { get; set; }
        public string TrangThai { get; set; } = "DangGiam";
    }

    public class UpdatePhamNhanDTO
    {
        public string? MaPhamNhan { get; set; }
        public string? HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? QueQuan { get; set; }
        public string? ToiDanh { get; set; }
        public DateTime? NgayVaoTrai { get; set; }
        public DateTime? NgayRaTrai { get; set; }
        public int? PhongGiamId { get; set; }
        public string? TrangThai { get; set; }
    }

    public class PhongGiamDTO
    {
        public int Id { get; set; }
        public string MaPhong { get; set; } = string.Empty;
        public string TenPhong { get; set; } = string.Empty;
        public int SucChua { get; set; }
        public int SoLuongHienTai { get; set; }
        public string? LoaiPhong { get; set; }
        public string TrangThai { get; set; } = "HoatDong";
    }
}
