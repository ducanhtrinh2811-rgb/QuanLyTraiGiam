using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisonManagement.Models
{
    [Table("TaiKhoan")]
    public class TaiKhoan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Ten { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string MatKhauHash { get; set; } = string.Empty;

        [Required]
        public int QuyenId { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation
        [ForeignKey("QuyenId")]
        public virtual Quyen? Quyen { get; set; }

        public virtual CanBo? CanBo { get; set; }
    }
}
