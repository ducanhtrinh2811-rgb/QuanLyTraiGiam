// Database types matching SQL Server schema

export type QuyenType = 'Admin' | 'CanBo' | 'GiamSat';

export interface Quyen {
  Id: number;
  TenQuyen: QuyenType;
  MoTa: string;
}

export interface TaiKhoan {
  Id: number;
  Ten: string;
  MatKhauHash: string;
  QuyenId: number;
  IsActive: boolean;
  Quyen?: Quyen;
}

export interface CanBo {
  Id: number;
  MaCanBo: string;
  HoTen: string;
  NgaySinh: string;
  GioiTinh: 'Nam' | 'Nữ';
  ChucVu: string;
  PhongPhuTrach: string;
  SDT: string;
  DiaChi: string;
  TaiKhoanId?: number;
}

export interface PhongGiam {
  Id: number;
  MaPhong: string;
  TenPhong: string;
  SucChua: number;
  SoLuongHienTai: number;
  LoaiPhong: string;
  TrangThai: 'HoatDong' | 'BaoTri' | 'DaKhoa';
}

export interface PhamNhan {
  Id: number;
  MaPhamNhan: string;
  HoTen: string;
  NgaySinh: string;
  GioiTinh: 'Nam' | 'Nữ';
  QueQuan: string;
  ToiDanh: string;
  NgayVaoTrai: string;
  NgayRaTrai?: string;
  PhongGiamId: number;
  TrangThai: 'DangGiam' | 'DaRa' | 'ChuyenTrai';
  PhongGiam?: PhongGiam;
}

export interface KhenThuong {
  Id: number;
  PhamNhanId: number;
  NgayKhenThuong: string;
  LyDo: string;
  HinhThuc: string;
  NguoiKy: string;
  GhiChu?: string;
  PhamNhan?: PhamNhan;
}

export interface KyLuat {
  Id: number;
  PhamNhanId: number;
  NgayKyLuat: string;
  LyDo: string;
  HinhThuc: string;
  ThoiHan?: string;
  NguoiKy: string;
  GhiChu?: string;
  PhamNhan?: PhamNhan;
}

export interface LichSuTruyCap {
  Id: number;
  TaiKhoanId: number;
  ThoiGian: string;
  HanhDong: string;
  ChiTiet: string;
  DiaChi_IP: string;
}

export interface ThongKePhongGiam {
  MaPhong: string;
  TenPhong: string;
  SucChua: number;
  SoLuongHienTai: number;
  TyLeSuDung: number;
  TrangThai: string;
}

export interface AuthUser {
  id: number;
  username: string;
  role: QuyenType;
  canBoId?: number;
}
