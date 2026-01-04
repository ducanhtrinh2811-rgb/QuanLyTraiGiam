using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisonManagement.Models
{
    [Table("PhamNhan")]
    public class PhamNhan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string MaPhamNhan { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        public DateTime? NgaySinh { get; set; }

        [StringLength(10)]
        public string? GioiTinh { get; set; } // Nam, Nữ

        [StringLength(255)]
        public string? QueQuan { get; set; }

        [StringLength(255)]
        public string? ToiDanh { get; set; }

        public DateTime NgayVaoTrai { get; set; }

        public DateTime? NgayRaTrai { get; set; }

        [Required]
        public int PhongGiamId { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; } = "DangGiam"; // DangGiam, DaRa, ChuyenTrai

        // Navigation
        [ForeignKey("PhongGiamId")]
        public virtual PhongGiam? PhongGiam { get; set; }

        public virtual ICollection<KhenThuong> KhenThuongs { get; set; } = new List<KhenThuong>();
        public virtual ICollection<KyLuat> KyLuats { get; set; } = new List<KyLuat>();
        public virtual ICollection<SucKhoe> SucKhoes { get; set; } = new List<SucKhoe>();
        public virtual ICollection<ThamGap> ThamGaps { get; set; } = new List<ThamGap>();
        public virtual ICollection<LaoDong> LaoDongs { get; set; } = new List<LaoDong>();
    }
}
