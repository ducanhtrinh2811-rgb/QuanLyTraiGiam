using System;
using System.Windows;
using System.Windows.Controls;
using PrisonManagement.Models;
using PrisonManagement.Services;

namespace PrisonManagement.Views.Pages
{
    public partial class SuCoDialog : Window
    {
        private readonly ApiService _apiService;
        private readonly SuCo? _editing;
        private readonly bool _isEdit;

        public SuCoDialog(ApiService apiService, SuCo? item = null)
        {
            InitializeComponent();
            _apiService = apiService;
            _editing = item;
            _isEdit = item != null;
            
            if (_isEdit && item != null)
            {
                LoadData(item);
            }
        }

        private void LoadData(SuCo item)
        {
            txtTitle.Text = "Chỉnh sửa";
            dpNgayXayRa.SelectedDate = item.NgayXayRa;
            txtMoTa.Text = item.MoTa;
            txtPhamNhanLienQuan.Text = item.PhamNhanLienQuan;
            txtBienPhapXuLy.Text = item.BienPhapXuLy;
            txtNguoiBaoCao.Text = item.NguoiBaoCao;
            
            foreach (ComboBoxItem i in cboLoaiSuCo.Items)
            {
                if (i.Content?.ToString() == item.LoaiSuCo)
                {
                    cboLoaiSuCo.SelectedItem = i;
                    break;
                }
            }
            
            foreach (ComboBoxItem i in cboMucDo.Items)
            {
                if (i.Content?.ToString() == item.MucDo)
                {
                    cboMucDo.SelectedItem = i;
                    break;
                }
            }
            
            foreach (ComboBoxItem i in cboTrangThai.Items)
            {
                if (i.Content?.ToString() == item.TrangThai)
                {
                    cboTrangThai.SelectedItem = i;
                    break;
                }
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (dpNgayXayRa.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày xảy ra!", "Cảnh báo");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMoTa.Text))
            {
                MessageBox.Show("Vui lòng nhập mô tả!", "Cảnh báo");
                return;
            }

            try
            {
                var item = new SuCo
                {
                    NgayXayRa = dpNgayXayRa.SelectedDate.Value,
                    LoaiSuCo = (cboLoaiSuCo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Khac",
                    MoTa = txtMoTa.Text.Trim(),
                    MucDo = (cboMucDo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Nhe",
                    PhamNhanLienQuan = txtPhamNhanLienQuan.Text,
                    BienPhapXuLy = txtBienPhapXuLy.Text,
                    NguoiBaoCao = txtNguoiBaoCao.Text,
                    TrangThai = (cboTrangThai.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "DangXuLy"
                };

                bool ok = _isEdit 
                    ? await _apiService.UpdateSuCoAsync(_editing!.Id, item) 
                    : await _apiService.CreateSuCoAsync(item);

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
