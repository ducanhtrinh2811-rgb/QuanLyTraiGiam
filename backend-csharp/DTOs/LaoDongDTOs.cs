namespace PrisonManagement.DTOs
{
    public class LaoDongDTO
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
        public PhamNhanSimpleDTO? PhamNhan { get; set; }
    }

    public class CreateLaoDongDTO
    {
        public int PhamNhanId { get; set; }
        public string LoaiHoatDong { get; set; } = "LaoDong";
        public string TenHoatDong { get; set; } = string.Empty;
        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string? KetQua { get; set; }
        public string? DanhGia { get; set; }
        public string? GhiChu { get; set; }
    }

    public class UpdateLaoDongDTO
    {
        public string? LoaiHoatDong { get; set; }
        public string? TenHoatDong { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string? KetQua { get; set; }
        public string? DanhGia { get; set; }
        public string? GhiChu { get; set; }
    }
}
