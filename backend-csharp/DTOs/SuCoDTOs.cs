namespace PrisonManagement.DTOs
{
    public class SuCoDTO
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

    public class CreateSuCoDTO
    {
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

    public class UpdateSuCoDTO
    {
        public DateTime? NgayXayRa { get; set; }
        public string? LoaiSuCo { get; set; }
        public string? MoTa { get; set; }
        public string? MucDo { get; set; }
        public string? PhamNhanLienQuan { get; set; }
        public string? BienPhapXuLy { get; set; }
        public string? NguoiBaoCao { get; set; }
        public string? TrangThai { get; set; }
        public string? GhiChu { get; set; }
    }
}
