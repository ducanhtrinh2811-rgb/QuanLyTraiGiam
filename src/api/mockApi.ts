// API endpoints - Connects to C# ASP.NET Core backend
// Set USE_MOCK = false when backend is ready

import apiClient from './apiClient';
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
  mockSuCo,
  mockTaiKhoan,
  mockQuyen
} from '@/data/mockData';

// ⚠️ Set to FALSE when C# backend is running
const USE_MOCK = true;

const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

// Mock data state
let canBoData = [...mockCanBo];
let phamNhanData = [...mockPhamNhan];
let khenThuongData = [...mockKhenThuong];
let kyLuatData = [...mockKyLuat];
let phongGiamData = [...mockPhongGiam];
let sucKhoeData = [...mockSucKhoe];
let thamGapData = [...mockThamGap];
let laoDongData = [...mockLaoDong];
let suCoData = [...mockSuCo];

// ============== AUTH API ==============
export const authApi = {
  login: async (username: string, password: string) => {
    if (USE_MOCK) {
      await delay(500);
      const account = mockTaiKhoan.find(
        acc => acc.Ten === username && acc.MatKhauHash === password && acc.IsActive
      );
      if (!account) {
        return { success: false, error: 'Tên đăng nhập hoặc mật khẩu không đúng' };
      }
      const role = mockQuyen.find(q => q.Id === account.QuyenId);
      if (!role) {
        return { success: false, error: 'Tài khoản không có quyền truy cập' };
      }
      return {
        success: true,
        user: { id: account.Id, username: account.Ten, role: role.TenQuyen },
        token: 'mock-jwt-token',
      };
    }
    const response = await apiClient.post('/auth/login', { Ten: username, MatKhau: password });
    return response.data;
  },
};

// ============== CAN BO API ==============
export const canBoApi = {
  getAll: async (): Promise<CanBo[]> => {
    if (USE_MOCK) { await delay(300); return [...canBoData]; }
    const response = await apiClient.get<CanBo[]>('/canbo');
    return response.data;
  },
  getById: async (id: number): Promise<CanBo | undefined> => {
    if (USE_MOCK) { await delay(200); return canBoData.find(cb => cb.Id === id); }
    const response = await apiClient.get<CanBo>(`/canbo/${id}`);
    return response.data;
  },
  create: async (canBo: Omit<CanBo, 'Id'>): Promise<CanBo> => {
    if (USE_MOCK) {
      await delay(300);
      const newCanBo: CanBo = { ...canBo, Id: Math.max(...canBoData.map(cb => cb.Id)) + 1 };
      canBoData.push(newCanBo);
      return newCanBo;
    }
    const response = await apiClient.post<CanBo>('/canbo', canBo);
    return response.data;
  },
  update: async (id: number, canBo: Partial<CanBo>): Promise<CanBo | undefined> => {
    if (USE_MOCK) {
      await delay(300);
      const index = canBoData.findIndex(cb => cb.Id === id);
      if (index === -1) return undefined;
      canBoData[index] = { ...canBoData[index], ...canBo };
      return canBoData[index];
    }
    await apiClient.put(`/canbo/${id}`, canBo);
    return { Id: id, ...canBo } as CanBo;
  },
  delete: async (id: number): Promise<boolean> => {
    if (USE_MOCK) {
      await delay(300);
      const index = canBoData.findIndex(cb => cb.Id === id);
      if (index === -1) return false;
      canBoData.splice(index, 1);
      return true;
    }
    await apiClient.delete(`/canbo/${id}`);
    return true;
  },
};

