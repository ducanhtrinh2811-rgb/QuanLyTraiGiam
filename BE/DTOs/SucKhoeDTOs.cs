namespace PrisonManagement.DTOs
{
    public class SucKhoeDTO
    {
        public int Id { get; set; }
        public int PhamNhanId { get; set; }
        public DateTime NgayKham { get; set; }
        public string LoaiKham { get; set; } = "DinhKy";
        public string? ChanDoan { get; set; }
        public string? DieuTri { get; set; }
        public string? BacSi { get; set; }
        public string? GhiChu { get; set; }
        public PhamNhanSimpleDTO? PhamNhan { get; set; }
    }

    public class CreateSucKhoeDTO
    {
        public int PhamNhanId { get; set; }
        public DateTime NgayKham { get; set; }
        public string LoaiKham { get; set; } = "DinhKy";
        public string? ChanDoan { get; set; }
        public string? DieuTri { get; set; }
        public string? BacSi { get; set; }
        public string? GhiChu { get; set; }
    }

    public class UpdateSucKhoeDTO
    {
        public DateTime? NgayKham { get; set; }
        public string? LoaiKham { get; set; }
        public string? ChanDoan { get; set; }
        public string? DieuTri { get; set; }
        public string? BacSi { get; set; }
        public string? GhiChu { get; set; }
    }
}
