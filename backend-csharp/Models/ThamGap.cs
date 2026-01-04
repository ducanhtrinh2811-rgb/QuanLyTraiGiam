using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisonManagement.Models
{
    [Table("ThamGap")]
    public class ThamGap
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PhamNhanId { get; set; }

        public DateTime NgayThamGap { get; set; }

        [Required]
        [StringLength(100)]
        public string NguoiThamGap { get; set; } = string.Empty;

        [StringLength(50)]
        public string? QuanHe { get; set; }

        [StringLength(20)]
        public string? CMND { get; set; }

        public TimeSpan? ThoiGianBatDau { get; set; }

        public TimeSpan? ThoiGianKetThuc { get; set; }

        [StringLength(500)]
        public string? NoiDungTiepTe { get; set; }

        [StringLength(500)]
        public string? GhiChu { get; set; }

        // Navigation
        [ForeignKey("PhamNhanId")]
        public virtual PhamNhan? PhamNhan { get; set; }
    }
}
