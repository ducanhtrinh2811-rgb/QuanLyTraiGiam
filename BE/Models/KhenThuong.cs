using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisonManagement.Models
{
    [Table("KhenThuong")]
    public class KhenThuong
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PhamNhanId { get; set; }

        public DateTime NgayKhenThuong { get; set; }

        [Required]
        [StringLength(500)]
        public string LyDo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string HinhThuc { get; set; } = string.Empty; // Giấy khen, Bằng khen, Giảm án

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
