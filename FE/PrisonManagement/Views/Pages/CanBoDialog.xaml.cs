using System;
using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Models;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class CanBoDialog : Window
    {
        private readonly ApiService _apiService;
        private readonly CanBo? _editingCanBo;
        private readonly bool _isEditMode;

        public CanBoDialog(ApiService apiService, CanBo? canBo = null)
        {
            InitializeComponent();
            _apiService = apiService;
            _editingCanBo = canBo;
            _isEditMode = canBo != null;

            if (_isEditMode && canBo != null)
            {
                txtTitle.Text = "Chỉnh sửa cán bộ";
                LoadCanBoData(canBo);
            }
        }

        private void LoadCanBoData(CanBo canBo)
        {
            txtMaCanBo.Text = canBo.MaCanBo;
            txtHoTen.Text = canBo.HoTen;
            dpNgaySinh.SelectedDate = canBo.NgaySinh;
            txtChucVu.Text = canBo.ChucVu;
            txtPhongPhuTrach.Text = canBo.PhongPhuTrach;
            txtSDT.Text = canBo.SDT;
            txtDiaChi.Text = canBo.DiaChi;

            if (canBo.GioiTinh == "Nam")
                cboGioiTinh.SelectedIndex = 0;
            else if (canBo.GioiTinh == "Nữ")
                cboGioiTinh.SelectedIndex = 1;
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaCanBo.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!", "Cảnh báo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var canBo = new CanBo
            {
                MaCanBo = txtMaCanBo.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                NgaySinh = dpNgaySinh.SelectedDate,
                GioiTinh = (cboGioiTinh.SelectedItem as ComboBoxItem)?.Content?.ToString(),
                ChucVu = txtChucVu.Text.Trim(),
                PhongPhuTrach = txtPhongPhuTrach.Text.Trim(),
                SDT = txtSDT.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim()
            };

            try
            {
                bool success;
                if (_isEditMode && _editingCanBo != null)
                {
                    canBo.Id = _editingCanBo.Id;
                    success = await _apiService.UpdateCanBoAsync(_editingCanBo.Id, canBo);
                }
                else
                {
                    success = await _apiService.CreateCanBoAsync(canBo);
                }

                if (success)
                {
                    MessageBox.Show(_isEditMode ? "Cập nhật thành công!" : "Thêm mới thành công!", 
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}