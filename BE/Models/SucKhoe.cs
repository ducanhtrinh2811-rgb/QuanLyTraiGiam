using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisonManagement.Models
{
    [Table("SucKhoe")]
    public class SucKhoe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PhamNhanId { get; set; }

        public DateTime NgayKham { get; set; }

        [StringLength(20)]
        public string LoaiKham { get; set; } = "DinhKy"; // DinhKy, DotXuat, NhapVien

        [StringLength(500)]
        public string? ChanDoan { get; set; }

        [StringLength(500)]
        public string? DieuTri { get; set; }

        [StringLength(100)]
        public string? BacSi { get; set; }

        [StringLength(500)]
        public string? GhiChu { get; set; }

        // Navigation
        [ForeignKey("PhamNhanId")]
        public virtual PhamNhan? PhamNhan { get; set; }
    }
}
