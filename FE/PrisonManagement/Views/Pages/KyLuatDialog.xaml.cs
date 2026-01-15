using System;
using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Models;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class KyLuatDialog : Window
    {
        private readonly ApiService _apiService;
        private readonly KyLuat? _editing;
        private readonly bool _isEdit;

        public KyLuatDialog(ApiService apiService, KyLuat? item = null)
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
                    dpNgay.SelectedDate = _editing.NgayKyLuat;
                    txtLyDo.Text = _editing.LyDo;
                    txtThoiHan.Text = _editing.ThoiHan;
                    txtNguoiKy.Text = _editing.NguoiKy;
                    
                    foreach (ComboBoxItem i in cboHinhThuc.Items)
                    {
                        if (i.Content?.ToString() == _editing.HinhThuc)
                        {
                            cboHinhThuc.SelectedItem = i;
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

            if (dpNgay.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày kỷ luật!", "Cảnh báo");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLyDo.Text))
            {
                MessageBox.Show("Vui lòng nhập lý do!", "Cảnh báo");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNguoiKy.Text))
            {
                MessageBox.Show("Vui lòng nhập người ký!", "Cảnh báo");
                return;
            }

            try
            {
                var item = new KyLuat
                {
                    PhamNhanId = (int)cboPhamNhan.SelectedValue,
                    NgayKyLuat = dpNgay.SelectedDate.Value,
                    HinhThuc = (cboHinhThuc.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "",
                    LyDo = txtLyDo.Text.Trim(),
                    ThoiHan = txtThoiHan.Text,
                    NguoiKy = txtNguoiKy.Text.Trim()
                };

                bool ok = _isEdit 
                    ? await _apiService.UpdateKyLuatAsync(_editing!.Id, item) 
                    : await _apiService.CreateKyLuatAsync(item);

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
