CREATE DATABASE PrisonDB;
GO

USE PrisonDB;
GO


--TABLES                          


CREATE TABLE Quyen (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TenQuyen NVARCHAR(50) NOT NULL,
    MoTa NVARCHAR(255)
);

CREATE TABLE TaiKhoan (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Ten NVARCHAR(100) NOT NULL UNIQUE,
    MatKhauHash NVARCHAR(255) NOT NULL,
    QuyenId INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (QuyenId) REFERENCES Quyen(Id)
);

CREATE TABLE CanBo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MaCanBo NVARCHAR(20) NOT NULL UNIQUE,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE NULL,
    GioiTinh NVARCHAR(10) NULL,
    ChucVu NVARCHAR(100) NULL,
    PhongPhuTrach NVARCHAR(100) NULL,
    SDT NVARCHAR(20) NULL,
    DiaChi NVARCHAR(255) NULL,
    TaiKhoanId INT NULL UNIQUE,
    FOREIGN KEY (TaiKhoanId) REFERENCES TaiKhoan(Id)
);

CREATE TABLE PhongGiam (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MaPhong NVARCHAR(20) NOT NULL UNIQUE,
    TenPhong NVARCHAR(100) NOT NULL,
    SucChua INT NOT NULL,
    SoLuongHienTai INT NOT NULL DEFAULT 0,
    LoaiPhong NVARCHAR(50) NULL,
    TrangThai NVARCHAR(20) NOT NULL DEFAULT 'HoatDong'
);

CREATE TABLE PhamNhan (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MaPhamNhan NVARCHAR(20) NOT NULL UNIQUE,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE NULL,
    GioiTinh NVARCHAR(10) NULL,
    QueQuan NVARCHAR(255) NULL,
    ToiDanh NVARCHAR(255) NULL,
    NgayVaoTrai DATETIME2 NOT NULL,
    NgayRaTrai DATETIME2 NULL,
    PhongGiamId INT NOT NULL,
    TrangThai NVARCHAR(20) NOT NULL DEFAULT 'DangGiam',
    FOREIGN KEY (PhongGiamId) REFERENCES PhongGiam(Id)
);

CREATE TABLE KhenThuong (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PhamNhanId INT NOT NULL,
    NgayKhenThuong DATETIME2 NOT NULL,
    LyDo NVARCHAR(500) NOT NULL,
    HinhThuc NVARCHAR(100) NOT NULL,
    NguoiKy NVARCHAR(100) NOT NULL,
    GhiChu NVARCHAR(500) NULL,
    FOREIGN KEY (PhamNhanId) REFERENCES PhamNhan(Id) ON DELETE CASCADE
);

CREATE TABLE KyLuat (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PhamNhanId INT NOT NULL,
    NgayKyLuat DATETIME2 NOT NULL,
    LyDo NVARCHAR(500) NOT NULL,
    HinhThuc NVARCHAR(100) NOT NULL,
    ThoiHan NVARCHAR(50) NULL,
    NguoiKy NVARCHAR(100) NOT NULL,
    GhiChu NVARCHAR(500) NULL,
    FOREIGN KEY (PhamNhanId) REFERENCES PhamNhan(Id) ON DELETE CASCADE
);

CREATE TABLE SucKhoe (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PhamNhanId INT NOT NULL,
    NgayKham DATETIME2 NOT NULL,
    LoaiKham NVARCHAR(20) NOT NULL,
    ChanDoan NVARCHAR(500) NULL,
    DieuTri NVARCHAR(500) NULL,
    BacSi NVARCHAR(100) NULL,
    GhiChu NVARCHAR(500) NULL,
    FOREIGN KEY (PhamNhanId) REFERENCES PhamNhan(Id) ON DELETE CASCADE
);

