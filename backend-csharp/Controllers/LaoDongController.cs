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
    [Authorize(Roles = "Admin,CanBo")]
    public class LaoDongController : ControllerBase
    {
        private readonly PrisonDbContext _context;

        public LaoDongController(PrisonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaoDongDTO>>> GetAll()
        {
            var items = await _context.LaoDongs
                .Include(l => l.PhamNhan)
                .Select(l => new LaoDongDTO
                {
                    Id = l.Id,
                    PhamNhanId = l.PhamNhanId,
                    LoaiHoatDong = l.LoaiHoatDong,
                    TenHoatDong = l.TenHoatDong,
                    NgayBatDau = l.NgayBatDau,
                    NgayKetThuc = l.NgayKetThuc,
                    KetQua = l.KetQua,
                    DanhGia = l.DanhGia,
                    GhiChu = l.GhiChu,
                    PhamNhan = l.PhamNhan == null ? null : new PhamNhanSimpleDTO
                    {
                        Id = l.PhamNhan.Id,
                        MaPhamNhan = l.PhamNhan.MaPhamNhan,
                        HoTen = l.PhamNhan.HoTen
                    }
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<LaoDongDTO>> Create([FromBody] CreateLaoDongDTO dto)
        {
            var item = new LaoDong
            {
                PhamNhanId = dto.PhamNhanId,
                LoaiHoatDong = dto.LoaiHoatDong,
                TenHoatDong = dto.TenHoatDong,
                NgayBatDau = dto.NgayBatDau,
                NgayKetThuc = dto.NgayKetThuc,
                KetQua = dto.KetQua,
                DanhGia = dto.DanhGia,
                GhiChu = dto.GhiChu
            };

            _context.LaoDongs.Add(item);
            await _context.SaveChangesAsync();

            return Ok(new LaoDongDTO
            {
                Id = item.Id,
                PhamNhanId = item.PhamNhanId,
                LoaiHoatDong = item.LoaiHoatDong,
                TenHoatDong = item.TenHoatDong,
                NgayBatDau = item.NgayBatDau,
                NgayKetThuc = item.NgayKetThuc,
                KetQua = item.KetQua,
                DanhGia = item.DanhGia,
                GhiChu = item.GhiChu
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLaoDongDTO dto)
        {
            var item = await _context.LaoDongs.FindAsync(id);
            if (item == null) return NotFound();

            if (dto.LoaiHoatDong != null) item.LoaiHoatDong = dto.LoaiHoatDong;
            if (dto.TenHoatDong != null) item.TenHoatDong = dto.TenHoatDong;
            if (dto.NgayBatDau.HasValue) item.NgayBatDau = dto.NgayBatDau.Value;
            if (dto.NgayKetThuc.HasValue) item.NgayKetThuc = dto.NgayKetThuc;
            if (dto.KetQua != null) item.KetQua = dto.KetQua;
            if (dto.DanhGia != null) item.DanhGia = dto.DanhGia;
            if (dto.GhiChu != null) item.GhiChu = dto.GhiChu;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.LaoDongs.FindAsync(id);
            if (item == null) return NotFound();

            _context.LaoDongs.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
