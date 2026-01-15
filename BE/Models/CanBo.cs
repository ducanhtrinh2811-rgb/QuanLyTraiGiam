using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisonManagement.Models
{
    [Table("CanBo")]
    public class CanBo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string MaCanBo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        public DateTime? NgaySinh { get; set; }

        [StringLength(10)]
        public string? GioiTinh { get; set; } // Nam, Nữ

        [StringLength(100)]
        public string? ChucVu { get; set; }

        [StringLength(100)]
        public string? PhongPhuTrach { get; set; }

        [StringLength(20)]
        public string? SDT { get; set; }

        [StringLength(255)]
        public string? DiaChi { get; set; }

        public int? TaiKhoanId { get; set; }

        // Navigation
        [ForeignKey("TaiKhoanId")]
        public virtual TaiKhoan? TaiKhoan { get; set; }
    }
}
