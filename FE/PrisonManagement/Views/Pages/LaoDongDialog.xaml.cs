using System;
using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Models;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class LaoDongDialog : Window
    {
        private readonly ApiService _apiService;
        private readonly LaoDong? _editing;
        private readonly bool _isEdit;

        public LaoDongDialog(ApiService apiService, LaoDong? item = null)
        {
            InitializeComponent();
            _apiService = apiService;
            _editing = item;
            _isEdit = item != null;
            Loaded += async (s, e) => await LoadData();
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            try
            {
                cboPhamNhan.ItemsSource = await _apiService.GetPhamNhanAsync();
                
                if (_isEdit && _editing != null)
                {
                    txtTitle.Text = "Chỉnh sửa";
                    cboPhamNhan.SelectedValue = _editing.PhamNhanId;
                    txtTenHoatDong.Text = _editing.TenHoatDong;
                    dpNgayBatDau.SelectedDate = _editing.NgayBatDau;
                    dpNgayKetThuc.SelectedDate = _editing.NgayKetThuc;
                    
                    foreach (ComboBoxItem i in cboLoaiHoatDong.Items)
                    {
                        if (i.Content?.ToString() == _editing.LoaiHoatDong)
                        {
                            cboLoaiHoatDong.SelectedItem = i;
                            break;
                        }
                    }
                    
                    foreach (ComboBoxItem i in cboDanhGia.Items)
                    {
                        if (i.Content?.ToString() == _editing.DanhGia)
                        {
                            cboDanhGia.SelectedItem = i;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cboPhamNhan.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phạm nhân!", "Cảnh báo");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenHoatDong.Text))
            {
                MessageBox.Show("Vui lòng nhập tên hoạt động!", "Cảnh báo");
                return;
            }

            if (dpNgayBatDau.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu!", "Cảnh báo");
                return;
            }

            try
            {
                var item = new LaoDong
                {
                    PhamNhanId = (int)cboPhamNhan.SelectedValue,
                    TenHoatDong = txtTenHoatDong.Text.Trim(),
                    LoaiHoatDong = (cboLoaiHoatDong.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "LaoDong",
                    NgayBatDau = dpNgayBatDau.SelectedDate.Value,
                    NgayKetThuc = dpNgayKetThuc.SelectedDate,
                    DanhGia = (cboDanhGia.SelectedItem as ComboBoxItem)?.Content?.ToString()
                };

                bool ok = _isEdit 
                    ? await _apiService.UpdateLaoDongAsync(_editing!.Id, item) 
                    : await _apiService.CreateLaoDongAsync(item);

                if (ok)
                {
                    MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Lưu thất bại! Vui lòng kiểm tra kết nối server.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}\n\nChitiết: {ex.InnerException?.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
