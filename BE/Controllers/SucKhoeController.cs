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
    public class SucKhoeController : ControllerBase
    {
        private readonly PrisonDbContext _context;

        public SucKhoeController(PrisonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SucKhoeDTO>>> GetAll()
        {
            var items = await _context.SucKhoes
                .Include(s => s.PhamNhan)
                .Select(s => new SucKhoeDTO
                {
                    Id = s.Id,
                    PhamNhanId = s.PhamNhanId,
                    NgayKham = s.NgayKham,
                    LoaiKham = s.LoaiKham,
                    ChanDoan = s.ChanDoan,
                    DieuTri = s.DieuTri,
                    BacSi = s.BacSi,
                    GhiChu = s.GhiChu,
                    PhamNhan = s.PhamNhan == null ? null : new PhamNhanSimpleDTO
                    {
                        Id = s.PhamNhan.Id,
                        MaPhamNhan = s.PhamNhan.MaPhamNhan,
                        HoTen = s.PhamNhan.HoTen
                    }
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<SucKhoeDTO>> Create([FromBody] CreateSucKhoeDTO dto)
        {
            var item = new SucKhoe
            {
                PhamNhanId = dto.PhamNhanId,
                NgayKham = dto.NgayKham,
                LoaiKham = dto.LoaiKham,
                ChanDoan = dto.ChanDoan,
                DieuTri = dto.DieuTri,
                BacSi = dto.BacSi,
                GhiChu = dto.GhiChu
            };

            _context.SucKhoes.Add(item);
            await _context.SaveChangesAsync();

            return Ok(new SucKhoeDTO
            {
                Id = item.Id,
                PhamNhanId = item.PhamNhanId,
                NgayKham = item.NgayKham,
                LoaiKham = item.LoaiKham,
                ChanDoan = item.ChanDoan,
                DieuTri = item.DieuTri,
                BacSi = item.BacSi,
                GhiChu = item.GhiChu
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSucKhoeDTO dto)
        {
            var item = await _context.SucKhoes.FindAsync(id);
            if (item == null) return NotFound();

            if (dto.NgayKham.HasValue) item.NgayKham = dto.NgayKham.Value;
            if (dto.LoaiKham != null) item.LoaiKham = dto.LoaiKham;
            if (dto.ChanDoan != null) item.ChanDoan = dto.ChanDoan;
            if (dto.DieuTri != null) item.DieuTri = dto.DieuTri;
            if (dto.BacSi != null) item.BacSi = dto.BacSi;
            if (dto.GhiChu != null) item.GhiChu = dto.GhiChu;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.SucKhoes.FindAsync(id);
            if (item == null) return NotFound();

            _context.SucKhoes.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
