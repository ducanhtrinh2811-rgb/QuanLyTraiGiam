using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisonManagement.Models
{
    [Table("SuCo")]
    public class SuCo
    {
        [Key]
        public int Id { get; set; }

        public DateTime NgayXayRa { get; set; }

        [StringLength(20)]
        public string LoaiSuCo { get; set; } = "Khac"; // AnNinh, YTe, ChayNo, TronTrai, Khac

        [Required]
        [StringLength(1000)]
        public string MoTa { get; set; } = string.Empty;

        [StringLength(20)]
        public string MucDo { get; set; } = "Nhe"; // NghiemTrong, Vua, Nhe

        [StringLength(500)]
        public string? PhamNhanLienQuan { get; set; }

        [StringLength(1000)]
        public string? BienPhapXuLy { get; set; }

        [StringLength(100)]
        public string? NguoiBaoCao { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; } = "DangXuLy"; // DangXuLy, DaXuLy, TheoDoi

        [StringLength(500)]
        public string? GhiChu { get; set; }
    }
}