// ============== PHAM NHAN API ==============
export const phamNhanApi = {
  getAll: async (): Promise<PhamNhan[]> => {
    if (USE_MOCK) {
      await delay(300);
      return phamNhanData.map(pn => ({ ...pn, PhongGiam: phongGiamData.find(pg => pg.Id === pn.PhongGiamId) }));
    }
    const response = await apiClient.get<PhamNhan[]>('/phamnhan');
    return response.data;
  },
  getById: async (id: number): Promise<PhamNhan | undefined> => {
    if (USE_MOCK) {
      await delay(200);
      const pn = phamNhanData.find(p => p.Id === id);
      if (!pn) return undefined;
      return { ...pn, PhongGiam: phongGiamData.find(pg => pg.Id === pn.PhongGiamId) };
    }
    const response = await apiClient.get<PhamNhan>(`/phamnhan/${id}`);
    return response.data;
  },
  create: async (phamNhan: Omit<PhamNhan, 'Id'>): Promise<PhamNhan> => {
    if (USE_MOCK) {
      await delay(300);
      const newPhamNhan: PhamNhan = { ...phamNhan, Id: Math.max(...phamNhanData.map(pn => pn.Id)) + 1 };
      phamNhanData.push(newPhamNhan);
      const room = phongGiamData.find(pg => pg.Id === phamNhan.PhongGiamId);
      if (room) room.SoLuongHienTai += 1;
      return newPhamNhan;
    }
    const response = await apiClient.post<PhamNhan>('/phamnhan', phamNhan);
    return response.data;
  },
  update: async (id: number, phamNhan: Partial<PhamNhan>): Promise<PhamNhan | undefined> => {
    if (USE_MOCK) {
      await delay(300);
      const index = phamNhanData.findIndex(pn => pn.Id === id);
      if (index === -1) return undefined;
      const oldRoomId = phamNhanData[index].PhongGiamId;
      phamNhanData[index] = { ...phamNhanData[index], ...phamNhan };
      if (phamNhan.PhongGiamId && phamNhan.PhongGiamId !== oldRoomId) {
        const oldRoom = phongGiamData.find(pg => pg.Id === oldRoomId);
        const newRoom = phongGiamData.find(pg => pg.Id === phamNhan.PhongGiamId);
        if (oldRoom) oldRoom.SoLuongHienTai -= 1;
        if (newRoom) newRoom.SoLuongHienTai += 1;
      }
      return phamNhanData[index];
    }
    await apiClient.put(`/phamnhan/${id}`, phamNhan);
    return { Id: id, ...phamNhan } as PhamNhan;
  },
  delete: async (id: number): Promise<boolean> => {
    if (USE_MOCK) {
      await delay(300);
      const index = phamNhanData.findIndex(pn => pn.Id === id);
      if (index === -1) return false;
      const roomId = phamNhanData[index].PhongGiamId;
      const room = phongGiamData.find(pg => pg.Id === roomId);
      if (room) room.SoLuongHienTai -= 1;
      phamNhanData.splice(index, 1);
      return true;
    }
    await apiClient.delete(`/phamnhan/${id}`);
    return true;
  },
};

// ============== PHONG GIAM API ==============
export const phongGiamApi = {
  getAll: async (): Promise<PhongGiam[]> => {
    if (USE_MOCK) { await delay(300); return [...phongGiamData]; }
    const response = await apiClient.get<PhongGiam[]>('/phonggiam');
    return response.data;
  },
  getAvailable: async (): Promise<PhongGiam[]> => {
    if (USE_MOCK) {
      await delay(200);
      return phongGiamData.filter(pg => pg.TrangThai === 'HoatDong' && pg.SoLuongHienTai < pg.SucChua);
    }
    const response = await apiClient.get<PhongGiam[]>('/phonggiam/available');
    return response.data;
  },
};

// ============== KHEN THUONG API ==============
export const khenThuongApi = {
  getAll: async (): Promise<KhenThuong[]> => {
    if (USE_MOCK) {
      await delay(300);
      return khenThuongData.map(kt => ({ ...kt, PhamNhan: phamNhanData.find(pn => pn.Id === kt.PhamNhanId) }));
    }
    const response = await apiClient.get<KhenThuong[]>('/khenthuong');
    return response.data;
  },
  create: async (khenThuong: Omit<KhenThuong, 'Id'>): Promise<KhenThuong> => {
    if (USE_MOCK) {
      await delay(300);
      const newKT: KhenThuong = { ...khenThuong, Id: Math.max(...khenThuongData.map(kt => kt.Id), 0) + 1 };
      khenThuongData.push(newKT);
      return newKT;
    }
    const response = await apiClient.post<KhenThuong>('/khenthuong', khenThuong);
    return response.data;
  },
  delete: async (id: number): Promise<boolean> => {
    if (USE_MOCK) {
      await delay(300);
      const index = khenThuongData.findIndex(kt => kt.Id === id);
      if (index === -1) return false;
      khenThuongData.splice(index, 1);
      return true;
    }
    await apiClient.delete(`/khenthuong/${id}`);
    return true;
  },
};

