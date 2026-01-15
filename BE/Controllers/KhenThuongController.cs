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
    public class KhenThuongController : ControllerBase
    {
        private readonly PrisonDbContext _context;

        public KhenThuongController(PrisonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhenThuongDTO>>> GetAll()
        {
            var items = await _context.KhenThuongs
                .Include(k => k.PhamNhan)
                .Select(k => new KhenThuongDTO
                {
                    Id = k.Id,
                    PhamNhanId = k.PhamNhanId,
                    NgayKhenThuong = k.NgayKhenThuong,
                    LyDo = k.LyDo,
                    HinhThuc = k.HinhThuc,
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
        public async Task<ActionResult<KhenThuongDTO>> Create([FromBody] CreateKhenThuongDTO dto)
        {
            var item = new KhenThuong
            {
                PhamNhanId = dto.PhamNhanId,
                NgayKhenThuong = dto.NgayKhenThuong,
                LyDo = dto.LyDo,
                HinhThuc = dto.HinhThuc,
                NguoiKy = dto.NguoiKy,
                GhiChu = dto.GhiChu
            };

            _context.KhenThuongs.Add(item);
            await _context.SaveChangesAsync();

            return Ok(new KhenThuongDTO
            {
                Id = item.Id,
                PhamNhanId = item.PhamNhanId,
                NgayKhenThuong = item.NgayKhenThuong,
                LyDo = item.LyDo,
                HinhThuc = item.HinhThuc,
                NguoiKy = item.NguoiKy,
                GhiChu = item.GhiChu
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,CanBo")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.KhenThuongs.FindAsync(id);
            if (item == null) return NotFound();

            _context.KhenThuongs.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
