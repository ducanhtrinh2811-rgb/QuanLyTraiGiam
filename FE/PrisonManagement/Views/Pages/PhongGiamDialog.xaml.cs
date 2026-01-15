using System;
using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Models;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class PhongGiamDialog : Window
    {
        private readonly ApiService _apiService;
        private readonly PhongGiam? _editingPhongGiam;
        private readonly bool _isEditMode;

        public PhongGiamDialog(ApiService apiService, PhongGiam? phongGiam = null)
        {
            InitializeComponent();
            _apiService = apiService;
            _editingPhongGiam = phongGiam;
            _isEditMode = phongGiam != null;

            if (_isEditMode && phongGiam != null)
            {
                txtTitle.Text = "Chỉnh sửa phòng giam";
                LoadPhongGiamData(phongGiam);
            }
        }

        private void LoadPhongGiamData(PhongGiam phongGiam)
        {
            txtMaPhong.Text = phongGiam.MaPhong;
            txtTenPhong.Text = phongGiam.TenPhong;
            txtSucChua.Text = phongGiam.SucChua.ToString();

            foreach (ComboBoxItem item in cboLoaiPhong.Items)
            {
                if (item.Content?.ToString() == phongGiam.LoaiPhong)
                {
                    cboLoaiPhong.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in cboTrangThai.Items)
            {
                if (item.Content?.ToString() == phongGiam.TrangThai)
                {
                    cboTrangThai.SelectedItem = item;
                    break;
                }
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaPhong.Text) || 
                string.IsNullOrWhiteSpace(txtTenPhong.Text) ||
                string.IsNullOrWhiteSpace(txtSucChua.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!", "Cảnh báo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtSucChua.Text, out int sucChua))
            {
                MessageBox.Show("Sức chứa phải là số nguyên!", "Cảnh báo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var phongGiam = new PhongGiam
            {
                MaPhong = txtMaPhong.Text.Trim(),
                TenPhong = txtTenPhong.Text.Trim(),
                LoaiPhong = (cboLoaiPhong.SelectedItem as ComboBoxItem)?.Content?.ToString(),
                SucChua = sucChua,
                TrangThai = (cboTrangThai.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "HoatDong"
            };

            try
            {
                bool success;
                if (_isEditMode && _editingPhongGiam != null)
                {
                    phongGiam.Id = _editingPhongGiam.Id;
                    phongGiam.SoLuongHienTai = _editingPhongGiam.SoLuongHienTai;
                    success = await _apiService.UpdatePhongGiamAsync(_editingPhongGiam.Id, phongGiam);
                }
                else
                {
                    success = await _apiService.CreatePhongGiamAsync(phongGiam);
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