// ============== KY LUAT API ==============
export const kyLuatApi = {
  getAll: async (): Promise<KyLuat[]> => {
    if (USE_MOCK) {
      await delay(300);
      return kyLuatData.map(kl => ({ ...kl, PhamNhan: phamNhanData.find(pn => pn.Id === kl.PhamNhanId) }));
    }
    const response = await apiClient.get<KyLuat[]>('/kyluat');
    return response.data;
  },
  create: async (kyLuat: Omit<KyLuat, 'Id'>): Promise<KyLuat> => {
    if (USE_MOCK) {
      await delay(300);
      const newKL: KyLuat = { ...kyLuat, Id: Math.max(...kyLuatData.map(kl => kl.Id), 0) + 1 };
      kyLuatData.push(newKL);
      return newKL;
    }
    const response = await apiClient.post<KyLuat>('/kyluat', kyLuat);
    return response.data;
  },
  delete: async (id: number): Promise<boolean> => {
    if (USE_MOCK) {
      await delay(300);
      const index = kyLuatData.findIndex(kl => kl.Id === id);
      if (index === -1) return false;
      kyLuatData.splice(index, 1);
      return true;
    }
    await apiClient.delete(`/kyluat/${id}`);
    return true;
  },
};

// ============== SUC KHOE API ==============
export const sucKhoeApi = {
  getAll: async (): Promise<SucKhoe[]> => {
    if (USE_MOCK) {
      await delay(300);
      return sucKhoeData.map(sk => ({ ...sk, PhamNhan: phamNhanData.find(pn => pn.Id === sk.PhamNhanId) }));
    }
    const response = await apiClient.get<SucKhoe[]>('/suckhoe');
    return response.data;
  },
  create: async (sucKhoe: Omit<SucKhoe, 'Id'>): Promise<SucKhoe> => {
    if (USE_MOCK) {
      await delay(300);
      const newSK: SucKhoe = { ...sucKhoe, Id: Math.max(...sucKhoeData.map(sk => sk.Id), 0) + 1 };
      sucKhoeData.push(newSK);
      return newSK;
    }
    const response = await apiClient.post<SucKhoe>('/suckhoe', sucKhoe);
    return response.data;
  },
  delete: async (id: number): Promise<boolean> => {
    if (USE_MOCK) {
      await delay(300);
      const index = sucKhoeData.findIndex(sk => sk.Id === id);
      if (index === -1) return false;
      sucKhoeData.splice(index, 1);
      return true;
    }
    await apiClient.delete(`/suckhoe/${id}`);
    return true;
  },
};

// ============== THAM GAP API ==============
export const thamGapApi = {
  getAll: async (): Promise<ThamGap[]> => {
    if (USE_MOCK) {
      await delay(300);
      return thamGapData.map(tg => ({ ...tg, PhamNhan: phamNhanData.find(pn => pn.Id === tg.PhamNhanId) }));
    }
    const response = await apiClient.get<ThamGap[]>('/thamgap');
    return response.data;
  },
  create: async (thamGap: Omit<ThamGap, 'Id'>): Promise<ThamGap> => {
    if (USE_MOCK) {
      await delay(300);
      const newTG: ThamGap = { ...thamGap, Id: Math.max(...thamGapData.map(tg => tg.Id), 0) + 1 };
      thamGapData.push(newTG);
      return newTG;
    }
    const response = await apiClient.post<ThamGap>('/thamgap', thamGap);
    return response.data;
  },
  delete: async (id: number): Promise<boolean> => {
    if (USE_MOCK) {
      await delay(300);
      const index = thamGapData.findIndex(tg => tg.Id === id);
      if (index === -1) return false;
      thamGapData.splice(index, 1);
      return true;
    }
    await apiClient.delete(`/thamgap/${id}`);
    return true;
  },
};

