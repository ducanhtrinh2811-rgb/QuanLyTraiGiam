using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisonManagement.Models
{
    [Table("LaoDong")]
    public class LaoDong
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PhamNhanId { get; set; }

        [StringLength(20)]
        public string LoaiHoatDong { get; set; } = "LaoDong"; // LaoDong, HocTap

        [Required]
        [StringLength(200)]
        public string TenHoatDong { get; set; } = string.Empty;

        public DateTime NgayBatDau { get; set; }

        public DateTime? NgayKetThuc { get; set; }

        [StringLength(255)]
        public string? KetQua { get; set; }

        [StringLength(20)]
        public string? DanhGia { get; set; } // Tot, Kha, TrungBinh, Yeu

        [StringLength(500)]
        public string? GhiChu { get; set; }

        // Navigation
        [ForeignKey("PhamNhanId")]
        public virtual PhamNhan? PhamNhan { get; set; }
    }
}
