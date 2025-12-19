// Mock API endpoints - Ready to connect to SQL Server backend
// These functions simulate REST API calls

import { 
  CanBo, 
  PhamNhan, 
  KhenThuong, 
  KyLuat, 
  ThongKePhongGiam,
  PhongGiam,
  SucKhoe,
  ThamGap,
  LaoDong,
  SuCo
} from '@/types';
import { 
  mockCanBo, 
  mockPhamNhan, 
  mockKhenThuong, 
  mockKyLuat, 
  mockPhongGiam,
  mockSucKhoe,
  mockThamGap,
  mockLaoDong,
  mockSuCo
} from '@/data/mockData';

// Simulate network delay
const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

// State for mock data (simulating database)
let canBoData = [...mockCanBo];
let phamNhanData = [...mockPhamNhan];
let khenThuongData = [...mockKhenThuong];
let kyLuatData = [...mockKyLuat];
let phongGiamData = [...mockPhongGiam];
let sucKhoeData = [...mockSucKhoe];
let thamGapData = [...mockThamGap];
let laoDongData = [...mockLaoDong];
let suCoData = [...mockSuCo];

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

// ============== SUC KHOE API ==============
export const sucKhoeApi = {
  getAll: async (): Promise<SucKhoe[]> => {
    await delay(300);
    return sucKhoeData.map(sk => ({
      ...sk,
      PhamNhan: phamNhanData.find(pn => pn.Id === sk.PhamNhanId),
    }));
  },

  create: async (sucKhoe: Omit<SucKhoe, 'Id'>): Promise<SucKhoe> => {
    await delay(300);
    const newSK: SucKhoe = {
      ...sucKhoe,
      Id: Math.max(...sucKhoeData.map(sk => sk.Id), 0) + 1,
    };
    sucKhoeData.push(newSK);
    return newSK;
  },

  delete: async (id: number): Promise<boolean> => {
    await delay(300);
    const index = sucKhoeData.findIndex(sk => sk.Id === id);
    if (index === -1) return false;
    sucKhoeData.splice(index, 1);
    return true;
  },
};

// ============== THAM GAP API ==============
export const thamGapApi = {
  getAll: async (): Promise<ThamGap[]> => {
    await delay(300);
    return thamGapData.map(tg => ({
      ...tg,
      PhamNhan: phamNhanData.find(pn => pn.Id === tg.PhamNhanId),
    }));
  },

  create: async (thamGap: Omit<ThamGap, 'Id'>): Promise<ThamGap> => {
    await delay(300);
    const newTG: ThamGap = {
      ...thamGap,
      Id: Math.max(...thamGapData.map(tg => tg.Id), 0) + 1,
    };
    thamGapData.push(newTG);
    return newTG;
  },

  delete: async (id: number): Promise<boolean> => {
    await delay(300);
    const index = thamGapData.findIndex(tg => tg.Id === id);
    if (index === -1) return false;
    thamGapData.splice(index, 1);
    return true;
  },
};

// ============== LAO DONG API ==============
export const laoDongApi = {
  getAll: async (): Promise<LaoDong[]> => {
    await delay(300);
    return laoDongData.map(ld => ({
      ...ld,
      PhamNhan: phamNhanData.find(pn => pn.Id === ld.PhamNhanId),
    }));
  },

  create: async (laoDong: Omit<LaoDong, 'Id'>): Promise<LaoDong> => {
    await delay(300);
    const newLD: LaoDong = {
      ...laoDong,
      Id: Math.max(...laoDongData.map(ld => ld.Id), 0) + 1,
    };
    laoDongData.push(newLD);
    return newLD;
  },

  delete: async (id: number): Promise<boolean> => {
    await delay(300);
    const index = laoDongData.findIndex(ld => ld.Id === id);
    if (index === -1) return false;
    laoDongData.splice(index, 1);
    return true;
  },
};

// ============== SU CO API ==============
export const suCoApi = {
  getAll: async (): Promise<SuCo[]> => {
    await delay(300);
    return [...suCoData];
  },

  create: async (suCo: Omit<SuCo, 'Id'>): Promise<SuCo> => {
    await delay(300);
    const newSC: SuCo = {
      ...suCo,
      Id: Math.max(...suCoData.map(sc => sc.Id), 0) + 1,
    };
    suCoData.push(newSC);
    return newSC;
  },

  update: async (id: number, suCo: Partial<SuCo>): Promise<SuCo | undefined> => {
    await delay(300);
    const index = suCoData.findIndex(sc => sc.Id === id);
    if (index === -1) return undefined;
    suCoData[index] = { ...suCoData[index], ...suCo };
    return suCoData[index];
  },

  delete: async (id: number): Promise<boolean> => {
    await delay(300);
    const index = suCoData.findIndex(sc => sc.Id === id);
    if (index === -1) return false;
    suCoData.splice(index, 1);
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
