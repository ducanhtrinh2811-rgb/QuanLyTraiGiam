using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrisonManagement.Data;
using PrisonManagement.DTOs;
using PrisonManagement.Models;

namespace PrisonManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CanBoController : ControllerBase
    {
        private readonly PrisonDbContext _context;

        public CanBoController(PrisonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CanBoDTO>>> GetAll()
        {
            var items = await _context.CanBos
                .Select(c => new CanBoDTO
                {
                    Id = c.Id,
                    MaCanBo = c.MaCanBo,
                    HoTen = c.HoTen,
                    NgaySinh = c.NgaySinh,
                    GioiTinh = c.GioiTinh,
                    ChucVu = c.ChucVu,
                    PhongPhuTrach = c.PhongPhuTrach,
                    SDT = c.SDT,
                    DiaChi = c.DiaChi
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CanBoDTO>> GetById(int id)
        {
            var item = await _context.CanBos.FindAsync(id);
            if (item == null) return NotFound();

            return Ok(new CanBoDTO
            {
                Id = item.Id,
                MaCanBo = item.MaCanBo,
                HoTen = item.HoTen,
                NgaySinh = item.NgaySinh,
                GioiTinh = item.GioiTinh,
                ChucVu = item.ChucVu,
                PhongPhuTrach = item.PhongPhuTrach,
                SDT = item.SDT,
                DiaChi = item.DiaChi
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,CanBo")]
        public async Task<ActionResult<CanBoDTO>> Create([FromBody] CreateCanBoDTO dto)
        {
            var item = new CanBo
            {
                MaCanBo = dto.MaCanBo,
                HoTen = dto.HoTen,
                NgaySinh = dto.NgaySinh,
                GioiTinh = dto.GioiTinh,
                ChucVu = dto.ChucVu,
                PhongPhuTrach = dto.PhongPhuTrach,
                SDT = dto.SDT,
                DiaChi = dto.DiaChi
            };

            _context.CanBos.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, new CanBoDTO
            {
                Id = item.Id,
                MaCanBo = item.MaCanBo,
                HoTen = item.HoTen,
                NgaySinh = item.NgaySinh,
                GioiTinh = item.GioiTinh,
                ChucVu = item.ChucVu,
                PhongPhuTrach = item.PhongPhuTrach,
                SDT = item.SDT,
                DiaChi = item.DiaChi
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,CanBo")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCanBoDTO dto)
        {
            var item = await _context.CanBos.FindAsync(id);
            if (item == null) return NotFound();

            if (dto.MaCanBo != null) item.MaCanBo = dto.MaCanBo;
            if (dto.HoTen != null) item.HoTen = dto.HoTen;
            if (dto.NgaySinh.HasValue) item.NgaySinh = dto.NgaySinh;
            if (dto.GioiTinh != null) item.GioiTinh = dto.GioiTinh;
            if (dto.ChucVu != null) item.ChucVu = dto.ChucVu;
            if (dto.PhongPhuTrach != null) item.PhongPhuTrach = dto.PhongPhuTrach;
            if (dto.SDT != null) item.SDT = dto.SDT;
            if (dto.DiaChi != null) item.DiaChi = dto.DiaChi;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.CanBos.FindAsync(id);
            if (item == null) return NotFound();

            _context.CanBos.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
