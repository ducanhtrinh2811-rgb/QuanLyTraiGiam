using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisonManagement.Models
{
    [Table("PhongGiam")]
    public class PhongGiam
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string MaPhong { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TenPhong { get; set; } = string.Empty;

        public int SucChua { get; set; }

        public int SoLuongHienTai { get; set; } = 0;

        [StringLength(50)]
        public string? LoaiPhong { get; set; } // Thường, Đặc biệt, Cách ly

        [StringLength(20)]
        public string TrangThai { get; set; } = "HoatDong"; // HoatDong, BaoTri, DaKhoa

        // Navigation
        public virtual ICollection<PhamNhan> PhamNhans { get; set; } = new List<PhamNhan>();
    }
}
