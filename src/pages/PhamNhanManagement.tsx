import React, { useState, useEffect, useMemo } from 'react';
import { PageHeader } from '@/components/PageHeader';
import { DataTable } from '@/components/DataTable';
import { FormModal, FormField } from '@/components/FormModal';
import { ConfirmDialog } from '@/components/ConfirmDialog';
import { Input } from '@/components/ui/input';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Pencil, Trash2, AlertTriangle } from 'lucide-react';
import { phamNhanApi, phongGiamApi } from '@/api/mockApi';
import { PhamNhan, PhongGiam } from '@/types';
import { useAuth } from '@/contexts/AuthContext';
import { toast } from 'sonner';
import { cn } from '@/lib/utils';

const PhamNhanManagement: React.FC = () => {
  const { hasPermission } = useAuth();
  const canEdit = hasPermission(['Admin', 'CanBo']);

  const [data, setData] = useState<PhamNhan[]>([]);
  const [phongGiam, setPhongGiam] = useState<PhongGiam[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [searchValue, setSearchValue] = useState('');
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedItem, setSelectedItem] = useState<PhamNhan | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [formData, setFormData] = useState({
    MaPhamNhan: '',
    HoTen: '',
    NgaySinh: '',
    GioiTinh: '' as 'Nam' | 'Nữ' | '',
    QueQuan: '',
    ToiDanh: '',
    NgayVaoTrai: '',
    PhongGiamId: 0,
    TrangThai: 'DangGiam' as 'DangGiam' | 'DaRa' | 'ChuyenTrai',
  });

  const loadData = async () => {
    setIsLoading(true);
    try {
      const [phamNhanResult, phongGiamResult] = await Promise.all([
        phamNhanApi.getAll(),
        phongGiamApi.getAll(),
      ]);
      setData(phamNhanResult);
      setPhongGiam(phongGiamResult);
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
        item.MaPhamNhan.toLowerCase().includes(search) ||
        item.HoTen.toLowerCase().includes(search) ||
        item.ToiDanh.toLowerCase().includes(search)
    );
  }, [data, searchValue]);

  const availableRooms = useMemo(() => {
    return phongGiam.filter(
      pg => pg.TrangThai === 'HoatDong' && pg.SoLuongHienTai < pg.SucChua
    );
  }, [phongGiam]);

  const resetForm = () => {
    setFormData({
      MaPhamNhan: '',
      HoTen: '',
      NgaySinh: '',
      GioiTinh: '',
      QueQuan: '',
      ToiDanh: '',
      NgayVaoTrai: '',
      PhongGiamId: 0,
      TrangThai: 'DangGiam',
    });
    setSelectedItem(null);
  };

  const handleAdd = () => {
    resetForm();
    setIsModalOpen(true);
  };

  const handleEdit = (item: PhamNhan) => {
    setSelectedItem(item);
    setFormData({
      MaPhamNhan: item.MaPhamNhan,
      HoTen: item.HoTen,
      NgaySinh: item.NgaySinh,
      GioiTinh: item.GioiTinh,
      QueQuan: item.QueQuan,
      ToiDanh: item.ToiDanh,
      NgayVaoTrai: item.NgayVaoTrai,
      PhongGiamId: item.PhongGiamId,
      TrangThai: item.TrangThai,
    });
    setIsModalOpen(true);
  };

  const handleDelete = (item: PhamNhan) => {
    setSelectedItem(item);
    setIsDeleteDialogOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.MaPhamNhan || !formData.HoTen || !formData.ToiDanh || !formData.PhongGiamId) {
      toast.error('Vui lòng nhập đầy đủ thông tin bắt buộc');
      return;
    }

    setIsSubmitting(true);
    try {
      if (selectedItem) {
        await phamNhanApi.update(selectedItem.Id, formData as Omit<PhamNhan, 'Id'>);
        toast.success('Cập nhật thành công');
      } else {
        await phamNhanApi.create(formData as Omit<PhamNhan, 'Id'>);
        toast.success('Thêm mới thành công');
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
      await phamNhanApi.delete(selectedItem.Id);
      toast.success('Xóa thành công');
      setIsDeleteDialogOpen(false);
      setSelectedItem(null);
      loadData();
    } catch {
      toast.error('Có lỗi xảy ra');
    }
  };

  const getRoomCapacityBadge = (room: PhongGiam | undefined) => {
    if (!room) return null;
    const ratio = room.SoLuongHienTai / room.SucChua;
    const isNearFull = ratio >= 0.9;
    const isFull = room.SoLuongHienTai >= room.SucChua;

    return (
      <div className="flex items-center gap-2">
        <span>{room.TenPhong}</span>
        {isFull && (
          <Badge variant="destructive" className="text-xs">
            <AlertTriangle className="w-3 h-3 mr-1" />
            Đầy
          </Badge>
        )}
        {isNearFull && !isFull && (
          <Badge variant="outline" className="text-xs border-warning text-warning">
            <AlertTriangle className="w-3 h-3 mr-1" />
            Gần đầy
          </Badge>
        )}
      </div>
    );
  };

  const columns = [
    { key: 'MaPhamNhan', header: 'Mã phạm nhân' },
    { key: 'HoTen', header: 'Họ và tên' },
    { 
      key: 'NgaySinh', 
      header: 'Ngày sinh',
      render: (item: PhamNhan) => item.NgaySinh ? new Date(item.NgaySinh).toLocaleDateString('vi-VN') : '-'
    },
    { key: 'GioiTinh', header: 'Giới tính' },
    { key: 'ToiDanh', header: 'Tội danh' },
    { 
      key: 'PhongGiam', 
      header: 'Phòng giam',
      render: (item: PhamNhan) => getRoomCapacityBadge(item.PhongGiam)
    },
    {
      key: 'TrangThai',
      header: 'Trạng thái',
      render: (item: PhamNhan) => (
        <Badge 
          variant={item.TrangThai === 'DangGiam' ? 'default' : 'secondary'}
          className={cn(
            item.TrangThai === 'DangGiam' && 'bg-primary'
          )}
        >
          {item.TrangThai === 'DangGiam' ? 'Đang giam' : 
           item.TrangThai === 'DaRa' ? 'Đã ra' : 'Chuyển trại'}
        </Badge>
      )
    },
    {
      key: 'actions',
      header: 'Thao tác',
      render: (item: PhamNhan) => canEdit ? (
        <div className="flex items-center gap-2">
          <Button
            variant="ghost"
            size="sm"
            onClick={() => handleEdit(item)}
            className="text-primary hover:text-primary"
          >
            <Pencil className="w-4 h-4" />
          </Button>
          <Button
            variant="ghost"
            size="sm"
            onClick={() => handleDelete(item)}
            className="text-destructive hover:text-destructive"
          >
            <Trash2 className="w-4 h-4" />
          </Button>
        </div>
      ) : null,
    },
  ];

  return (
    <div className="min-h-screen bg-background">
      <PageHeader
        title="Quản lý Phạm nhân"
        icon="👤"
        showBackButton
      />

      <main className="container mx-auto px-4 py-8">
        <DataTable
          data={filteredData}
          columns={columns}
          searchValue={searchValue}
          onSearchChange={setSearchValue}
          searchPlaceholder="Tìm kiếm..."
          onAdd={handleAdd}
          addLabel="Thêm mới"
          isLoading={isLoading}
          emptyMessage="Chưa có dữ liệu"
          canAdd={canEdit}
        />
      </main>

      <FormModal
        open={isModalOpen}
        onOpenChange={setIsModalOpen}
        title={selectedItem ? 'Cập nhật phạm nhân' : 'Thêm mới'}
        onSubmit={handleSubmit}
        isLoading={isSubmitting}
      >
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <FormField label="Mã phạm nhân" required>
            <Input
              value={formData.MaPhamNhan}
              onChange={(e) => setFormData({ ...formData, MaPhamNhan: e.target.value })}
              placeholder="VD: PN001"
            />
          </FormField>
          <FormField label="Họ và tên" required>
            <Input
              value={formData.HoTen}
              onChange={(e) => setFormData({ ...formData, HoTen: e.target.value })}
              placeholder="Nhập họ và tên"
            />
          </FormField>
          <FormField label="Ngày sinh">
            <Input
              type="date"
              value={formData.NgaySinh}
              onChange={(e) => setFormData({ ...formData, NgaySinh: e.target.value })}
            />
          </FormField>
          <FormField label="Giới tính">
            <Select
              value={formData.GioiTinh}
              onValueChange={(value) => setFormData({ ...formData, GioiTinh: value as 'Nam' | 'Nữ' })}
            >
              <SelectTrigger>
                <SelectValue placeholder="-- Chọn --" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="Nam">Nam</SelectItem>
                <SelectItem value="Nữ">Nữ</SelectItem>
              </SelectContent>
            </Select>
          </FormField>
          <FormField label="Quê quán">
            <Input
              value={formData.QueQuan}
              onChange={(e) => setFormData({ ...formData, QueQuan: e.target.value })}
              placeholder="VD: Hà Nội"
            />
          </FormField>
          <FormField label="Tội danh" required>
            <Input
              value={formData.ToiDanh}
              onChange={(e) => setFormData({ ...formData, ToiDanh: e.target.value })}
              placeholder="Nhập tội danh"
            />
          </FormField>
          <FormField label="Ngày vào trại">
            <Input
              type="date"
              value={formData.NgayVaoTrai}
              onChange={(e) => setFormData({ ...formData, NgayVaoTrai: e.target.value })}
            />
          </FormField>
          <FormField label="Phòng giam" required>
            <Select
              value={formData.PhongGiamId.toString()}
              onValueChange={(value) => setFormData({ ...formData, PhongGiamId: parseInt(value) })}
            >
              <SelectTrigger>
                <SelectValue placeholder="-- Chọn phòng --" />
              </SelectTrigger>
              <SelectContent>
                {(selectedItem ? phongGiam.filter(pg => pg.TrangThai === 'HoatDong') : availableRooms).map((pg) => (
                  <SelectItem key={pg.Id} value={pg.Id.toString()}>
                    {pg.TenPhong} ({pg.SoLuongHienTai}/{pg.SucChua})
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </FormField>
        </div>
      </FormModal>

      <ConfirmDialog
        open={isDeleteDialogOpen}
        onOpenChange={setIsDeleteDialogOpen}
        title="Xác nhận xóa"
        description={`Bạn có chắc chắn muốn xóa phạm nhân "${selectedItem?.HoTen}"? Hành động này không thể hoàn tác.`}
        onConfirm={handleConfirmDelete}
        confirmLabel="Xóa"
      />
    </div>
  );
};

export default PhamNhanManagement;
