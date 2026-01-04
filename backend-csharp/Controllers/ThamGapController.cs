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
    public class ThamGapController : ControllerBase
    {
        private readonly PrisonDbContext _context;

        public ThamGapController(PrisonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThamGapDTO>>> GetAll()
        {
            var items = await _context.ThamGaps
                .Include(t => t.PhamNhan)
                .Select(t => new ThamGapDTO
                {
                    Id = t.Id,
                    PhamNhanId = t.PhamNhanId,
                    NgayThamGap = t.NgayThamGap,
                    NguoiThamGap = t.NguoiThamGap,
                    QuanHe = t.QuanHe,
                    CMND = t.CMND,
                    ThoiGianBatDau = t.ThoiGianBatDau.HasValue ? t.ThoiGianBatDau.Value.ToString(@"hh\:mm") : null,
                    ThoiGianKetThuc = t.ThoiGianKetThuc.HasValue ? t.ThoiGianKetThuc.Value.ToString(@"hh\:mm") : null,
                    NoiDungTiepTe = t.NoiDungTiepTe,
                    GhiChu = t.GhiChu,
                    PhamNhan = t.PhamNhan == null ? null : new PhamNhanSimpleDTO
                    {
                        Id = t.PhamNhan.Id,
                        MaPhamNhan = t.PhamNhan.MaPhamNhan,
                        HoTen = t.PhamNhan.HoTen
                    }
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<ThamGapDTO>> Create([FromBody] CreateThamGapDTO dto)
        {
            var item = new ThamGap
            {
                PhamNhanId = dto.PhamNhanId,
                NgayThamGap = dto.NgayThamGap,
                NguoiThamGap = dto.NguoiThamGap,
                QuanHe = dto.QuanHe,
                CMND = dto.CMND,
                ThoiGianBatDau = string.IsNullOrEmpty(dto.ThoiGianBatDau) ? null : TimeSpan.Parse(dto.ThoiGianBatDau),
                ThoiGianKetThuc = string.IsNullOrEmpty(dto.ThoiGianKetThuc) ? null : TimeSpan.Parse(dto.ThoiGianKetThuc),
                NoiDungTiepTe = dto.NoiDungTiepTe,
                GhiChu = dto.GhiChu
            };

            _context.ThamGaps.Add(item);
            await _context.SaveChangesAsync();

            return Ok(new ThamGapDTO
            {
                Id = item.Id,
                PhamNhanId = item.PhamNhanId,
                NgayThamGap = item.NgayThamGap,
                NguoiThamGap = item.NguoiThamGap,
                QuanHe = item.QuanHe,
                CMND = item.CMND,
                ThoiGianBatDau = item.ThoiGianBatDau?.ToString(@"hh\:mm"),
                ThoiGianKetThuc = item.ThoiGianKetThuc?.ToString(@"hh\:mm"),
                NoiDungTiepTe = item.NoiDungTiepTe,
                GhiChu = item.GhiChu
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateThamGapDTO dto)
        {
            var item = await _context.ThamGaps.FindAsync(id);
            if (item == null) return NotFound();

            if (dto.NgayThamGap.HasValue) item.NgayThamGap = dto.NgayThamGap.Value;
            if (dto.NguoiThamGap != null) item.NguoiThamGap = dto.NguoiThamGap;
            if (dto.QuanHe != null) item.QuanHe = dto.QuanHe;
            if (dto.CMND != null) item.CMND = dto.CMND;
            if (dto.ThoiGianBatDau != null) item.ThoiGianBatDau = TimeSpan.Parse(dto.ThoiGianBatDau);
            if (dto.ThoiGianKetThuc != null) item.ThoiGianKetThuc = TimeSpan.Parse(dto.ThoiGianKetThuc);
            if (dto.NoiDungTiepTe != null) item.NoiDungTiepTe = dto.NoiDungTiepTe;
            if (dto.GhiChu != null) item.GhiChu = dto.GhiChu;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.ThamGaps.FindAsync(id);
            if (item == null) return NotFound();

            _context.ThamGaps.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
