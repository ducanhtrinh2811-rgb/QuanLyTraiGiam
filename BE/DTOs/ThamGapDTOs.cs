namespace PrisonManagement.DTOs
{
    public class ThamGapDTO
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public DateTime NgayThamGap { get; set; }
        public string NguoiThamGap { get; set; } = string.Empty;
        public string? QuanHe { get; set; }
        public string? CMND { get; set; }
        public string? ThoiGianBatDau { get; set; }
        public string? ThoiGianKetThuc { get; set; }
        public string? NoiDungTiepTe { get; set; }
        public string? GhiChu { get; set; }
        public PhamNhanSimpleDTO? PhamNhan { get; set; }
    }

    public class CreateThamGapDTO
    {
        public int PhamNhanId { get; set; }
        public DateTime NgayThamGap { get; set; }
        public string NguoiThamGap { get; set; } = string.Empty;
        public string? QuanHe { get; set; }
        public string? CMND { get; set; }
        public string? ThoiGianBatDau { get; set; }
        public string? ThoiGianKetThuc { get; set; }
        public string? NoiDungTiepTe { get; set; }
        public string? GhiChu { get; set; }
    }

    public class UpdateThamGapDTO
    {
        public DateTime? NgayThamGap { get; set; }
        public string? NguoiThamGap { get; set; }
        public string? QuanHe { get; set; }
        public string? CMND { get; set; }
        public string? ThoiGianBatDau { get; set; }
        public string? ThoiGianKetThuc { get; set; }
        public string? NoiDungTiepTe { get; set; }
        public string? GhiChu { get; set; }
    }
}
