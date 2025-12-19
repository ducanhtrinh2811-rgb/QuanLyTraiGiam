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
import { Plus, Trash2, Award, AlertCircle, Search } from 'lucide-react';
import { khenThuongApi, kyLuatApi, phamNhanApi } from '@/api/mockApi';
import { KhenThuong, KyLuat, PhamNhan } from '@/types';
import { useAuth } from '@/contexts/AuthContext';
import { toast } from 'sonner';
import { cn } from '@/lib/utils';

const KhenThuongKyLuatManagement: React.FC = () => {
  const { hasPermission } = useAuth();
  const canEdit = hasPermission(['Admin', 'CanBo']);

  const [khenThuongData, setKhenThuongData] = useState<KhenThuong[]>([]);
  const [kyLuatData, setKyLuatData] = useState<KyLuat[]>([]);
  const [phamNhanList, setPhamNhanList] = useState<PhamNhan[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [searchValue, setSearchValue] = useState('');
  const [activeTab, setActiveTab] = useState('khen-thuong');
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedItem, setSelectedItem] = useState<KhenThuong | KyLuat | null>(null);
  const [modalType, setModalType] = useState<'khenThuong' | 'kyLuat'>('khenThuong');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [formData, setFormData] = useState({
    PhamNhanId: 0,
    NgayKhenThuong: '',
    NgayKyLuat: '',
    LyDo: '',
    HinhThuc: '',
    NguoiKy: '',
    ThoiHan: '',
    GhiChu: '',
  });

  const loadData = async () => {
    setIsLoading(true);
    try {
      const [kt, kl, pn] = await Promise.all([
        khenThuongApi.getAll(),
        kyLuatApi.getAll(),
        phamNhanApi.getAll(),
      ]);
      setKhenThuongData(kt);
      setKyLuatData(kl);
      setPhamNhanList(pn.filter(p => p.TrangThai === 'DangGiam'));
    } catch {
      toast.error('Không thể tải dữ liệu');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const filteredKhenThuong = useMemo(() => {
    if (!searchValue) return khenThuongData;
    const search = searchValue.toLowerCase();
    return khenThuongData.filter(
      item =>
        item.PhamNhan?.HoTen.toLowerCase().includes(search) ||
        item.LyDo.toLowerCase().includes(search)
    );
  }, [khenThuongData, searchValue]);

  const filteredKyLuat = useMemo(() => {
    if (!searchValue) return kyLuatData;
    const search = searchValue.toLowerCase();
    return kyLuatData.filter(
      item =>
        item.PhamNhan?.HoTen.toLowerCase().includes(search) ||
        item.LyDo.toLowerCase().includes(search)
    );
  }, [kyLuatData, searchValue]);

  const resetForm = () => {
    setFormData({
      PhamNhanId: 0,
      NgayKhenThuong: '',
      NgayKyLuat: '',
      LyDo: '',
      HinhThuc: '',
      NguoiKy: '',
      ThoiHan: '',
      GhiChu: '',
    });
    setSelectedItem(null);
  };

  const handleAddKhenThuong = () => {
    resetForm();
    setModalType('khenThuong');
    setIsModalOpen(true);
  };

  const handleAddKyLuat = () => {
    resetForm();
    setModalType('kyLuat');
    setIsModalOpen(true);
  };

  const handleDeleteKhenThuong = (item: KhenThuong) => {
    setSelectedItem(item);
    setModalType('khenThuong');
    setIsDeleteDialogOpen(true);
  };

  const handleDeleteKyLuat = (item: KyLuat) => {
    setSelectedItem(item);
    setModalType('kyLuat');
    setIsDeleteDialogOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.PhamNhanId || !formData.LyDo || !formData.HinhThuc || !formData.NguoiKy) {
      toast.error('Vui lòng nhập đầy đủ thông tin bắt buộc');
      return;
    }

    setIsSubmitting(true);
    try {
      if (modalType === 'khenThuong') {
        await khenThuongApi.create({
          PhamNhanId: formData.PhamNhanId,
          NgayKhenThuong: formData.NgayKhenThuong || new Date().toISOString().split('T')[0],
          LyDo: formData.LyDo,
          HinhThuc: formData.HinhThuc,
          NguoiKy: formData.NguoiKy,
          GhiChu: formData.GhiChu,
        });
        toast.success('Thêm khen thưởng thành công');
      } else {
        await kyLuatApi.create({
          PhamNhanId: formData.PhamNhanId,
          NgayKyLuat: formData.NgayKyLuat || new Date().toISOString().split('T')[0],
          LyDo: formData.LyDo,
          HinhThuc: formData.HinhThuc,
          ThoiHan: formData.ThoiHan,
          NguoiKy: formData.NguoiKy,
          GhiChu: formData.GhiChu,
        });
        toast.success('Thêm kỷ luật thành công');
      }
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
      if (modalType === 'khenThuong') {
        await khenThuongApi.delete(selectedItem.Id);
        toast.success('Xóa khen thưởng thành công');
      } else {
        await kyLuatApi.delete(selectedItem.Id);
        toast.success('Xóa kỷ luật thành công');
      }
      setIsDeleteDialogOpen(false);
      setSelectedItem(null);
      loadData();
    } catch {
      toast.error('Có lỗi xảy ra');
    }
  };

  const renderKhenThuongTable = () => (
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
          <Button onClick={handleAddKhenThuong} className="gap-2">
            <Plus className="w-4 h-4" />
            Thêm khen thưởng
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
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Lý do</th>
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Hình thức</th>
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Người ký</th>
              {canEdit && <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Thao tác</th>}
            </tr>
          </thead>
          <tbody>
            {isLoading ? (
              <tr>
                <td colSpan={7} className="text-center py-12 text-muted-foreground">
                  <div className="flex items-center justify-center gap-2">
                    <div className="w-5 h-5 border-2 border-primary border-t-transparent rounded-full animate-spin" />
                    Đang tải...
                  </div>
                </td>
              </tr>
            ) : filteredKhenThuong.length === 0 ? (
              <tr>
                <td colSpan={7} className="text-center py-12 text-muted-foreground">
                  Chưa có dữ liệu khen thưởng
                </td>
              </tr>
            ) : (
              filteredKhenThuong.map((item, index) => (
                <tr key={item.Id} className="border-b border-border last:border-0 hover:bg-muted/30 transition-colors">
                  <td className="p-4 text-sm">{index + 1}</td>
                  <td className="p-4 text-sm font-medium">{item.PhamNhan?.HoTen || '-'}</td>
                  <td className="p-4 text-sm">{new Date(item.NgayKhenThuong).toLocaleDateString('vi-VN')}</td>
                  <td className="p-4 text-sm">{item.LyDo}</td>
                  <td className="p-4 text-sm">
                    <Badge className="bg-success text-success-foreground">{item.HinhThuc}</Badge>
                  </td>
                  <td className="p-4 text-sm">{item.NguoiKy}</td>
                  {canEdit && (
                    <td className="p-4 text-sm">
                      <Button
                        variant="ghost"
                        size="sm"
                        onClick={() => handleDeleteKhenThuong(item)}
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

  const renderKyLuatTable = () => (
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
          <Button onClick={handleAddKyLuat} className="gap-2 bg-destructive hover:bg-destructive/90">
            <Plus className="w-4 h-4" />
            Thêm kỷ luật
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
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Lý do</th>
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Hình thức</th>
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Thời hạn</th>
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Người ký</th>
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
            ) : filteredKyLuat.length === 0 ? (
              <tr>
                <td colSpan={8} className="text-center py-12 text-muted-foreground">
                  Chưa có dữ liệu kỷ luật
                </td>
              </tr>
            ) : (
              filteredKyLuat.map((item, index) => (
                <tr key={item.Id} className="border-b border-border last:border-0 hover:bg-muted/30 transition-colors">
                  <td className="p-4 text-sm">{index + 1}</td>
                  <td className="p-4 text-sm font-medium">{item.PhamNhan?.HoTen || '-'}</td>
                  <td className="p-4 text-sm">{new Date(item.NgayKyLuat).toLocaleDateString('vi-VN')}</td>
                  <td className="p-4 text-sm">{item.LyDo}</td>
                  <td className="p-4 text-sm">
                    <Badge variant="destructive">{item.HinhThuc}</Badge>
                  </td>
                  <td className="p-4 text-sm">{item.ThoiHan || '-'}</td>
                  <td className="p-4 text-sm">{item.NguoiKy}</td>
                  {canEdit && (
                    <td className="p-4 text-sm">
                      <Button
                        variant="ghost"
                        size="sm"
                        onClick={() => handleDeleteKyLuat(item)}
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
      <PageHeader
        title="Khen thưởng - Kỷ luật"
        icon="🏆"
        showBackButton
      />

      <main className="container mx-auto px-4 py-8">
        <Tabs value={activeTab} onValueChange={setActiveTab} className="animate-slide-up">
          <TabsList className="mb-6">
            <TabsTrigger value="khen-thuong" className="gap-2">
              <Award className="w-4 h-4" />
              Khen thưởng ({khenThuongData.length})
            </TabsTrigger>
            <TabsTrigger value="ky-luat" className="gap-2">
              <AlertCircle className="w-4 h-4" />
              Kỷ luật ({kyLuatData.length})
            </TabsTrigger>
          </TabsList>
          
          <TabsContent value="khen-thuong">
            {renderKhenThuongTable()}
          </TabsContent>
          
          <TabsContent value="ky-luat">
            {renderKyLuatTable()}
          </TabsContent>
        </Tabs>
      </main>

      <FormModal
        open={isModalOpen}
        onOpenChange={setIsModalOpen}
        title={modalType === 'khenThuong' ? 'Thêm khen thưởng' : 'Thêm kỷ luật'}
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
          <FormField label={modalType === 'khenThuong' ? 'Ngày khen thưởng' : 'Ngày kỷ luật'}>
            <Input
              type="date"
              value={modalType === 'khenThuong' ? formData.NgayKhenThuong : formData.NgayKyLuat}
              onChange={(e) => setFormData({ 
                ...formData, 
                [modalType === 'khenThuong' ? 'NgayKhenThuong' : 'NgayKyLuat']: e.target.value 
              })}
            />
          </FormField>
          <FormField label="Hình thức" required>
            <Select
              value={formData.HinhThuc}
              onValueChange={(value) => setFormData({ ...formData, HinhThuc: value })}
            >
              <SelectTrigger>
                <SelectValue placeholder="-- Chọn --" />
              </SelectTrigger>
              <SelectContent>
                {modalType === 'khenThuong' ? (
                  <>
                    <SelectItem value="Giấy khen">Giấy khen</SelectItem>
                    <SelectItem value="Bằng khen">Bằng khen</SelectItem>
                    <SelectItem value="Giảm án">Giảm án</SelectItem>
                  </>
                ) : (
                  <>
                    <SelectItem value="Cảnh cáo">Cảnh cáo</SelectItem>
                    <SelectItem value="Khiển trách">Khiển trách</SelectItem>
                    <SelectItem value="Biệt giam">Biệt giam</SelectItem>
                    <SelectItem value="Tước quyền lợi">Tước quyền lợi</SelectItem>
                  </>
                )}
              </SelectContent>
            </Select>
          </FormField>
          {modalType === 'kyLuat' && (
            <FormField label="Thời hạn">
              <Input
                value={formData.ThoiHan}
                onChange={(e) => setFormData({ ...formData, ThoiHan: e.target.value })}
                placeholder="VD: 7 ngày"
              />
            </FormField>
          )}
          <FormField label="Người ký" required>
            <Input
              value={formData.NguoiKy}
              onChange={(e) => setFormData({ ...formData, NguoiKy: e.target.value })}
              placeholder="Tên người ký quyết định"
            />
          </FormField>
          <FormField label="Lý do" required className="sm:col-span-2">
            <Textarea
              value={formData.LyDo}
              onChange={(e) => setFormData({ ...formData, LyDo: e.target.value })}
              placeholder="Nhập lý do..."
              rows={3}
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
        description={`Bạn có chắc chắn muốn xóa bản ghi ${modalType === 'khenThuong' ? 'khen thưởng' : 'kỷ luật'} này? Hành động này không thể hoàn tác.`}
        onConfirm={handleConfirmDelete}
        confirmLabel="Xóa"
      />
    </div>
  );
};

export default KhenThuongKyLuatManagement;
