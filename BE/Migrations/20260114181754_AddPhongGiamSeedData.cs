using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PrisonManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddPhongGiamSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhongGiam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhong = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenPhong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SucChua = table.Column<int>(type: "int", nullable: false),
                    SoLuongHienTai = table.Column<int>(type: "int", nullable: false),
                    LoaiPhong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongGiam", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quyen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenQuyen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quyen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuCo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayXayRa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoaiSuCo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MucDo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhamNhanLienQuan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BienPhapXuLy = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NguoiBaoCao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuCo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhamNhan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhamNhan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    QueQuan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ToiDanh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NgayVaoTrai = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayRaTrai = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhongGiamId = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhamNhan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhamNhan_PhongGiam_PhongGiamId",
                        column: x => x.PhongGiamId,
                        principalTable: "PhongGiam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatKhauHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    QuyenId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaiKhoan_Quyen_QuyenId",
                        column: x => x.QuyenId,
                        principalTable: "Quyen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KhenThuong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhamNhanId = table.Column<int>(type: "int", nullable: false),
                    NgayKhenThuong = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HinhThuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NguoiKy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhenThuong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KhenThuong_PhamNhan_PhamNhanId",
                        column: x => x.PhamNhanId,
                        principalTable: "PhamNhan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KyLuat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhamNhanId = table.Column<int>(type: "int", nullable: false),
                    NgayKyLuat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HinhThuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ThoiHan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NguoiKy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KyLuat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KyLuat_PhamNhan_PhamNhanId",
                        column: x => x.PhamNhanId,
                        principalTable: "PhamNhan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LaoDong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhamNhanId = table.Column<int>(type: "int", nullable: false),
                    LoaiHoatDong = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenHoatDong = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KetQua = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DanhGia = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaoDong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaoDong_PhamNhan_PhamNhanId",
                        column: x => x.PhamNhanId,
                        principalTable: "PhamNhan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SucKhoe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhamNhanId = table.Column<int>(type: "int", nullable: false),
                    NgayKham = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoaiKham = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ChanDoan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DieuTri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BacSi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SucKhoe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SucKhoe_PhamNhan_PhamNhanId",
                        column: x => x.PhamNhanId,
                        principalTable: "PhamNhan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThamGap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhamNhanId = table.Column<int>(type: "int", nullable: false),
                    NgayThamGap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiThamGap = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QuanHe = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CMND = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ThoiGianBatDau = table.Column<TimeSpan>(type: "time", nullable: true),
                    ThoiGianKetThuc = table.Column<TimeSpan>(type: "time", nullable: true),
                    NoiDungTiepTe = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThamGap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThamGap_PhamNhan_PhamNhanId",
                        column: x => x.PhamNhanId,
                        principalTable: "PhamNhan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CanBo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaCanBo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ChucVu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhongPhuTrach = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TaiKhoanId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanBo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanBo_TaiKhoan_TaiKhoanId",
                        column: x => x.TaiKhoanId,
                        principalTable: "TaiKhoan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "LichSuTruyCap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaiKhoanId = table.Column<int>(type: "int", nullable: false),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HanhDong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChiTiet = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DiaChi_IP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuTruyCap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LichSuTruyCap_TaiKhoan_TaiKhoanId",
                        column: x => x.TaiKhoanId,
                        principalTable: "TaiKhoan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PhongGiam",
                columns: new[] { "Id", "LoaiPhong", "MaPhong", "SoLuongHienTai", "SucChua", "TenPhong", "TrangThai" },
                values: new object[,]
                {
                    { 1, "Bình thường", "A1", 0, 10, "Phòng A1 - Tầng 1", "HoatDong" },
                    { 2, "Bình thường", "A2", 0, 10, "Phòng A2 - Tầng 1", "HoatDong" },
                    { 3, "Đặc biệt", "B1", 0, 8, "Phòng B1 - Tầng 2", "HoatDong" }
                });

            migrationBuilder.InsertData(
                table: "Quyen",
                columns: new[] { "Id", "MoTa", "TenQuyen" },
                values: new object[,]
                {
                    { 1, "Quản trị viên hệ thống", "Admin" },
                    { 2, "Cán bộ quản lý", "CanBo" },
                    { 3, "Giám sát viên", "GiamSat" }
                });

            migrationBuilder.InsertData(
                table: "TaiKhoan",
                columns: new[] { "Id", "IsActive", "MatKhauHash", "QuyenId", "Ten" },
                values: new object[] { 1, true, "admin123", 1, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaCanBo",
                table: "CanBo",
                column: "MaCanBo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_TaiKhoanId",
                table: "CanBo",
                column: "TaiKhoanId",
                unique: true,
                filter: "[TaiKhoanId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_KhenThuong_PhamNhanId",
                table: "KhenThuong",
                column: "PhamNhanId");

            migrationBuilder.CreateIndex(
                name: "IX_KyLuat_PhamNhanId",
                table: "KyLuat",
                column: "PhamNhanId");

            migrationBuilder.CreateIndex(
                name: "IX_LaoDong_PhamNhanId",
                table: "LaoDong",
                column: "PhamNhanId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuTruyCap_TaiKhoanId",
                table: "LichSuTruyCap",
                column: "TaiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_PhamNhan_MaPhamNhan",
                table: "PhamNhan",
                column: "MaPhamNhan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhamNhan_PhongGiamId",
                table: "PhamNhan",
                column: "PhongGiamId");

            migrationBuilder.CreateIndex(
                name: "IX_PhongGiam_MaPhong",
                table: "PhongGiam",
                column: "MaPhong",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SucKhoe_PhamNhanId",
                table: "SucKhoe",
                column: "PhamNhanId");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_QuyenId",
                table: "TaiKhoan",
                column: "QuyenId");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_Ten",
                table: "TaiKhoan",
                column: "Ten",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThamGap_PhamNhanId",
                table: "ThamGap",
                column: "PhamNhanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CanBo");

            migrationBuilder.DropTable(
                name: "KhenThuong");

            migrationBuilder.DropTable(
                name: "KyLuat");

            migrationBuilder.DropTable(
                name: "LaoDong");

            migrationBuilder.DropTable(
                name: "LichSuTruyCap");

            migrationBuilder.DropTable(
                name: "SucKhoe");

            migrationBuilder.DropTable(
                name: "SuCo");

            migrationBuilder.DropTable(
                name: "ThamGap");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "PhamNhan");

            migrationBuilder.DropTable(
                name: "Quyen");

            migrationBuilder.DropTable(
                name: "PhongGiam");
        }
    }
}
