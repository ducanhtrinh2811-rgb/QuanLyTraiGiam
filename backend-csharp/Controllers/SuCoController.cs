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
    public class SuCoController : ControllerBase
    {
        private readonly PrisonDbContext _context;

        public SuCoController(PrisonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SuCoDTO>>> GetAll()
        {
            var items = await _context.SuCos
                .Select(s => new SuCoDTO
                {
                    Id = s.Id,
                    NgayXayRa = s.NgayXayRa,
                    LoaiSuCo = s.LoaiSuCo,
                    MoTa = s.MoTa,
                    MucDo = s.MucDo,
                    PhamNhanLienQuan = s.PhamNhanLienQuan,
                    BienPhapXuLy = s.BienPhapXuLy,
                    NguoiBaoCao = s.NguoiBaoCao,
                    TrangThai = s.TrangThai,
                    GhiChu = s.GhiChu
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<SuCoDTO>> Create([FromBody] CreateSuCoDTO dto)
        {
            var item = new SuCo
            {
                NgayXayRa = dto.NgayXayRa,
                LoaiSuCo = dto.LoaiSuCo,
                MoTa = dto.MoTa,
                MucDo = dto.MucDo,
                PhamNhanLienQuan = dto.PhamNhanLienQuan,
                BienPhapXuLy = dto.BienPhapXuLy,
                NguoiBaoCao = dto.NguoiBaoCao,
                TrangThai = dto.TrangThai,
                GhiChu = dto.GhiChu
            };

            _context.SuCos.Add(item);
            await _context.SaveChangesAsync();

            return Ok(new SuCoDTO
            {
                Id = item.Id,
                NgayXayRa = item.NgayXayRa,
                LoaiSuCo = item.LoaiSuCo,
                MoTa = item.MoTa,
                MucDo = item.MucDo,
                PhamNhanLienQuan = item.PhamNhanLienQuan,
                BienPhapXuLy = item.BienPhapXuLy,
                NguoiBaoCao = item.NguoiBaoCao,
                TrangThai = item.TrangThai,
                GhiChu = item.GhiChu
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSuCoDTO dto)
        {
            var item = await _context.SuCos.FindAsync(id);
            if (item == null) return NotFound();

            if (dto.NgayXayRa.HasValue) item.NgayXayRa = dto.NgayXayRa.Value;
            if (dto.LoaiSuCo != null) item.LoaiSuCo = dto.LoaiSuCo;
            if (dto.MoTa != null) item.MoTa = dto.MoTa;
            if (dto.MucDo != null) item.MucDo = dto.MucDo;
            if (dto.PhamNhanLienQuan != null) item.PhamNhanLienQuan = dto.PhamNhanLienQuan;
            if (dto.BienPhapXuLy != null) item.BienPhapXuLy = dto.BienPhapXuLy;
            if (dto.NguoiBaoCao != null) item.NguoiBaoCao = dto.NguoiBaoCao;
            if (dto.TrangThai != null) item.TrangThai = dto.TrangThai;
            if (dto.GhiChu != null) item.GhiChu = dto.GhiChu;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.SuCos.FindAsync(id);
            if (item == null) return NotFound();

            _context.SuCos.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
