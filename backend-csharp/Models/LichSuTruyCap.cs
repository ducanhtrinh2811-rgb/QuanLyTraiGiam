using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisonManagement.Models
{
    [Table("LichSuTruyCap")]
    public class LichSuTruyCap
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TaiKhoanId { get; set; }

        public DateTime ThoiGian { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? HanhDong { get; set; }

        [StringLength(500)]
        public string? ChiTiet { get; set; }

        [StringLength(50)]
        public string? DiaChi_IP { get; set; }

        // Navigation
        [ForeignKey("TaiKhoanId")]
        public virtual TaiKhoan? TaiKhoan { get; set; }
    }
}
