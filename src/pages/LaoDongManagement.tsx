import React, { useState, useEffect, useMemo } from 'react';
import { PageHeader } from '@/components/PageHeader';
import { FormModal, FormField } from '@/components/FormModal';
import { ConfirmDialog } from '@/components/ConfirmDialog';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { Plus, Trash2, Search, Hammer, GraduationCap } from 'lucide-react';
import { laoDongApi, phamNhanApi } from '@/api/mockApi';
import { LaoDong, PhamNhan } from '@/types';
import { useAuth } from '@/contexts/AuthContext';
import { toast } from 'sonner';

const LaoDongManagement: React.FC = () => {
  const { hasPermission } = useAuth();
  const canEdit = hasPermission(['Admin', 'CanBo']);

  const [data, setData] = useState<LaoDong[]>([]);
  const [phamNhanList, setPhamNhanList] = useState<PhamNhan[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [searchValue, setSearchValue] = useState('');
  const [activeTab, setActiveTab] = useState('lao-dong');
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedItem, setSelectedItem] = useState<LaoDong | null>(null);
  const [modalType, setModalType] = useState<'LaoDong' | 'HocTap'>('LaoDong');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [formData, setFormData] = useState({
    PhamNhanId: 0,
    LoaiHoatDong: '' as 'LaoDong' | 'HocTap' | '',
    TenHoatDong: '',
    NgayBatDau: '',
    NgayKetThuc: '',
    KetQua: '',
    DanhGia: '' as 'Tot' | 'Kha' | 'TrungBinh' | 'Yeu' | '',
    GhiChu: '',
  });

  const loadData = async () => {
    setIsLoading(true);
    try {
      const [ldResult, pnResult] = await Promise.all([
        laoDongApi.getAll(),
        phamNhanApi.getAll(),
      ]);
      setData(ldResult);
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

  const laoDongData = useMemo(() => {
    const filtered = data.filter(d => d.LoaiHoatDong === 'LaoDong');
    if (!searchValue) return filtered;
    const search = searchValue.toLowerCase();
    return filtered.filter(
      item =>
        item.PhamNhan?.HoTen.toLowerCase().includes(search) ||
        item.TenHoatDong.toLowerCase().includes(search)
    );
  }, [data, searchValue]);

  const hocTapData = useMemo(() => {
    const filtered = data.filter(d => d.LoaiHoatDong === 'HocTap');
    if (!searchValue) return filtered;
    const search = searchValue.toLowerCase();
    return filtered.filter(
      item =>
        item.PhamNhan?.HoTen.toLowerCase().includes(search) ||
        item.TenHoatDong.toLowerCase().includes(search)
    );
  }, [data, searchValue]);

  const resetForm = () => {
    setFormData({
      PhamNhanId: 0,
      LoaiHoatDong: '',
      TenHoatDong: '',
      NgayBatDau: '',
      NgayKetThuc: '',
      KetQua: '',
      DanhGia: '',
      GhiChu: '',
    });
    setSelectedItem(null);
  };

  const handleAddLaoDong = () => {
    resetForm();
    setModalType('LaoDong');
    setFormData(prev => ({ ...prev, LoaiHoatDong: 'LaoDong' }));
    setIsModalOpen(true);
  };

  const handleAddHocTap = () => {
    resetForm();
    setModalType('HocTap');
    setFormData(prev => ({ ...prev, LoaiHoatDong: 'HocTap' }));
    setIsModalOpen(true);
  };

  const handleDelete = (item: LaoDong) => {
    setSelectedItem(item);
    setIsDeleteDialogOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.PhamNhanId || !formData.TenHoatDong || !formData.NgayBatDau) {
      toast.error('Vui lòng nhập đầy đủ thông tin bắt buộc');
      return;
    }

    setIsSubmitting(true);
    try {
      await laoDongApi.create({
        PhamNhanId: formData.PhamNhanId,
        LoaiHoatDong: modalType,
        TenHoatDong: formData.TenHoatDong,
        NgayBatDau: formData.NgayBatDau,
        NgayKetThuc: formData.NgayKetThuc || undefined,
        KetQua: formData.KetQua || undefined,
        DanhGia: formData.DanhGia as 'Tot' | 'Kha' | 'TrungBinh' | 'Yeu' || undefined,
        GhiChu: formData.GhiChu || undefined,
      });
      toast.success(modalType === 'LaoDong' ? 'Thêm hoạt động lao động thành công' : 'Thêm hoạt động học tập thành công');
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
      await laoDongApi.delete(selectedItem.Id);
      toast.success('Xóa thành công');
      setIsDeleteDialogOpen(false);
      setSelectedItem(null);
      loadData();
    } catch {
      toast.error('Có lỗi xảy ra');
    }
  };

  const getDanhGiaBadge = (danhGia?: string) => {
    switch (danhGia) {
      case 'Tot':
        return <Badge className="bg-success">Tốt</Badge>;
      case 'Kha':
        return <Badge className="bg-primary">Khá</Badge>;
      case 'TrungBinh':
        return <Badge className="bg-warning">Trung bình</Badge>;
      case 'Yeu':
        return <Badge variant="destructive">Yếu</Badge>;
      default:
        return <Badge variant="secondary">Chưa đánh giá</Badge>;
    }
  };

  const renderTable = (tableData: LaoDong[], type: 'LaoDong' | 'HocTap') => (
    <div className="bg-card rounded-xl border border-border shadow-sm">
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
          <Button onClick={type === 'LaoDong' ? handleAddLaoDong : handleAddHocTap} className="gap-2">
            <Plus className="w-4 h-4" />
            {type === 'LaoDong' ? 'Thêm lao động' : 'Thêm học tập'}
          </Button>
        )}
      </div>
      <div className="overflow-x-auto">
        <table className="w-full">
          <thead>
            <tr className="border-b border-border bg-muted/50">
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">STT</th>
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Phạm nhân</th>
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">{type === 'LaoDong' ? 'Công việc' : 'Khóa học'}</th>
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Ngày bắt đầu</th>
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Ngày kết thúc</th>
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Kết quả</th>
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Đánh giá</th>
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
            ) : tableData.length === 0 ? (
              <tr>
                <td colSpan={8} className="text-center py-12 text-muted-foreground">
                  {type === 'LaoDong' ? (
                    <Hammer className="w-12 h-12 mx-auto mb-3 opacity-30" />
                  ) : (
                    <GraduationCap className="w-12 h-12 mx-auto mb-3 opacity-30" />
                  )}
                  Chưa có dữ liệu {type === 'LaoDong' ? 'lao động' : 'học tập'}
                </td>
              </tr>
            ) : (
              tableData.map((item, index) => (
                <tr key={item.Id} className="border-b border-border last:border-0 hover:bg-muted/30 transition-colors">
                  <td className="p-4 text-sm">{index + 1}</td>
                  <td className="p-4 text-sm font-medium">{item.PhamNhan?.HoTen || '-'}</td>
                  <td className="p-4 text-sm">{item.TenHoatDong}</td>
                  <td className="p-4 text-sm">{new Date(item.NgayBatDau).toLocaleDateString('vi-VN')}</td>
                  <td className="p-4 text-sm">{item.NgayKetThuc ? new Date(item.NgayKetThuc).toLocaleDateString('vi-VN') : 'Đang tiếp tục'}</td>
                  <td className="p-4 text-sm max-w-[150px] truncate">{item.KetQua || '-'}</td>
                  <td className="p-4 text-sm">{getDanhGiaBadge(item.DanhGia)}</td>
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
  );

  return (
    <div className="min-h-screen bg-background">
      <PageHeader title="Lao động - Học tập" icon="🎓" showBackButton />

      <main className="container mx-auto px-4 py-8">
        <Tabs value={activeTab} onValueChange={setActiveTab} className="animate-slide-up">
          <TabsList className="mb-6">
            <TabsTrigger value="lao-dong" className="gap-2">
              <Hammer className="w-4 h-4" />
              Lao động ({laoDongData.length})
            </TabsTrigger>
            <TabsTrigger value="hoc-tap" className="gap-2">
              <GraduationCap className="w-4 h-4" />
              Học tập ({hocTapData.length})
            </TabsTrigger>
          </TabsList>
          
          <TabsContent value="lao-dong">
            {renderTable(laoDongData, 'LaoDong')}
          </TabsContent>
          
          <TabsContent value="hoc-tap">
            {renderTable(hocTapData, 'HocTap')}
          </TabsContent>
        </Tabs>
      </main>

      <FormModal
        open={isModalOpen}
        onOpenChange={setIsModalOpen}
        title={modalType === 'LaoDong' ? 'Thêm hoạt động lao động' : 'Thêm hoạt động học tập'}
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
          <FormField label={modalType === 'LaoDong' ? 'Tên công việc' : 'Tên khóa học'} required className="sm:col-span-2">
            <Input
              value={formData.TenHoatDong}
              onChange={(e) => setFormData({ ...formData, TenHoatDong: e.target.value })}
              placeholder={modalType === 'LaoDong' ? 'VD: Làm mộc, trồng rau...' : 'VD: Học văn hóa lớp 9...'}
            />
          </FormField>
          <FormField label="Ngày bắt đầu" required>
            <Input
              type="date"
              value={formData.NgayBatDau}
              onChange={(e) => setFormData({ ...formData, NgayBatDau: e.target.value })}
            />
          </FormField>
          <FormField label="Ngày kết thúc">
            <Input
              type="date"
              value={formData.NgayKetThuc}
              onChange={(e) => setFormData({ ...formData, NgayKetThuc: e.target.value })}
            />
          </FormField>
          <FormField label="Đánh giá">
            <Select
              value={formData.DanhGia}
              onValueChange={(value) => setFormData({ ...formData, DanhGia: value as 'Tot' | 'Kha' | 'TrungBinh' | 'Yeu' })}
            >
              <SelectTrigger>
                <SelectValue placeholder="-- Chọn --" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="Tot">Tốt</SelectItem>
                <SelectItem value="Kha">Khá</SelectItem>
                <SelectItem value="TrungBinh">Trung bình</SelectItem>
                <SelectItem value="Yeu">Yếu</SelectItem>
              </SelectContent>
            </Select>
          </FormField>
          <FormField label="Kết quả">
            <Input
              value={formData.KetQua}
              onChange={(e) => setFormData({ ...formData, KetQua: e.target.value })}
              placeholder="Mô tả kết quả..."
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
        description="Bạn có chắc chắn muốn xóa hoạt động này? Hành động này không thể hoàn tác."
        onConfirm={handleConfirmDelete}
        confirmLabel="Xóa"
      />
    </div>
  );
};

export default LaoDongManagement;