CREATE TABLE ThamGap (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PhamNhanId INT NOT NULL,
    NgayThamGap DATETIME2 NOT NULL,
    NguoiThamGap NVARCHAR(100) NOT NULL,
    QuanHe NVARCHAR(50) NULL,
    CMND NVARCHAR(20) NULL,
    ThoiGianBatDau TIME NULL,
    ThoiGianKetThuc TIME NULL,
    NoiDungTiepTe NVARCHAR(500) NULL,
    GhiChu NVARCHAR(500) NULL,
    FOREIGN KEY (PhamNhanId) REFERENCES PhamNhan(Id) ON DELETE CASCADE
);

CREATE TABLE LaoDong (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PhamNhanId INT NOT NULL,
    LoaiHoatDong NVARCHAR(20) NOT NULL,
    TenHoatDong NVARCHAR(200) NOT NULL,
    NgayBatDau DATETIME2 NOT NULL,
    NgayKetThuc DATETIME2 NULL,
    KetQua NVARCHAR(255) NULL,
    DanhGia NVARCHAR(20) NULL,
    GhiChu NVARCHAR(500) NULL,
    FOREIGN KEY (PhamNhanId) REFERENCES PhamNhan(Id) ON DELETE CASCADE
);

CREATE TABLE SuCo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NgayXayRa DATETIME2 NOT NULL,
    LoaiSuCo NVARCHAR(20) NOT NULL,
    MoTa NVARCHAR(1000) NOT NULL,
    MucDo NVARCHAR(20) NOT NULL,
    PhamNhanLienQuan NVARCHAR(500) NULL,
    BienPhapXuLy NVARCHAR(1000) NULL,
    NguoiBaoCao NVARCHAR(100) NULL,
    TrangThai NVARCHAR(20) NOT NULL,
    GhiChu NVARCHAR(500) NULL
);

CREATE TABLE LichSuTruyCap (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TaiKhoanId INT NOT NULL,
    ThoiGian DATETIME2 NOT NULL,
    HanhDong NVARCHAR(100) NULL,
    ChiTiet NVARCHAR(500) NULL,
    DiaChi_IP NVARCHAR(50) NULL,
    FOREIGN KEY (TaiKhoanId) REFERENCES TaiKhoan(Id) ON DELETE CASCADE
);


--INDEX                       


CREATE INDEX IX_PhamNhan_PhongGiamId ON PhamNhan(PhongGiamId);
CREATE INDEX IX_TaiKhoan_QuyenId ON TaiKhoan(QuyenId);
CREATE INDEX IX_KhenThuong_PhamNhanId ON KhenThuong(PhamNhanId);
CREATE INDEX IX_KyLuat_PhamNhanId ON KyLuat(PhamNhanId);
CREATE INDEX IX_SucKhoe_PhamNhanId ON SucKhoe(PhamNhanId);
CREATE INDEX IX_ThamGap_PhamNhanId ON ThamGap(PhamNhanId);
CREATE INDEX IX_LaoDong_PhamNhanId ON LaoDong(PhamNhanId);
CREATE INDEX IX_LichSuTruyCap_TaiKhoanId ON LichSuTruyCap(TaiKhoanId);


--DEFAULT DATA                     


INSERT INTO Quyen (TenQuyen, MoTa) VALUES
('Admin', 'Quản trị viên hệ thống'),
('CanBo', 'Cán bộ quản lý'),
('GiamSat', 'Giám sát viên');

INSERT INTO TaiKhoan (Ten, MatKhauHash, QuyenId, IsActive)
VALUES ('admin', 'admin123', 1, 1);

-- Sample PhongGiam
INSERT INTO PhongGiam (MaPhong, TenPhong, SucChua, SoLuongHienTai, LoaiPhong, TrangThai)
VALUES 
('A1', 'Phong A1 - Tang 1', 10, 0, 'Binh thuong', 'HoatDong'),
('A2', 'Phong A2 - Tang 1', 10, 0, 'Binh thuonng', 'HoatDong'),
('B1', 'Phong B1 - Tang 2', 8, 0, 'Dac biet', 'HoatDong');




