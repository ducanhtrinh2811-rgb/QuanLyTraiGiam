namespace PrisonManagement.DTOs
{
    public class KhenThuongDTO
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public DateTime NgayKhenThuong { get; set; }
        public string LyDo { get; set; } = string.Empty;
        public string HinhThuc { get; set; } = string.Empty;
        public string NguoiKy { get; set; } = string.Empty;
        public string? GhiChu { get; set; }
        public PhamNhanSimpleDTO? PhamNhan { get; set; }
    }

    public class CreateKhenThuongDTO
    {
        public int PhamNhanId { get; set; }
        public DateTime NgayKhenThuong { get; set; }
        public string LyDo { get; set; } = string.Empty;
        public string HinhThuc { get; set; } = string.Empty;
        public string NguoiKy { get; set; } = string.Empty;
        public string? GhiChu { get; set; }
    }

    public class KyLuatDTO
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public DateTime NgayKyLuat { get; set; }
        public string LyDo { get; set; } = string.Empty;
        public string HinhThuc { get; set; } = string.Empty;
        public string? ThoiHan { get; set; }
        public string NguoiKy { get; set; } = string.Empty;
        public string? GhiChu { get; set; }
        public PhamNhanSimpleDTO? PhamNhan { get; set; }
    }

    public class CreateKyLuatDTO
    {
        public int PhamNhanId { get; set; }
        public DateTime NgayKyLuat { get; set; }
        public string LyDo { get; set; } = string.Empty;
        public string HinhThuc { get; set; } = string.Empty;
        public string? ThoiHan { get; set; }
        public string NguoiKy { get; set; } = string.Empty;
        public string? GhiChu { get; set; }
    }

    public class PhamNhanSimpleDTO
    {
        public int Id { get; set; }
        public string MaPhamNhan { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
    }
}
