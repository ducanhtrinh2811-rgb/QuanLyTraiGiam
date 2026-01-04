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
    public class PhamNhanController : ControllerBase
    {
        private readonly PrisonDbContext _context;

        public PhamNhanController(PrisonDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhamNhanDTO>>> GetAll()
        {
            var items = await _context.PhamNhans
                .Include(p => p.PhongGiam)
                .Select(p => new PhamNhanDTO
                {
                    Id = p.Id,
                    MaPhamNhan = p.MaPhamNhan,
                    HoTen = p.HoTen,
                    NgaySinh = p.NgaySinh,
                    GioiTinh = p.GioiTinh,
                    QueQuan = p.QueQuan,
                    ToiDanh = p.ToiDanh,
                    NgayVaoTrai = p.NgayVaoTrai,
                    NgayRaTrai = p.NgayRaTrai,
                    PhongGiamId = p.PhongGiamId,
                    TrangThai = p.TrangThai,
                    PhongGiam = p.PhongGiam == null ? null : new PhongGiamDTO
                    {
                        Id = p.PhongGiam.Id,
                        MaPhong = p.PhongGiam.MaPhong,
                        TenPhong = p.PhongGiam.TenPhong,
                        SucChua = p.PhongGiam.SucChua,
                        SoLuongHienTai = p.PhongGiam.SoLuongHienTai,
                        LoaiPhong = p.PhongGiam.LoaiPhong,
                        TrangThai = p.PhongGiam.TrangThai
                    }
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PhamNhanDTO>> GetById(int id)
        {
            var item = await _context.PhamNhans
                .Include(p => p.PhongGiam)
                .FirstOrDefaultAsync(p => p.Id == id);
                
            if (item == null) return NotFound();

            return Ok(new PhamNhanDTO
            {
                Id = item.Id,
                MaPhamNhan = item.MaPhamNhan,
                HoTen = item.HoTen,
                NgaySinh = item.NgaySinh,
                GioiTinh = item.GioiTinh,
                QueQuan = item.QueQuan,
                ToiDanh = item.ToiDanh,
                NgayVaoTrai = item.NgayVaoTrai,
                NgayRaTrai = item.NgayRaTrai,
                PhongGiamId = item.PhongGiamId,
                TrangThai = item.TrangThai,
                PhongGiam = item.PhongGiam == null ? null : new PhongGiamDTO
                {
                    Id = item.PhongGiam.Id,
                    MaPhong = item.PhongGiam.MaPhong,
                    TenPhong = item.PhongGiam.TenPhong,
                    SucChua = item.PhongGiam.SucChua,
                    SoLuongHienTai = item.PhongGiam.SoLuongHienTai,
                    LoaiPhong = item.PhongGiam.LoaiPhong,
                    TrangThai = item.PhongGiam.TrangThai
                }
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,CanBo")]
        public async Task<ActionResult<PhamNhanDTO>> Create([FromBody] CreatePhamNhanDTO dto)
        {
            // Check room capacity
            var room = await _context.PhongGiams.FindAsync(dto.PhongGiamId);
            if (room == null) return BadRequest("Phòng giam không tồn tại");
            if (room.SoLuongHienTai >= room.SucChua)
                return BadRequest("Phòng giam đã đầy");

            var item = new PhamNhan
            {
                MaPhamNhan = dto.MaPhamNhan,
                HoTen = dto.HoTen,
                NgaySinh = dto.NgaySinh,
                GioiTinh = dto.GioiTinh,
                QueQuan = dto.QueQuan,
                ToiDanh = dto.ToiDanh,
                NgayVaoTrai = dto.NgayVaoTrai,
                PhongGiamId = dto.PhongGiamId,
                TrangThai = dto.TrangThai
            };

            _context.PhamNhans.Add(item);
            room.SoLuongHienTai++;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, new PhamNhanDTO
            {
                Id = item.Id,
                MaPhamNhan = item.MaPhamNhan,
                HoTen = item.HoTen,
                NgaySinh = item.NgaySinh,
                GioiTinh = item.GioiTinh,
                QueQuan = item.QueQuan,
                ToiDanh = item.ToiDanh,
                NgayVaoTrai = item.NgayVaoTrai,
                PhongGiamId = item.PhongGiamId,
                TrangThai = item.TrangThai
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,CanBo")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePhamNhanDTO dto)
        {
            var item = await _context.PhamNhans.FindAsync(id);
            if (item == null) return NotFound();

            // Handle room change
            if (dto.PhongGiamId.HasValue && dto.PhongGiamId != item.PhongGiamId)
            {
                var oldRoom = await _context.PhongGiams.FindAsync(item.PhongGiamId);
                var newRoom = await _context.PhongGiams.FindAsync(dto.PhongGiamId);
                
                if (newRoom == null) return BadRequest("Phòng giam mới không tồn tại");
                if (newRoom.SoLuongHienTai >= newRoom.SucChua)
                    return BadRequest("Phòng giam mới đã đầy");

                if (oldRoom != null) oldRoom.SoLuongHienTai--;
                newRoom.SoLuongHienTai++;
                item.PhongGiamId = dto.PhongGiamId.Value;
            }

            if (dto.MaPhamNhan != null) item.MaPhamNhan = dto.MaPhamNhan;
            if (dto.HoTen != null) item.HoTen = dto.HoTen;
            if (dto.NgaySinh.HasValue) item.NgaySinh = dto.NgaySinh;
            if (dto.GioiTinh != null) item.GioiTinh = dto.GioiTinh;
            if (dto.QueQuan != null) item.QueQuan = dto.QueQuan;
            if (dto.ToiDanh != null) item.ToiDanh = dto.ToiDanh;
            if (dto.NgayVaoTrai.HasValue) item.NgayVaoTrai = dto.NgayVaoTrai.Value;
            if (dto.NgayRaTrai.HasValue) item.NgayRaTrai = dto.NgayRaTrai;
            if (dto.TrangThai != null) item.TrangThai = dto.TrangThai;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.PhamNhans.FindAsync(id);
            if (item == null) return NotFound();

            var room = await _context.PhongGiams.FindAsync(item.PhongGiamId);
            if (room != null) room.SoLuongHienTai--;

            _context.PhamNhans.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
