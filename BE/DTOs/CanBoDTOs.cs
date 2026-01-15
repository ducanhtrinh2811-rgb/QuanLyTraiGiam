namespace PrisonManagement.DTOs
{
    public class CanBoDTO
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
    }

    public class CreateCanBoDTO
    {
        public string MaCanBo { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? ChucVu { get; set; }
        public string? PhongPhuTrach { get; set; }
        public string? SDT { get; set; }
        public string? DiaChi { get; set; }
    }

    public class UpdateCanBoDTO
    {
        public string? MaCanBo { get; set; }
        public string? HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? ChucVu { get; set; }
        public string? PhongPhuTrach { get; set; }
        public string? SDT { get; set; }
        public string? DiaChi { get; set; }
    }
}
