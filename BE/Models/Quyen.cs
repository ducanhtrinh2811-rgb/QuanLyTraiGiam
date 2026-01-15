using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrisonManagement.Models
{
    [Table("Quyen")]
    public class Quyen
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TenQuyen { get; set; } = string.Empty; // Admin, CanBo, GiamSat

        [StringLength(255)]
        public string? MoTa { get; set; }

        // Navigation
        public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
    }
}
