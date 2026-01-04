namespace PrisonManagement.DTOs
{
    public class ThongKePhongGiamDTO
    {
        public string MaPhong { get; set; } = string.Empty;
        public string TenPhong { get; set; } = string.Empty;
        public int SucChua { get; set; }
        public int SoLuongHienTai { get; set; }
        public int TyLeSuDung { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }

    public class DashboardStatsDTO
    {
        public int TongPhamNhan { get; set; }
        public int TongCanBo { get; set; }
        public int TongPhongGiam { get; set; }
        public int TongKhenThuong { get; set; }
        public int TongKyLuat { get; set; }
    }
}
