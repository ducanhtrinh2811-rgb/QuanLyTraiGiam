import React, { useState, useEffect, useMemo } from 'react';
import { PageHeader } from '@/components/PageHeader';
import { FormModal, FormField } from '@/components/FormModal';
import { ConfirmDialog } from '@/components/ConfirmDialog';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Plus, Trash2, Search, ShieldAlert, CheckCircle, Clock } from 'lucide-react';
import { suCoApi } from '@/api/mockApi';
import { SuCo } from '@/types';
import { useAuth } from '@/contexts/AuthContext';
import { toast } from 'sonner';
import { cn } from '@/lib/utils';

const SuCoManagement: React.FC = () => {
  const { hasPermission } = useAuth();
  const canEdit = hasPermission(['Admin', 'CanBo']);

  const [data, setData] = useState<SuCo[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [searchValue, setSearchValue] = useState('');
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedItem, setSelectedItem] = useState<SuCo | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [formData, setFormData] = useState({
    NgayXayRa: '',
    LoaiSuCo: '' as 'AnNinh' | 'YTe' | 'ChayNo' | 'TronTrai' | 'Khac' | '',
    MoTa: '',
    MucDo: '' as 'NghiemTrong' | 'Vua' | 'Nhe' | '',
    PhamNhanLienQuan: '',
    BienPhapXuLy: '',
    NguoiBaoCao: '',
    TrangThai: 'DangXuLy' as 'DangXuLy' | 'DaXuLy' | 'TheoDoi',
    GhiChu: '',
  });

  const loadData = async () => {
    setIsLoading(true);
    try {
      const result = await suCoApi.getAll();
      setData(result);
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
        item.MoTa.toLowerCase().includes(search) ||
        item.NguoiBaoCao.toLowerCase().includes(search) ||
        (item.PhamNhanLienQuan?.toLowerCase().includes(search))
    );
  }, [data, searchValue]);

  const resetForm = () => {
    setFormData({
      NgayXayRa: '',
      LoaiSuCo: '',
      MoTa: '',
      MucDo: '',
      PhamNhanLienQuan: '',
      BienPhapXuLy: '',
      NguoiBaoCao: '',
      TrangThai: 'DangXuLy',
      GhiChu: '',
    });
    setSelectedItem(null);
  };

  const handleAdd = () => {
    resetForm();
    setIsModalOpen(true);
  };

  const handleDelete = (item: SuCo) => {
    setSelectedItem(item);
    setIsDeleteDialogOpen(true);
  };

  const handleUpdateStatus = async (item: SuCo, newStatus: 'DangXuLy' | 'DaXuLy' | 'TheoDoi') => {
    try {
      await suCoApi.update(item.Id, { TrangThai: newStatus });
      toast.success('Cập nhật trạng thái thành công');
      loadData();
    } catch {
      toast.error('Có lỗi xảy ra');
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.LoaiSuCo || !formData.MoTa || !formData.MucDo || !formData.BienPhapXuLy || !formData.NguoiBaoCao) {
      toast.error('Vui lòng nhập đầy đủ thông tin bắt buộc');
      return;
    }

    setIsSubmitting(true);
    try {
      await suCoApi.create({
        NgayXayRa: formData.NgayXayRa || new Date().toISOString().split('T')[0],
        LoaiSuCo: formData.LoaiSuCo as 'AnNinh' | 'YTe' | 'ChayNo' | 'TronTrai' | 'Khac',
        MoTa: formData.MoTa,
        MucDo: formData.MucDo as 'NghiemTrong' | 'Vua' | 'Nhe',
        PhamNhanLienQuan: formData.PhamNhanLienQuan,
        BienPhapXuLy: formData.BienPhapXuLy,
        NguoiBaoCao: formData.NguoiBaoCao,
        TrangThai: formData.TrangThai,
        GhiChu: formData.GhiChu,
      });
      toast.success('Báo cáo sự cố thành công');
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
      await suCoApi.delete(selectedItem.Id);
      toast.success('Xóa thành công');
      setIsDeleteDialogOpen(false);
      setSelectedItem(null);
      loadData();
    } catch {
      toast.error('Có lỗi xảy ra');
    }
  };

  const getLoaiSuCoBadge = (loai: string) => {
    const loaiMap: Record<string, { label: string; className: string }> = {
      AnNinh: { label: 'An ninh', className: 'bg-destructive' },
      YTe: { label: 'Y tế', className: 'bg-warning' },
      ChayNo: { label: 'Cháy nổ', className: 'bg-destructive' },
      TronTrai: { label: 'Trốn trại', className: 'bg-destructive' },
      Khac: { label: 'Khác', className: 'bg-muted' },
    };
    const info = loaiMap[loai] || { label: loai, className: 'bg-muted' };
    return <Badge className={info.className}>{info.label}</Badge>;
  };

  const getMucDoBadge = (mucDo: string) => {
    switch (mucDo) {
      case 'NghiemTrong':
        return <Badge variant="destructive">Nghiêm trọng</Badge>;
      case 'Vua':
        return <Badge className="bg-warning">Vừa</Badge>;
      case 'Nhe':
        return <Badge className="bg-success">Nhẹ</Badge>;
      default:
        return <Badge variant="secondary">{mucDo}</Badge>;
    }
  };

  const getTrangThaiBadge = (trangThai: string) => {
    switch (trangThai) {
      case 'DaXuLy':
        return (
          <Badge className="bg-success gap-1">
            <CheckCircle className="w-3 h-3" /> Đã xử lý
          </Badge>
        );
      case 'DangXuLy':
        return (
          <Badge className="bg-warning gap-1">
            <Clock className="w-3 h-3" /> Đang xử lý
          </Badge>
        );
      case 'TheoDoi':
        return (
          <Badge className="bg-primary gap-1">
            <ShieldAlert className="w-3 h-3" /> Theo dõi
          </Badge>
        );
      default:
        return <Badge variant="secondary">{trangThai}</Badge>;
    }
  };

  return (
    <div className="min-h-screen bg-background">
      <PageHeader title="Sự cố - An ninh" icon="⚠️" showBackButton />

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
              <Button onClick={handleAdd} className="gap-2 bg-destructive hover:bg-destructive/90">
                <Plus className="w-4 h-4" />
                Báo cáo sự cố
              </Button>
            )}
          </div>
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="border-b border-border bg-muted/50">
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">STT</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Ngày</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Loại</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Mức độ</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Mô tả</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Biện pháp</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Trạng thái</th>
                  <th className="text-left p-4 font-semibold text-sm text-muted-foreground">Người báo cáo</th>
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
                      <ShieldAlert className="w-12 h-12 mx-auto mb-3 opacity-30" />
                      Chưa có sự cố nào được ghi nhận
                    </td>
                  </tr>
                ) : (
                  filteredData.map((item, index) => (
                    <tr key={item.Id} className="border-b border-border last:border-0 hover:bg-muted/30 transition-colors">
                      <td className="p-4 text-sm">{index + 1}</td>
                      <td className="p-4 text-sm">{new Date(item.NgayXayRa).toLocaleDateString('vi-VN')}</td>
                      <td className="p-4 text-sm">{getLoaiSuCoBadge(item.LoaiSuCo)}</td>
                      <td className="p-4 text-sm">{getMucDoBadge(item.MucDo)}</td>
                      <td className="p-4 text-sm max-w-[200px] truncate">{item.MoTa}</td>
                      <td className="p-4 text-sm max-w-[150px] truncate">{item.BienPhapXuLy}</td>
                      <td className="p-4 text-sm">
                        {canEdit ? (
                          <Select
                            value={item.TrangThai}
                            onValueChange={(value) => handleUpdateStatus(item, value as 'DangXuLy' | 'DaXuLy' | 'TheoDoi')}
                          >
                            <SelectTrigger className="w-[140px] h-8">
                              <SelectValue />
                            </SelectTrigger>
                            <SelectContent>
                              <SelectItem value="DangXuLy">Đang xử lý</SelectItem>
                              <SelectItem value="DaXuLy">Đã xử lý</SelectItem>
                              <SelectItem value="TheoDoi">Theo dõi</SelectItem>
                            </SelectContent>
                          </Select>
                        ) : (
                          getTrangThaiBadge(item.TrangThai)
                        )}
                      </td>
                      <td className="p-4 text-sm">{item.NguoiBaoCao}</td>
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
        title="Báo cáo sự cố"
        onSubmit={handleSubmit}
        isLoading={isSubmitting}
      >
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <FormField label="Ngày xảy ra">
            <Input
              type="date"
              value={formData.NgayXayRa}
              onChange={(e) => setFormData({ ...formData, NgayXayRa: e.target.value })}
            />
          </FormField>
          <FormField label="Loại sự cố" required>
            <Select
              value={formData.LoaiSuCo}
              onValueChange={(value) => setFormData({ ...formData, LoaiSuCo: value as 'AnNinh' | 'YTe' | 'ChayNo' | 'TronTrai' | 'Khac' })}
            >
              <SelectTrigger>
                <SelectValue placeholder="-- Chọn --" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="AnNinh">An ninh</SelectItem>
                <SelectItem value="YTe">Y tế</SelectItem>
                <SelectItem value="ChayNo">Cháy nổ</SelectItem>
                <SelectItem value="TronTrai">Trốn trại</SelectItem>
                <SelectItem value="Khac">Khác</SelectItem>
              </SelectContent>
            </Select>
          </FormField>
          <FormField label="Mức độ" required>
            <Select
              value={formData.MucDo}
              onValueChange={(value) => setFormData({ ...formData, MucDo: value as 'NghiemTrong' | 'Vua' | 'Nhe' })}
            >
              <SelectTrigger>
                <SelectValue placeholder="-- Chọn --" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="NghiemTrong">Nghiêm trọng</SelectItem>
                <SelectItem value="Vua">Vừa</SelectItem>
                <SelectItem value="Nhe">Nhẹ</SelectItem>
              </SelectContent>
            </Select>
          </FormField>
          <FormField label="Người báo cáo" required>
            <Input
              value={formData.NguoiBaoCao}
              onChange={(e) => setFormData({ ...formData, NguoiBaoCao: e.target.value })}
              placeholder="Tên người báo cáo"
            />
          </FormField>
          <FormField label="Phạm nhân liên quan">
            <Input
              value={formData.PhamNhanLienQuan}
              onChange={(e) => setFormData({ ...formData, PhamNhanLienQuan: e.target.value })}
              placeholder="VD: PN001, PN002"
            />
          </FormField>
          <FormField label="Trạng thái">
            <Select
              value={formData.TrangThai}
              onValueChange={(value) => setFormData({ ...formData, TrangThai: value as 'DangXuLy' | 'DaXuLy' | 'TheoDoi' })}
            >
              <SelectTrigger>
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="DangXuLy">Đang xử lý</SelectItem>
                <SelectItem value="DaXuLy">Đã xử lý</SelectItem>
                <SelectItem value="TheoDoi">Theo dõi</SelectItem>
              </SelectContent>
            </Select>
          </FormField>
          <FormField label="Mô tả sự cố" required className="sm:col-span-2">
            <Textarea
              value={formData.MoTa}
              onChange={(e) => setFormData({ ...formData, MoTa: e.target.value })}
              placeholder="Mô tả chi tiết sự cố..."
              rows={3}
            />
          </FormField>
          <FormField label="Biện pháp xử lý" required className="sm:col-span-2">
            <Textarea
              value={formData.BienPhapXuLy}
              onChange={(e) => setFormData({ ...formData, BienPhapXuLy: e.target.value })}
              placeholder="Các biện pháp đã và đang thực hiện..."
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
        description="Bạn có chắc chắn muốn xóa báo cáo sự cố này? Hành động này không thể hoàn tác."
        onConfirm={handleConfirmDelete}
        confirmLabel="Xóa"
      />
    </div>
  );
};

export default SuCoManagement;
