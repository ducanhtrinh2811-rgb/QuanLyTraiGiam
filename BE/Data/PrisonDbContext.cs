using Microsoft.EntityFrameworkCore;
using PrisonManagement.Models;

namespace PrisonManagement.Data
{
    public class PrisonDbContext : DbContext
    {
        public PrisonDbContext(DbContextOptions<PrisonDbContext> options) : base(options)
        {
        }

        public DbSet<Quyen> Quyens { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<CanBo> CanBos { get; set; }
        public DbSet<PhongGiam> PhongGiams { get; set; }
        public DbSet<PhamNhan> PhamNhans { get; set; }
        public DbSet<KhenThuong> KhenThuongs { get; set; }
        public DbSet<KyLuat> KyLuats { get; set; }
        public DbSet<SucKhoe> SucKhoes { get; set; }
        public DbSet<ThamGap> ThamGaps { get; set; }
        public DbSet<LaoDong> LaoDongs { get; set; }
        public DbSet<SuCo> SuCos { get; set; }
        public DbSet<LichSuTruyCap> LichSuTruyCaps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique constraints
            modelBuilder.Entity<TaiKhoan>()
                .HasIndex(t => t.Ten)
                .IsUnique();

            modelBuilder.Entity<CanBo>()
                .HasIndex(c => c.MaCanBo)
                .IsUnique();

            modelBuilder.Entity<PhamNhan>()
                .HasIndex(p => p.MaPhamNhan)
                .IsUnique();

            modelBuilder.Entity<PhongGiam>()
                .HasIndex(p => p.MaPhong)
                .IsUnique();

            // Relationships
            modelBuilder.Entity<TaiKhoan>()
                .HasOne(t => t.Quyen)
                .WithMany(q => q.TaiKhoans)
                .HasForeignKey(t => t.QuyenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CanBo>()
                .HasOne(c => c.TaiKhoan)
                .WithOne(t => t.CanBo)
                .HasForeignKey<CanBo>(c => c.TaiKhoanId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PhamNhan>()
                .HasOne(p => p.PhongGiam)
                .WithMany(pg => pg.PhamNhans)
                .HasForeignKey(p => p.PhongGiamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            modelBuilder.Entity<Quyen>().HasData(
                new Quyen { Id = 1, TenQuyen = "Admin", MoTa = "Quản trị viên hệ thống" },
                new Quyen { Id = 2, TenQuyen = "CanBo", MoTa = "Cán bộ quản lý" },
                new Quyen { Id = 3, TenQuyen = "GiamSat", MoTa = "Giám sát viên" }
            );

            // Seed Admin Account
            modelBuilder.Entity<TaiKhoan>().HasData(
                new TaiKhoan 
                { 
                    Id = 1, 
                    Ten = "admin", 
                    MatKhauHash = "admin123",  // TODO: Use BCrypt in production
                    QuyenId = 1, 
                    IsActive = true
                }
            );

            // Seed PhongGiam (Prison Rooms)
            modelBuilder.Entity<PhongGiam>().HasData(
                new PhongGiam 
                { 
                    Id = 1, 
                    MaPhong = "A1", 
                    TenPhong = "Phòng A1 - Tầng 1", 
                    SucChua = 10, 
                    SoLuongHienTai = 0,
                    LoaiPhong = "Bình thường",
                    TrangThai = "HoatDong"
                },
                new PhongGiam 
                { 
                    Id = 2, 
                    MaPhong = "A2", 
                    TenPhong = "Phòng A2 - Tầng 1", 
                    SucChua = 10, 
                    SoLuongHienTai = 0,
                    LoaiPhong = "Bình thường",
                    TrangThai = "HoatDong"
                },
                new PhongGiam 
                { 
                    Id = 3, 
                    MaPhong = "B1", 
                    TenPhong = "Phòng B1 - Tầng 2", 
                    SucChua = 8, 
                    SoLuongHienTai = 0,
                    LoaiPhong = "Đặc biệt",
                    TrangThai = "HoatDong"
                }
            );
        }
    }
}
