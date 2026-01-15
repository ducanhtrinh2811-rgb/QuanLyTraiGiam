using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrisonManagement.Data;
using PrisonManagement.DTOs;

namespace PrisonManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ThongKeController : ControllerBase
    {
        private readonly PrisonDbContext _context;

        public ThongKeController(PrisonDbContext context)
        {
            _context = context;
        }

        [HttpGet("phonggiam")]
        public async Task<ActionResult<IEnumerable<ThongKePhongGiamDTO>>> GetPhongGiamStats()
        {
            var stats = await _context.PhongGiams
                .Select(p => new ThongKePhongGiamDTO
                {
                    MaPhong = p.MaPhong,
                    TenPhong = p.TenPhong,
                    SucChua = p.SucChua,
                    SoLuongHienTai = p.SoLuongHienTai,
                    TyLeSuDung = p.SucChua > 0 
                        ? (int)Math.Round((double)p.SoLuongHienTai / p.SucChua * 100) 
                        : 0,
                    TrangThai = p.TrangThai == "HoatDong" ? "Hoạt động" :
                               p.TrangThai == "BaoTri" ? "Bảo trì" : "Đã khóa"
                })
                .ToListAsync();

            return Ok(stats);
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardStatsDTO>> GetDashboardStats()
        {
            var stats = new DashboardStatsDTO
            {
                TongPhamNhan = await _context.PhamNhans.CountAsync(p => p.TrangThai == "DangGiam"),
                TongCanBo = await _context.CanBos.CountAsync(),
                TongPhongGiam = await _context.PhongGiams.CountAsync(p => p.TrangThai == "HoatDong"),
                TongKhenThuong = await _context.KhenThuongs.CountAsync(),
                TongKyLuat = await _context.KyLuats.CountAsync()
            };

            return Ok(stats);
        }
    }
}
