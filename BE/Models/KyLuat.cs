using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisonManagement.Models
{
    [Table("KyLuat")]
    public class KyLuat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PhamNhanId { get; set; }

        public DateTime NgayKyLuat { get; set; }

        [Required]
        [StringLength(500)]
        public string LyDo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string HinhThuc { get; set; } = string.Empty; // Cảnh cáo, Khiển trách, Biệt giam

        [StringLength(50)]
        public string? ThoiHan { get; set; }

        [Required]
        [StringLength(100)]
        public string NguoiKy { get; set; } = string.Empty;

        [StringLength(500)]
        public string? GhiChu { get; set; }

        // Navigation
        [ForeignKey("PhamNhanId")]
        public virtual PhamNhan? PhamNhan { get; set; }
    }
}
