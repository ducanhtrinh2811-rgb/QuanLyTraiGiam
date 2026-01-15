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
    public class KyLuatController : ControllerBase
    {
        private readonly PrisonDbContext _context;

        public KyLuatController(PrisonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KyLuatDTO>>> GetAll()
        {
            var items = await _context.KyLuats
                .Include(k => k.PhamNhan)
                .Select(k => new KyLuatDTO
                {
                    Id = k.Id,
                    PhamNhanId = k.PhamNhanId,
                    NgayKyLuat = k.NgayKyLuat,
                    LyDo = k.LyDo,
                    HinhThuc = k.HinhThuc,
                    ThoiHan = k.ThoiHan,
                    NguoiKy = k.NguoiKy,
                    GhiChu = k.GhiChu,
                    PhamNhan = k.PhamNhan == null ? null : new PhamNhanSimpleDTO
                    {
                        Id = k.PhamNhan.Id,
                        MaPhamNhan = k.PhamNhan.MaPhamNhan,
                        HoTen = k.PhamNhan.HoTen
                    }
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,CanBo")]
        public async Task<ActionResult<KyLuatDTO>> Create([FromBody] CreateKyLuatDTO dto)
        {
            var item = new KyLuat
            {
                PhamNhanId = dto.PhamNhanId,
                NgayKyLuat = dto.NgayKyLuat,
                LyDo = dto.LyDo,
                HinhThuc = dto.HinhThuc,
                ThoiHan = dto.ThoiHan,
                NguoiKy = dto.NguoiKy,
                GhiChu = dto.GhiChu
            };

            _context.KyLuats.Add(item);
            await _context.SaveChangesAsync();

            return Ok(new KyLuatDTO
            {
                Id = item.Id,
                PhamNhanId = item.PhamNhanId,
                NgayKyLuat = item.NgayKyLuat,
                LyDo = item.LyDo,
                HinhThuc = item.HinhThuc,
                ThoiHan = item.ThoiHan,
                NguoiKy = item.NguoiKy,
                GhiChu = item.GhiChu
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,CanBo")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.KyLuats.FindAsync(id);
            if (item == null) return NotFound();

            _context.KyLuats.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
