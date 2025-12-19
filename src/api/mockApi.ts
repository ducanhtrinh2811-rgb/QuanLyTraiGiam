// Mock API endpoints - Ready to connect to SQL Server backend
// These functions simulate REST API calls

import { 
  CanBo, 
  PhamNhan, 
  KhenThuong, 
  KyLuat, 
  ThongKePhongGiam,
  PhongGiam 
} from '@/types';
import { 
  mockCanBo, 
  mockPhamNhan, 
  mockKhenThuong, 
  mockKyLuat, 
  mockPhongGiam 
} from '@/data/mockData';

// Simulate network delay
const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

// State for mock data (simulating database)
let canBoData = [...mockCanBo];
let phamNhanData = [...mockPhamNhan];
let khenThuongData = [...mockKhenThuong];
let kyLuatData = [...mockKyLuat];
let phongGiamData = [...mockPhongGiam];

// ============== CAN BO API ==============
export const canBoApi = {
  getAll: async (): Promise<CanBo[]> => {
    await delay(300);
    return [...canBoData];
  },

  getById: async (id: number): Promise<CanBo | undefined> => {
    await delay(200);
    return canBoData.find(cb => cb.Id === id);
  },

  create: async (canBo: Omit<CanBo, 'Id'>): Promise<CanBo> => {
    await delay(300);
    const newCanBo: CanBo = {
      ...canBo,
      Id: Math.max(...canBoData.map(cb => cb.Id)) + 1,
    };
    canBoData.push(newCanBo);
    return newCanBo;
  },

  update: async (id: number, canBo: Partial<CanBo>): Promise<CanBo | undefined> => {
    await delay(300);
    const index = canBoData.findIndex(cb => cb.Id === id);
    if (index === -1) return undefined;
    canBoData[index] = { ...canBoData[index], ...canBo };
    return canBoData[index];
  },

  delete: async (id: number): Promise<boolean> => {
    await delay(300);
    const index = canBoData.findIndex(cb => cb.Id === id);
    if (index === -1) return false;
    canBoData.splice(index, 1);
    return true;
  },
};

// ============== PHAM NHAN API ==============
export const phamNhanApi = {
  getAll: async (): Promise<PhamNhan[]> => {
    await delay(300);
    return phamNhanData.map(pn => ({
      ...pn,
      PhongGiam: phongGiamData.find(pg => pg.Id === pn.PhongGiamId),
    }));
  },

  getById: async (id: number): Promise<PhamNhan | undefined> => {
    await delay(200);
    const pn = phamNhanData.find(p => p.Id === id);
    if (!pn) return undefined;
    return {
      ...pn,
      PhongGiam: phongGiamData.find(pg => pg.Id === pn.PhongGiamId),
    };
  },

  create: async (phamNhan: Omit<PhamNhan, 'Id'>): Promise<PhamNhan> => {
    await delay(300);
    const newPhamNhan: PhamNhan = {
      ...phamNhan,
      Id: Math.max(...phamNhanData.map(pn => pn.Id)) + 1,
    };
    phamNhanData.push(newPhamNhan);
    
    // Update room count
    const room = phongGiamData.find(pg => pg.Id === phamNhan.PhongGiamId);
    if (room) {
      room.SoLuongHienTai += 1;
    }
    
    return newPhamNhan;
  },

  update: async (id: number, phamNhan: Partial<PhamNhan>): Promise<PhamNhan | undefined> => {
    await delay(300);
    const index = phamNhanData.findIndex(pn => pn.Id === id);
    if (index === -1) return undefined;
    
    const oldRoomId = phamNhanData[index].PhongGiamId;
    phamNhanData[index] = { ...phamNhanData[index], ...phamNhan };
    
    // Update room counts if room changed
    if (phamNhan.PhongGiamId && phamNhan.PhongGiamId !== oldRoomId) {
      const oldRoom = phongGiamData.find(pg => pg.Id === oldRoomId);
      const newRoom = phongGiamData.find(pg => pg.Id === phamNhan.PhongGiamId);
      if (oldRoom) oldRoom.SoLuongHienTai -= 1;
      if (newRoom) newRoom.SoLuongHienTai += 1;
    }
    
    return phamNhanData[index];
  },

  delete: async (id: number): Promise<boolean> => {
    await delay(300);
    const index = phamNhanData.findIndex(pn => pn.Id === id);
    if (index === -1) return false;
    
    const roomId = phamNhanData[index].PhongGiamId;
    const room = phongGiamData.find(pg => pg.Id === roomId);
    if (room) room.SoLuongHienTai -= 1;
    
    phamNhanData.splice(index, 1);
    return true;
  },
};

