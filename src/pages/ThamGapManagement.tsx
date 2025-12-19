import React, { useState, useEffect, useMemo } from 'react';
import { PageHeader } from '@/components/PageHeader';
import { FormModal, FormField } from '@/components/FormModal';
import { ConfirmDialog } from '@/components/ConfirmDialog';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Button } from '@/components/ui/button';
import { Plus, Trash2, Search, Users } from 'lucide-react';
import { thamGapApi, phamNhanApi } from '@/api/mockApi';
import { ThamGap, PhamNhan } from '@/types';
import { useAuth } from '@/contexts/AuthContext';
import { toast } from 'sonner';

const ThamGapManagement: React.FC = () => {
  const { hasPermission } = useAuth();
  const canEdit = hasPermission(['Admin', 'CanBo']);

  const [data, setData] = useState<ThamGap[]>([]);
  const [phamNhanList, setPhamNhanList] = useState<PhamNhan[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [searchValue, setSearchValue] = useState('');
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedItem, setSelectedItem] = useState<ThamGap | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [formData, setFormData] = useState({
    PhamNhanId: 0,
    NgayThamGap: '',
    NguoiThamGap: '',
    QuanHe: '',
    CMND: '',
    ThoiGianBatDau: '',
    ThoiGianKetThuc: '',
    NoiDungTiepTe: '',
    GhiChu: '',
  });

  const loadData = async () => {
    setIsLoading(true);
    try {
      const [tgResult, pnResult] = await Promise.all([
        thamGapApi.getAll(),
        phamNhanApi.getAll(),
      ]);
      setData(tgResult);
      setPhamNhanList(pnResult.filter(p => p.TrangThai === 'DangGiam'));
    } catch {
      toast.error('Không thể tải dữ liệu');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const filteredData = useMemo(() => {
    if (!searchValue) return data;
    const search = searchValue.toLowerCase();
    return data.filter(
      item =>
        item.PhamNhan?.HoTen.toLowerCase().includes(search) ||
        item.NguoiThamGap.toLowerCase().includes(search)
    );
  }, [data, searchValue]);

  const resetForm = () => {
    setFormData({
      PhamNhanId: 0,
      NgayThamGap: '',
      NguoiThamGap: '',
      QuanHe: '',
      CMND: '',
      ThoiGianBatDau: '',
      ThoiGianKetThuc: '',
      NoiDungTiepTe: '',
      GhiChu: '',
    });
    setSelectedItem(null);
  };

  const handleAdd = () => {
    resetForm();
    setIsModalOpen(true);
  };

  const handleDelete = (item: ThamGap) => {
    setSelectedItem(item);
    setIsDeleteDialogOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.PhamNhanId || !formData.NguoiThamGap || !formData.QuanHe || !formData.CMND) {
      toast.error('Vui lòng nhập đầy đủ thông tin bắt buộc');
      return;
    }

    setIsSubmitting(true);
    try {
      await thamGapApi.create({
        PhamNhanId: formData.PhamNhanId,
        NgayThamGap: formData.NgayThamGap || new Date().toISOString().split('T')[0],
        NguoiThamGap: formData.NguoiThamGap,
        QuanHe: formData.QuanHe,
        CMND: formData.CMND,
        ThoiGianBatDau: formData.ThoiGianBatDau || '08:00',
        ThoiGianKetThuc: formData.ThoiGianKetThuc || '09:00',
        NoiDungTiepTe: formData.NoiDungTiepTe,
        GhiChu: formData.GhiChu,
      });
      toast.success('Đăng ký thăm gặp thành công');
      setIsModalOpen(false);
      resetForm();
      loadData();
    } catch {
      toast.error('Có lỗi xảy ra');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleConfirmDelete = async () => {
    if (!selectedItem) return;
    
    try {
      await thamGapApi.delete(selectedItem.Id);
      toast.success('Xóa thành công');
      setIsDeleteDialogOpen(false);
      setSelectedItem(null);
      loadData();
    } catch {
      toast.error('Có lỗi xảy ra');
    }
  };

  return (
    <div className="min-h-screen bg-background">
      <PageHeader title="Thăm gặp - Tiếp tế" icon="👥" showBackButton />

      <main className="container mx-auto px-4 py-8">
        <div className="bg-card rounded-xl border border-border shadow-sm animate-slide-up">
          <div className="p-4 border-b border-border flex flex-col sm:flex-row gap-4 items-start sm:items-center justify-between">
            <div className="relative flex-1 max-w-md">
              <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-muted-foreground" />
              <Input
                value={searchValue}
                onChange={(e) => setSearchValue(e.target.value)}
                placeholder="Tìm kiếm..."
                className="pl-10"
              />
            </div>
            {canEdit && (
              <Button onClick={handleAdd} className="gap-2">
                <Plus className="w-4 h-4" />
                Đăng ký thăm gặp
              </Button>
            )}
          </div>
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="border-b border-border bg-muted/50">
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">STT</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Phạm nhân</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Ngày</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Người thăm</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Quan hệ</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">CMND/CCCD</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Thời gian</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Tiếp tế</th>
                  {canEdit && <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Thao tác</th>}
                </tr>
              </thead>
              <tbody>
                {isLoading ? (
                  <tr>
                    <td colSpan={9} className="text-center py-12 text-muted-foreground">
                      <div className="flex items-center justify-center gap-2">
                        <div className="w-5 h-5 border-2 border-primary border-t-transparent rounded-full animate-spin" />
                        Đang tải...
                      </div>
                    </td>
                  </tr>
                ) : filteredData.length === 0 ? (
                  <tr>
                    <td colSpan={9} className="text-center py-12 text-muted-foreground">
                      <Users className="w-12 h-12 mx-auto mb-3 opacity-30" />
                      Chưa có lịch thăm gặp
                    </td>
                  </tr>
                ) : (
                  filteredData.map((item, index) => (
                    <tr key={item.Id} className="border-b border-border last:border-0 hover:bg-muted/30 transition-colors">
                      <td className="p-4 text-sm">{index + 1}</td>
                      <td className="p-4 text-sm font-medium">{item.PhamNhan?.HoTen || '-'}</td>
                      <td className="p-4 text-sm">{new Date(item.NgayThamGap).toLocaleDateString('vi-VN')}</td>
                      <td className="p-4 text-sm">{item.NguoiThamGap}</td>
                      <td className="p-4 text-sm">{item.QuanHe}</td>
                      <td className="p-4 text-sm font-mono">{item.CMND}</td>
                      <td className="p-4 text-sm">{item.ThoiGianBatDau} - {item.ThoiGianKetThuc}</td>
                      <td className="p-4 text-sm max-w-[150px] truncate">{item.NoiDungTiepTe || '-'}</td>
                      {canEdit && (
                        <td className="p-4 text-sm">
                          <Button
                            variant="ghost"
                            size="sm"
                            onClick={() => handleDelete(item)}
                            className="text-destructive hover:text-destructive"
                          >
                            <Trash2 className="w-4 h-4" />
                          </Button>
                        </td>
                      )}
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>
      </main>

      <FormModal
        open={isModalOpen}
        onOpenChange={setIsModalOpen}
        title="Đăng ký thăm gặp"
        onSubmit={handleSubmit}
        isLoading={isSubmitting}
      >
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <FormField label="Phạm nhân" required className="sm:col-span-2">
            <Select
              value={formData.PhamNhanId.toString()}
              onValueChange={(value) => setFormData({ ...formData, PhamNhanId: parseInt(value) })}
            >
              <SelectTrigger>
                <SelectValue placeholder="-- Chọn phạm nhân --" />
              </SelectTrigger>
              <SelectContent>
                {phamNhanList.map((pn) => (
                  <SelectItem key={pn.Id} value={pn.Id.toString()}>
                    {pn.MaPhamNhan} - {pn.HoTen}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </FormField>
          <FormField label="Ngày thăm gặp">
            <Input
              type="date"
              value={formData.NgayThamGap}
              onChange={(e) => setFormData({ ...formData, NgayThamGap: e.target.value })}
            />
          </FormField>
          <FormField label="Quan hệ" required>
            <Select
              value={formData.QuanHe}
              onValueChange={(value) => setFormData({ ...formData, QuanHe: value })}
            >
              <SelectTrigger>
                <SelectValue placeholder="-- Chọn --" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="Vợ">Vợ</SelectItem>
                <SelectItem value="Chồng">Chồng</SelectItem>
                <SelectItem value="Cha">Cha</SelectItem>
                <SelectItem value="Mẹ">Mẹ</SelectItem>
                <SelectItem value="Con">Con</SelectItem>
                <SelectItem value="Anh/Chị/Em">Anh/Chị/Em</SelectItem>
                <SelectItem value="Họ hàng">Họ hàng</SelectItem>
                <SelectItem value="Khác">Khác</SelectItem>
              </SelectContent>
            </Select>
          </FormField>
          <FormField label="Người thăm" required>
            <Input
              value={formData.NguoiThamGap}
              onChange={(e) => setFormData({ ...formData, NguoiThamGap: e.target.value })}
              placeholder="Họ và tên người thăm"
            />
          </FormField>
          <FormField label="CMND/CCCD" required>
            <Input
              value={formData.CMND}
              onChange={(e) => setFormData({ ...formData, CMND: e.target.value })}
              placeholder="Số CMND hoặc CCCD"
            />
          </FormField>
          <FormField label="Thời gian bắt đầu">
            <Input
              type="time"
              value={formData.ThoiGianBatDau}
              onChange={(e) => setFormData({ ...formData, ThoiGianBatDau: e.target.value })}
            />
          </FormField>
          <FormField label="Thời gian kết thúc">
            <Input
              type="time"
              value={formData.ThoiGianKetThuc}
              onChange={(e) => setFormData({ ...formData, ThoiGianKetThuc: e.target.value })}
            />
          </FormField>
          <FormField label="Nội dung tiếp tế" className="sm:col-span-2">
            <Textarea
              value={formData.NoiDungTiepTe}
              onChange={(e) => setFormData({ ...formData, NoiDungTiepTe: e.target.value })}
              placeholder="Quần áo, thực phẩm, đồ dùng..."
              rows={2}
            />
          </FormField>
          <FormField label="Ghi chú" className="sm:col-span-2">
            <Textarea
              value={formData.GhiChu}
              onChange={(e) => setFormData({ ...formData, GhiChu: e.target.value })}
              placeholder="Ghi chú thêm..."
              rows={2}
            />
          </FormField>
        </div>
      </FormModal>

      <ConfirmDialog
        open={isDeleteDialogOpen}
        onOpenChange={setIsDeleteDialogOpen}
        title="Xác nhận xóa"
        description="Bạn có chắc chắn muốn xóa lịch thăm gặp này? Hành động này không thể hoàn tác."
        onConfirm={handleConfirmDelete}
        confirmLabel="Xóa"
      />
    </div>
  );
};

export default ThamGapManagement;
