import { 
  TaiKhoan, 
  Quyen, 
  CanBo, 
  PhongGiam, 
  PhamNhan, 
  KhenThuong, 
  KyLuat 
} from '@/types';

export const mockQuyen: Quyen[] = [
  { Id: 1, TenQuyen: 'Admin', MoTa: 'Quản trị viên hệ thống' },
  { Id: 2, TenQuyen: 'CanBo', MoTa: 'Cán bộ quản lý' },
  { Id: 3, TenQuyen: 'GiamSat', MoTa: 'Giám sát viên' },
];

export const mockTaiKhoan: TaiKhoan[] = [
  { Id: 1, Ten: 'admin', MatKhauHash: 'admin123', QuyenId: 1, IsActive: true },
  { Id: 2, Ten: 'canbo1', MatKhauHash: 'canbo123', QuyenId: 2, IsActive: true },
  { Id: 3, Ten: 'giamsat1', MatKhauHash: 'giamsat123', QuyenId: 3, IsActive: true },
];

export const mockCanBo: CanBo[] = [
  {
    Id: 1,
    MaCanBo: 'CB001',
    HoTen: 'Nguyễn Văn An',
    NgaySinh: '1985-03-15',
    GioiTinh: 'Nam',
    ChucVu: 'Trưởng phòng',
    PhongPhuTrach: 'Khu A',
    SDT: '0901234567',
    DiaChi: '123 Đường Lê Lợi, Quận 1, TP.HCM',
  },
  {
    Id: 2,
    MaCanBo: 'CB002',
    HoTen: 'Trần Thị Bình',
    NgaySinh: '1990-07-22',
    GioiTinh: 'Nữ',
    ChucVu: 'Phó phòng',
    PhongPhuTrach: 'Khu B',
    SDT: '0912345678',
    DiaChi: '456 Đường Nguyễn Huệ, Quận 3, TP.HCM',
  },
  {
    Id: 3,
    MaCanBo: 'CB003',
    HoTen: 'Lê Minh Cường',
    NgaySinh: '1988-11-08',
    GioiTinh: 'Nam',
    ChucVu: 'Cán bộ',
    PhongPhuTrach: 'Khu C',
    SDT: '0923456789',
    DiaChi: '789 Đường Trần Hưng Đạo, Quận 5, TP.HCM',
  },
];

export const mockPhongGiam: PhongGiam[] = [
  { Id: 1, MaPhong: 'PG-A01', TenPhong: 'Phòng A01', SucChua: 20, SoLuongHienTai: 15, LoaiPhong: 'Thường', TrangThai: 'HoatDong' },
  { Id: 2, MaPhong: 'PG-A02', TenPhong: 'Phòng A02', SucChua: 20, SoLuongHienTai: 18, LoaiPhong: 'Thường', TrangThai: 'HoatDong' },
  { Id: 3, MaPhong: 'PG-B01', TenPhong: 'Phòng B01', SucChua: 15, SoLuongHienTai: 12, LoaiPhong: 'Đặc biệt', TrangThai: 'HoatDong' },
  { Id: 4, MaPhong: 'PG-B02', TenPhong: 'Phòng B02', SucChua: 15, SoLuongHienTai: 15, LoaiPhong: 'Đặc biệt', TrangThai: 'HoatDong' },
  { Id: 5, MaPhong: 'PG-C01', TenPhong: 'Phòng C01', SucChua: 10, SoLuongHienTai: 0, LoaiPhong: 'Cách ly', TrangThai: 'BaoTri' },
];

export const mockPhamNhan: PhamNhan[] = [
  {
    Id: 1,
    MaPhamNhan: 'PN001',
    HoTen: 'Hoàng Văn Dũng',
    NgaySinh: '1980-05-12',
    GioiTinh: 'Nam',
    QueQuan: 'Hà Nội',
    ToiDanh: 'Trộm cắp tài sản',
    NgayVaoTrai: '2022-01-15',
    PhongGiamId: 1,
    TrangThai: 'DangGiam',
  },
  {
    Id: 2,
    MaPhamNhan: 'PN002',
    HoTen: 'Phạm Thị Hoa',
    NgaySinh: '1992-08-25',
    GioiTinh: 'Nữ',
    QueQuan: 'Hải Phòng',
    ToiDanh: 'Lừa đảo chiếm đoạt tài sản',
    NgayVaoTrai: '2021-06-20',
    PhongGiamId: 2,
    TrangThai: 'DangGiam',
  },
  {
    Id: 3,
    MaPhamNhan: 'PN003',
    HoTen: 'Ngô Văn Hùng',
    NgaySinh: '1975-12-03',
    GioiTinh: 'Nam',
    QueQuan: 'Đà Nẵng',
    ToiDanh: 'Cố ý gây thương tích',
    NgayVaoTrai: '2020-09-10',
    PhongGiamId: 3,
    TrangThai: 'DangGiam',
  },
  {
    Id: 4,
    MaPhamNhan: 'PN004',
    HoTen: 'Lý Văn Khang',
    NgaySinh: '1988-02-18',
    GioiTinh: 'Nam',
    QueQuan: 'Cần Thơ',
    ToiDanh: 'Buôn bán ma túy',
    NgayVaoTrai: '2023-03-05',
    PhongGiamId: 1,
    TrangThai: 'DangGiam',
  },
];

export const mockKhenThuong: KhenThuong[] = [
  {
    Id: 1,
    PhamNhanId: 1,
    NgayKhenThuong: '2023-06-15',
    LyDo: 'Tham gia tích cực lao động sản xuất',
    HinhThuc: 'Giấy khen',
    NguoiKy: 'Nguyễn Văn An',
  },
  {
    Id: 2,
    PhamNhanId: 3,
    NgayKhenThuong: '2023-09-20',
    LyDo: 'Có thành tích xuất sắc trong học tập',
    HinhThuc: 'Bằng khen',
    NguoiKy: 'Trần Thị Bình',
  },
];

export const mockKyLuat: KyLuat[] = [
  {
    Id: 1,
    PhamNhanId: 2,
    NgayKyLuat: '2023-04-10',
    LyDo: 'Vi phạm nội quy trại giam',
    HinhThuc: 'Cảnh cáo',
    NguoiKy: 'Nguyễn Văn An',
  },
  {
    Id: 2,
    PhamNhanId: 4,
    NgayKyLuat: '2023-08-25',
    LyDo: 'Đánh nhau với phạm nhân khác',
    HinhThuc: 'Biệt giam 7 ngày',
    ThoiHan: '7 ngày',
    NguoiKy: 'Lê Minh Cường',
  },
];