// ============== LAO DONG API ==============
export const laoDongApi = {
  getAll: async (): Promise<LaoDong[]> => {
    if (USE_MOCK) {
      await delay(300);
      return laoDongData.map(ld => ({ ...ld, PhamNhan: phamNhanData.find(pn => pn.Id === ld.PhamNhanId) }));
    }
    const response = await apiClient.get<LaoDong[]>('/laodong');
    return response.data;
  },
  create: async (laoDong: Omit<LaoDong, 'Id'>): Promise<LaoDong> => {
    if (USE_MOCK) {
      await delay(300);
      const newLD: LaoDong = { ...laoDong, Id: Math.max(...laoDongData.map(ld => ld.Id), 0) + 1 };
      laoDongData.push(newLD);
      return newLD;
    }
    const response = await apiClient.post<LaoDong>('/laodong', laoDong);
    return response.data;
  },
  delete: async (id: number): Promise<boolean> => {
    if (USE_MOCK) {
      await delay(300);
      const index = laoDongData.findIndex(ld => ld.Id === id);
      if (index === -1) return false;
      laoDongData.splice(index, 1);
      return true;
    }
    await apiClient.delete(`/laodong/${id}`);
    return true;
  },
};

// ============== SU CO API ==============
export const suCoApi = {
  getAll: async (): Promise<SuCo[]> => {
    if (USE_MOCK) { await delay(300); return [...suCoData]; }
    const response = await apiClient.get<SuCo[]>('/suco');
    return response.data;
  },
  create: async (suCo: Omit<SuCo, 'Id'>): Promise<SuCo> => {
    if (USE_MOCK) {
      await delay(300);
      const newSC: SuCo = { ...suCo, Id: Math.max(...suCoData.map(sc => sc.Id), 0) + 1 };
      suCoData.push(newSC);
      return newSC;
    }
    const response = await apiClient.post<SuCo>('/suco', suCo);
    return response.data;
  },
  update: async (id: number, suCo: Partial<SuCo>): Promise<SuCo | undefined> => {
    if (USE_MOCK) {
      await delay(300);
      const index = suCoData.findIndex(sc => sc.Id === id);
      if (index === -1) return undefined;
      suCoData[index] = { ...suCoData[index], ...suCo };
      return suCoData[index];
    }
    await apiClient.put(`/suco/${id}`, suCo);
    return { Id: id, ...suCo } as SuCo;
  },
  delete: async (id: number): Promise<boolean> => {
    if (USE_MOCK) {
      await delay(300);
      const index = suCoData.findIndex(sc => sc.Id === id);
      if (index === -1) return false;
      suCoData.splice(index, 1);
      return true;
    }
    await apiClient.delete(`/suco/${id}`);
    return true;
  },
};

// ============== THONG KE API ==============
export const thongKeApi = {
  getPhongGiam: async (): Promise<ThongKePhongGiam[]> => {
    if (USE_MOCK) {
      await delay(300);
      return phongGiamData.map(pg => ({
        MaPhong: pg.MaPhong,
        TenPhong: pg.TenPhong,
        SucChua: pg.SucChua,
        SoLuongHienTai: pg.SoLuongHienTai,
        TyLeSuDung: Math.round((pg.SoLuongHienTai / pg.SucChua) * 100),
        TrangThai: pg.TrangThai === 'HoatDong' ? 'Hoạt động' : pg.TrangThai === 'BaoTri' ? 'Bảo trì' : 'Đã khóa',
      }));
    }
    const response = await apiClient.get<ThongKePhongGiam[]>('/thongke/phonggiam');
    return response.data;
  },
  getDashboardStats: async () => {
    if (USE_MOCK) {
      await delay(200);
      return {
        tongPhamNhan: phamNhanData.filter(pn => pn.TrangThai === 'DangGiam').length,
        tongCanBo: canBoData.length,
        tongPhongGiam: phongGiamData.filter(pg => pg.TrangThai === 'HoatDong').length,
        tongKhenThuong: khenThuongData.length,
        tongKyLuat: kyLuatData.length,
      };
    }
    const response = await apiClient.get('/thongke/dashboard');
    return response.data;
  },
};