// ============== PHONG GIAM API ==============
export const phongGiamApi = {
  getAll: async (): Promise<PhongGiam[]> => {
    await delay(300);
    return [...phongGiamData];
  },

  getAvailable: async (): Promise<PhongGiam[]> => {
    await delay(200);
    return phongGiamData.filter(
      pg => pg.TrangThai === 'HoatDong' && pg.SoLuongHienTai < pg.SucChua
    );
  },
};

// ============== KHEN THUONG API ==============
export const khenThuongApi = {
  getAll: async (): Promise<KhenThuong[]> => {
    await delay(300);
    return khenThuongData.map(kt => ({
      ...kt,
      PhamNhan: phamNhanData.find(pn => pn.Id === kt.PhamNhanId),
    }));
  },

  create: async (khenThuong: Omit<KhenThuong, 'Id'>): Promise<KhenThuong> => {
    await delay(300);
    const newKT: KhenThuong = {
      ...khenThuong,
      Id: Math.max(...khenThuongData.map(kt => kt.Id), 0) + 1,
    };
    khenThuongData.push(newKT);
    return newKT;
  },

  delete: async (id: number): Promise<boolean> => {
    await delay(300);
    const index = khenThuongData.findIndex(kt => kt.Id === id);
    if (index === -1) return false;
    khenThuongData.splice(index, 1);
    return true;
  },
};

// ============== KY LUAT API ==============
export const kyLuatApi = {
  getAll: async (): Promise<KyLuat[]> => {
    await delay(300);
    return kyLuatData.map(kl => ({
      ...kl,
      PhamNhan: phamNhanData.find(pn => pn.Id === kl.PhamNhanId),
    }));
  },

  create: async (kyLuat: Omit<KyLuat, 'Id'>): Promise<KyLuat> => {
    await delay(300);
    const newKL: KyLuat = {
      ...kyLuat,
      Id: Math.max(...kyLuatData.map(kl => kl.Id), 0) + 1,
    };
    kyLuatData.push(newKL);
    return newKL;
  },

  delete: async (id: number): Promise<boolean> => {
    await delay(300);
    const index = kyLuatData.findIndex(kl => kl.Id === id);
    if (index === -1) return false;
    kyLuatData.splice(index, 1);
    return true;
  },
};

// ============== THONG KE API ==============
export const thongKeApi = {
  getPhongGiam: async (): Promise<ThongKePhongGiam[]> => {
    await delay(300);
    return phongGiamData.map(pg => ({
      MaPhong: pg.MaPhong,
      TenPhong: pg.TenPhong,
      SucChua: pg.SucChua,
      SoLuongHienTai: pg.SoLuongHienTai,
      TyLeSuDung: Math.round((pg.SoLuongHienTai / pg.SucChua) * 100),
      TrangThai: pg.TrangThai === 'HoatDong' ? 'Hoạt động' : 
                 pg.TrangThai === 'BaoTri' ? 'Bảo trì' : 'Đã khóa',
    }));
  },

  getDashboardStats: async () => {
    await delay(200);
    return {
      tongPhamNhan: phamNhanData.filter(pn => pn.TrangThai === 'DangGiam').length,
      tongCanBo: canBoData.length,
      tongPhongGiam: phongGiamData.filter(pg => pg.TrangThai === 'HoatDong').length,
      tongKhenThuong: khenThuongData.length,
      tongKyLuat: kyLuatData.length,
    };
  },
};
