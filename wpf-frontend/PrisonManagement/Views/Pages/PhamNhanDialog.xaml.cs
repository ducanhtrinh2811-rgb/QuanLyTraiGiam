using System;
using System.Windows;
using PrisonManagement.Models;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class PhamNhanDialog : Window
    {
        private readonly ApiService _apiService;
        private readonly PhamNhan? _editingPhamNhan;
        private readonly bool _isEditing;

        public PhamNhanDialog(ApiService apiService, PhamNhan? phamNhan = null)
        {
            InitializeComponent();
            _apiService = apiService;
            _editingPhamNhan = phamNhan;
            _isEditing = phamNhan != null;

            if (_isEditing && phamNhan != null)
            {
                txtTitle.Text = "Chỉnh sửa phạm nhân";
                LoadPhamNhanData(phamNhan);
            }
            else
            {
                dpNgayVaoTrai.SelectedDate = DateTime.Today;
                cboTrangThai.SelectedIndex = 0;
                cboMucDoNguyHiem.SelectedIndex = 0;
                cboGioiTinh.SelectedIndex = 0;
            }
        }

        private void LoadPhamNhanData(PhamNhan phamNhan)
        {
            txtHoTen.Text = phamNhan.HoTen;
            dpNgaySinh.SelectedDate = phamNhan.NgaySinh;
            txtQueQuan.Text = phamNhan.QueQuan;
            txtToiDanh.Text = phamNhan.ToiDanh;
            dpNgayVaoTrai.SelectedDate = phamNhan.NgayVaoTrai;
            txtGhiChu.Text = phamNhan.GhiChu;

            // Set combo boxes
            SetComboBoxByText(cboGioiTinh, phamNhan.GioiTinh);
            SetComboBoxByText(cboMucDoNguyHiem, phamNhan.MucDoNguyHiem);
            SetComboBoxByText(cboTrangThai, phamNhan.TrangThai);
        }

        private void SetComboBoxByText(System.Windows.Controls.ComboBox comboBox, string text)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                var item = comboBox.Items[i] as System.Windows.Controls.ComboBoxItem;
                if (item?.Content?.ToString() == text)
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!dpNgaySinh.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtToiDanh.Text))
            {
                MessageBox.Show("Vui lòng nhập tội danh!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var phamNhan = new PhamNhan
            {
                Id = _editingPhamNhan?.Id ?? 0,
                HoTen = txtHoTen.Text.Trim(),
                NgaySinh = dpNgaySinh.SelectedDate.Value,
                GioiTinh = (cboGioiTinh.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString() ?? "Nam",
                QueQuan = txtQueQuan.Text.Trim(),
                ToiDanh = txtToiDanh.Text.Trim(),
                MucDoNguyHiem = (cboMucDoNguyHiem.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString() ?? "Thấp",
                NgayVaoTrai = dpNgayVaoTrai.SelectedDate ?? DateTime.Today,
                TrangThai = (cboTrangThai.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString() ?? "Đang thụ án",
                GhiChu = txtGhiChu.Text.Trim()
            };

            btnSave.IsEnabled = false;
            btnSave.Content = "Đang lưu...";

            try
            {
                bool success;
                
                if (_isEditing)
                {
                    success = await _apiService.UpdatePhamNhanAsync(phamNhan.Id, phamNhan);
                }
                else
                {
                    success = await _apiService.CreatePhamNhanAsync(phamNhan);
                }

                if (success)
                {
                    MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Lưu thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                btnSave.IsEnabled = true;
                btnSave.Content = "Lưu";
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
