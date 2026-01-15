using System;
using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Models;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class SucKhoeDialog : Window
    {
        private readonly ApiService _apiService;
        private readonly SucKhoe? _editing;
        private readonly bool _isEdit;

        public SucKhoeDialog(ApiService apiService, SucKhoe? item = null)
        {
            InitializeComponent();
            _apiService = apiService;
            _editing = item;
            _isEdit = item != null;
            Loaded += async (s, e) => await LoadDataAsync();
        }

        private async System.Threading.Tasks.Task LoadDataAsync()
        {
            try
            {
                cboPhamNhan.ItemsSource = await _apiService.GetPhamNhanAsync();
                
                if (_isEdit && _editing != null)
                {
                    LoadData(_editing);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
            }
        }

        private void LoadData(SucKhoe item)
        {
            txtTitle.Text = "Chỉnh sửa sức khỏe";
            cboPhamNhan.SelectedValue = item.PhamNhanId;
            dpNgayKham.SelectedDate = item.NgayKham;
            txtChanDoan.Text = item.ChanDoan;
            txtDieuTri.Text = item.DieuTri;
            txtBacSi.Text = item.BacSi;
            txtGhiChu.Text = item.GhiChu;
            
            foreach (ComboBoxItem i in cboLoaiKham.Items)
            {
                if (i.Content?.ToString() == item.LoaiKham)
                {
                    cboLoaiKham.SelectedItem = i;
                    break;
                }
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cboPhamNhan.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phạm nhân!", "Cảnh báo");
                return;
            }

            if (dpNgayKham.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày khám!", "Cảnh báo");
                return;
            }

            try
            {
                var item = new SucKhoe
                {
                    PhamNhanId = (int)cboPhamNhan.SelectedValue,
                    NgayKham = dpNgayKham.SelectedDate.Value,
                    LoaiKham = (cboLoaiKham.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "DinhKy",
                    ChanDoan = txtChanDoan.Text,
                    DieuTri = txtDieuTri.Text,
                    BacSi = txtBacSi.Text,
                    GhiChu = txtGhiChu.Text
                };

                bool ok = _isEdit 
                    ? await _apiService.UpdateSucKhoeAsync(_editing!.Id, item) 
                    : await _apiService.CreateSucKhoeAsync(item);

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
