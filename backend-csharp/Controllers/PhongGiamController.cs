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
    public class PhongGiamController : ControllerBase
    {
        private readonly PrisonDbContext _context;

        public PhongGiamController(PrisonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhongGiamDTO>>> GetAll()
        {
            var items = await _context.PhongGiams
                .Select(p => new PhongGiamDTO
                {
                    Id = p.Id,
                    MaPhong = p.MaPhong,
                    TenPhong = p.TenPhong,
                    SucChua = p.SucChua,
                    SoLuongHienTai = p.SoLuongHienTai,
                    LoaiPhong = p.LoaiPhong,
                    TrangThai = p.TrangThai
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<PhongGiamDTO>>> GetAvailable()
        {
            var items = await _context.PhongGiams
                .Where(p => p.TrangThai == "HoatDong" && p.SoLuongHienTai < p.SucChua)
                .Select(p => new PhongGiamDTO
                {
                    Id = p.Id,
                    MaPhong = p.MaPhong,
                    TenPhong = p.TenPhong,
                    SucChua = p.SucChua,
                    SoLuongHienTai = p.SoLuongHienTai,
                    LoaiPhong = p.LoaiPhong,
                    TrangThai = p.TrangThai
                })
                .ToListAsync();

            return Ok(items);
        }
    }
}
