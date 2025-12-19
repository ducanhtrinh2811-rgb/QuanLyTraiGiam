import React, { useState, useEffect, useMemo } from 'react';
import { PageHeader } from '@/components/PageHeader';
import { FormModal, FormField } from '@/components/FormModal';
import { ConfirmDialog } from '@/components/ConfirmDialog';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Plus, Trash2, Search, Stethoscope } from 'lucide-react';
import { sucKhoeApi, phamNhanApi } from '@/api/mockApi';
import { SucKhoe, PhamNhan } from '@/types';
import { useAuth } from '@/contexts/AuthContext';
import { toast } from 'sonner';

const SucKhoeManagement: React.FC = () => {
  const { hasPermission } = useAuth();
  const canEdit = hasPermission(['Admin', 'CanBo']);

  const [data, setData] = useState<SucKhoe[]>([]);
  const [phamNhanList, setPhamNhanList] = useState<PhamNhan[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [searchValue, setSearchValue] = useState('');
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedItem, setSelectedItem] = useState<SucKhoe | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [formData, setFormData] = useState({
    PhamNhanId: 0,
    NgayKham: '',
    LoaiKham: '' as 'DinhKy' | 'DotXuat' | 'NhapVien' | '',
    ChanDoan: '',
    DieuTri: '',
    BacSi: '',
    GhiChu: '',
  });

  const loadData = async () => {
    setIsLoading(true);
    try {
      const [skResult, pnResult] = await Promise.all([
        sucKhoeApi.getAll(),
        phamNhanApi.getAll(),
      ]);
      setData(skResult);
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
        item.ChanDoan.toLowerCase().includes(search) ||
        item.BacSi.toLowerCase().includes(search)
    );
  }, [data, searchValue]);

  const resetForm = () => {
    setFormData({
      PhamNhanId: 0,
      NgayKham: '',
      LoaiKham: '',
      ChanDoan: '',
      DieuTri: '',
      BacSi: '',
      GhiChu: '',
    });
    setSelectedItem(null);
  };

  const handleAdd = () => {
    resetForm();
    setIsModalOpen(true);
  };

  const handleDelete = (item: SucKhoe) => {
    setSelectedItem(item);
    setIsDeleteDialogOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.PhamNhanId || !formData.LoaiKham || !formData.ChanDoan || !formData.BacSi) {
      toast.error('Vui lòng nhập đầy đủ thông tin bắt buộc');
      return;
    }

    setIsSubmitting(true);
    try {
      await sucKhoeApi.create({
        PhamNhanId: formData.PhamNhanId,
        NgayKham: formData.NgayKham || new Date().toISOString().split('T')[0],
        LoaiKham: formData.LoaiKham as 'DinhKy' | 'DotXuat' | 'NhapVien',
        ChanDoan: formData.ChanDoan,
        DieuTri: formData.DieuTri,
        BacSi: formData.BacSi,
        GhiChu: formData.GhiChu,
      });
      toast.success('Thêm hồ sơ khám bệnh thành công');
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
      await sucKhoeApi.delete(selectedItem.Id);
      toast.success('Xóa thành công');
      setIsDeleteDialogOpen(false);
      setSelectedItem(null);
      loadData();
    } catch {
      toast.error('Có lỗi xảy ra');
    }
  };

  const getLoaiKhamBadge = (loai: string) => {
    switch (loai) {
      case 'DinhKy':
        return <Badge className="bg-success">Định kỳ</Badge>;
      case 'DotXuat':
        return <Badge className="bg-warning">Đột xuất</Badge>;
      case 'NhapVien':
        return <Badge variant="destructive">Nhập viện</Badge>;
      default:
        return <Badge variant="secondary">{loai}</Badge>;
    }
  };

  return (
    <div className="min-h-screen bg-background">
      <PageHeader title="Sức khỏe Phạm nhân" icon="🏥" showBackButton />

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
                Thêm hồ sơ khám
              </Button>
            )}
          </div>
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="border-b border-border bg-muted/50">
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">STT</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Phạm nhân</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Ngày khám</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Loại khám</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Chẩn đoán</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Điều trị</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Bác sĩ</th>
                  {canEdit && <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Thao tác</th>}
                </tr>
              </thead>
              <tbody>
                {isLoading ? (
                  <tr>
                    <td colSpan={8} className="text-center py-12 text-muted-foreground">
                      <div className="flex items-center justify-center gap-2">
                        <div className="w-5 h-5 border-2 border-primary border-t-transparent rounded-full animate-spin" />
                        Đang tải...
                      </div>
                    </td>
                  </tr>
                ) : filteredData.length === 0 ? (
                  <tr>
                    <td colSpan={8} className="text-center py-12 text-muted-foreground">
                      <Stethoscope className="w-12 h-12 mx-auto mb-3 opacity-30" />
                      Chưa có dữ liệu sức khỏe
                    </td>
                  </tr>
                ) : (
                  filteredData.map((item, index) => (
                    <tr key={item.Id} className="border-b border-border last:border-0 hover:bg-muted/30 transition-colors">
                      <td className="p-4 text-sm">{index + 1}</td>
                      <td className="p-4 text-sm font-medium">{item.PhamNhan?.HoTen || '-'}</td>
                      <td className="p-4 text-sm">{new Date(item.NgayKham).toLocaleDateString('vi-VN')}</td>
                      <td className="p-4 text-sm">{getLoaiKhamBadge(item.LoaiKham)}</td>
                      <td className="p-4 text-sm">{item.ChanDoan}</td>
                      <td className="p-4 text-sm max-w-[200px] truncate">{item.DieuTri}</td>
                      <td className="p-4 text-sm">{item.BacSi}</td>
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
        title="Thêm hồ sơ khám bệnh"
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
          <FormField label="Ngày khám">
            <Input
              type="date"
              value={formData.NgayKham}
              onChange={(e) => setFormData({ ...formData, NgayKham: e.target.value })}
            />
          </FormField>
          <FormField label="Loại khám" required>
            <Select
              value={formData.LoaiKham}
              onValueChange={(value) => setFormData({ ...formData, LoaiKham: value as 'DinhKy' | 'DotXuat' | 'NhapVien' })}
            >
              <SelectTrigger>
                <SelectValue placeholder="-- Chọn --" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="DinhKy">Định kỳ</SelectItem>
                <SelectItem value="DotXuat">Đột xuất</SelectItem>
                <SelectItem value="NhapVien">Nhập viện</SelectItem>
              </SelectContent>
            </Select>
          </FormField>
          <FormField label="Bác sĩ khám" required>
            <Input
              value={formData.BacSi}
              onChange={(e) => setFormData({ ...formData, BacSi: e.target.value })}
              placeholder="VD: BS. Nguyễn Văn A"
            />
          </FormField>
          <FormField label="Chẩn đoán" required className="sm:col-span-2">
            <Textarea
              value={formData.ChanDoan}
              onChange={(e) => setFormData({ ...formData, ChanDoan: e.target.value })}
              placeholder="Nhập chẩn đoán..."
              rows={2}
            />
          </FormField>
          <FormField label="Điều trị" className="sm:col-span-2">
            <Textarea
              value={formData.DieuTri}
              onChange={(e) => setFormData({ ...formData, DieuTri: e.target.value })}
              placeholder="Phương pháp điều trị, thuốc..."
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
        description="Bạn có chắc chắn muốn xóa hồ sơ khám bệnh này? Hành động này không thể hoàn tác."
        onConfirm={handleConfirmDelete}
        confirmLabel="Xóa"
      />
    </div>
  );
};

export default SucKhoeManagement;
