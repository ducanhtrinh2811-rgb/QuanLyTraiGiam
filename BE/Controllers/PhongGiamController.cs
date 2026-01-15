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

        [HttpPost]
        public async Task<ActionResult<PhongGiamDTO>> Create(PhongGiamDTO dto)
        {
            var phongGiam = new Models.PhongGiam
            {
                MaPhong = dto.MaPhong,
                TenPhong = dto.TenPhong,
                SucChua = dto.SucChua,
                SoLuongHienTai = 0,
                LoaiPhong = dto.LoaiPhong,
                TrangThai = dto.TrangThai ?? "HoatDong"
            };

            _context.PhongGiams.Add(phongGiam);
            await _context.SaveChangesAsync();

            dto.Id = phongGiam.Id;
            return CreatedAtAction(nameof(GetAll), new { id = phongGiam.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PhongGiamDTO dto)
        {
            var phongGiam = await _context.PhongGiams.FindAsync(id);
            if (phongGiam == null)
                return NotFound();

            phongGiam.MaPhong = dto.MaPhong;
            phongGiam.TenPhong = dto.TenPhong;
            phongGiam.SucChua = dto.SucChua;
            phongGiam.LoaiPhong = dto.LoaiPhong;
            phongGiam.TrangThai = dto.TrangThai ?? "HoatDong";

            _context.PhongGiams.Update(phongGiam);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var phongGiam = await _context.PhongGiams.FindAsync(id);
            if (phongGiam == null)
                return NotFound();

            _context.PhongGiams.Remove(phongGiam);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
