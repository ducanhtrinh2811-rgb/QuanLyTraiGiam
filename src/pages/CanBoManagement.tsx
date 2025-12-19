import React, { useState, useEffect, useMemo } from 'react';
import { PageHeader } from '@/components/PageHeader';
import { DataTable } from '@/components/DataTable';
import { FormModal, FormField } from '@/components/FormModal';
import { ConfirmDialog } from '@/components/ConfirmDialog';
import { Input } from '@/components/ui/input';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Button } from '@/components/ui/button';
import { Pencil, Trash2 } from 'lucide-react';
import { canBoApi } from '@/api/mockApi';
import { CanBo } from '@/types';
import { useAuth } from '@/contexts/AuthContext';
import { toast } from 'sonner';

const CanBoManagement: React.FC = () => {
  const { hasPermission } = useAuth();
  const canEdit = hasPermission(['Admin', 'CanBo']);

  const [data, setData] = useState<CanBo[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [searchValue, setSearchValue] = useState('');
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const [selectedItem, setSelectedItem] = useState<CanBo | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [formData, setFormData] = useState({
    MaCanBo: '',
    HoTen: '',
    NgaySinh: '',
    GioiTinh: '' as 'Nam' | 'Nữ' | '',
    ChucVu: '',
    PhongPhuTrach: '',
    SDT: '',
    DiaChi: '',
  });

  const loadData = async () => {
    setIsLoading(true);
    try {
      const result = await canBoApi.getAll();
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
        item.MaCanBo.toLowerCase().includes(search) ||
        item.HoTen.toLowerCase().includes(search) ||
        item.ChucVu.toLowerCase().includes(search)
    );
  }, [data, searchValue]);

  const resetForm = () => {
    setFormData({
      MaCanBo: '',
      HoTen: '',
      NgaySinh: '',
      GioiTinh: '',
      ChucVu: '',
      PhongPhuTrach: '',
      SDT: '',
      DiaChi: '',
    });
    setSelectedItem(null);
  };

  const handleAdd = () => {
    resetForm();
    setIsModalOpen(true);
  };

  const handleEdit = (item: CanBo) => {
    setSelectedItem(item);
    setFormData({
      MaCanBo: item.MaCanBo,
      HoTen: item.HoTen,
      NgaySinh: item.NgaySinh,
      GioiTinh: item.GioiTinh,
      ChucVu: item.ChucVu,
      PhongPhuTrach: item.PhongPhuTrach,
      SDT: item.SDT,
      DiaChi: item.DiaChi,
    });
    setIsModalOpen(true);
  };

  const handleDelete = (item: CanBo) => {
    setSelectedItem(item);
    setIsDeleteDialogOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.MaCanBo || !formData.HoTen || !formData.ChucVu) {
      toast.error('Vui lòng nhập đầy đủ thông tin bắt buộc');
      return;
    }

    setIsSubmitting(true);
    try {
      if (selectedItem) {
        await canBoApi.update(selectedItem.Id, formData as Omit<CanBo, 'Id'>);
        toast.success('Cập nhật thành công');
      } else {
        await canBoApi.create(formData as Omit<CanBo, 'Id'>);
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
      await canBoApi.delete(selectedItem.Id);
      toast.success('Xóa thành công');
      setIsDeleteDialogOpen(false);
      setSelectedItem(null);
      loadData();
    } catch {
      toast.error('Có lỗi xảy ra');
    }
  };

  const columns = [
    { key: 'MaCanBo', header: 'Mã cán bộ' },
    { key: 'HoTen', header: 'Họ và tên' },
    { 
      key: 'NgaySinh', 
      header: 'Ngày sinh',
      render: (item: CanBo) => item.NgaySinh ? new Date(item.NgaySinh).toLocaleDateString('vi-VN') : '-'
    },
    { key: 'GioiTinh', header: 'Giới tính' },
    { key: 'ChucVu', header: 'Chức vụ' },
    { key: 'PhongPhuTrach', header: 'Khu vực phụ trách' },
    {
      key: 'actions',
      header: 'Thao tác',
      render: (item: CanBo) => canEdit ? (
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
        title="Quản lý Cán bộ"
        icon="👮"
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
        title={selectedItem ? 'Cập nhật cán bộ' : 'Thêm mới'}
        onSubmit={handleSubmit}
        isLoading={isSubmitting}
      >
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <FormField label="Mã cán bộ" required>
            <Input
              value={formData.MaCanBo}
              onChange={(e) => setFormData({ ...formData, MaCanBo: e.target.value })}
              placeholder="VD: CB001"
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
          <FormField label="Chức vụ" required>
            <Input
              value={formData.ChucVu}
              onChange={(e) => setFormData({ ...formData, ChucVu: e.target.value })}
              placeholder="VD: Trưởng phòng"
            />
          </FormField>
          <FormField label="Khu vực phụ trách">
            <Input
              value={formData.PhongPhuTrach}
              onChange={(e) => setFormData({ ...formData, PhongPhuTrach: e.target.value })}
              placeholder="VD: Khu A"
            />
          </FormField>
          <FormField label="Số điện thoại">
            <Input
              value={formData.SDT}
              onChange={(e) => setFormData({ ...formData, SDT: e.target.value })}
              placeholder="VD: 0901234567"
            />
          </FormField>
          <FormField label="Địa chỉ" className="sm:col-span-2">
            <Input
              value={formData.DiaChi}
              onChange={(e) => setFormData({ ...formData, DiaChi: e.target.value })}
              placeholder="Nhập địa chỉ"
            />
          </FormField>
        </div>
      </FormModal>

      <ConfirmDialog
        open={isDeleteDialogOpen}
        onOpenChange={setIsDeleteDialogOpen}
        title="Xác nhận xóa"
        description={`Bạn có chắc chắn muốn xóa cán bộ "${selectedItem?.HoTen}"? Hành động này không thể hoàn tác.`}
        onConfirm={handleConfirmDelete}
        confirmLabel="Xóa"
      />
    </div>
  );
};

export default CanBoManagement;
