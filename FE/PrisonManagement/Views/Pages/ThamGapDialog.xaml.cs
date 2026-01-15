using System;
using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Models;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class ThamGapDialog : Window
    {
        private readonly ApiService _apiService;
        private readonly ThamGap? _editing;
        private readonly bool _isEdit;

        public ThamGapDialog(ApiService apiService, ThamGap? item = null)
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
                    txtTitle.Text = "Chỉnh sửa";
                    cboPhamNhan.SelectedValue = _editing.PhamNhanId;
                    dpNgayThamGap.SelectedDate = _editing.NgayThamGap;
                    txtNguoiThamGap.Text = _editing.NguoiThamGap;
                    txtQuanHe.Text = _editing.QuanHe;
                    txtCMND.Text = _editing.CMND;
                    txtNoiDungTiepTe.Text = _editing.NoiDungTiepTe;
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

            if (dpNgayThamGap.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày thăm!", "Cảnh báo");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNguoiThamGap.Text))
            {
                MessageBox.Show("Vui lòng nhập tên người thăm!", "Cảnh báo");
                return;
            }

            try
            {
                var item = new ThamGap
                {
                    PhamNhanId = (int)cboPhamNhan.SelectedValue,
                    NgayThamGap = dpNgayThamGap.SelectedDate.Value,
                    NguoiThamGap = txtNguoiThamGap.Text.Trim(),
                    QuanHe = txtQuanHe.Text,
                    CMND = txtCMND.Text,
                    NoiDungTiepTe = txtNoiDungTiepTe.Text
                };

                bool ok = _isEdit 
                    ? await _apiService.UpdateThamGapAsync(_editing!.Id, item) 
                    : await _apiService.CreateThamGapAsync(item);

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
