import React, { createContext, useContext, useState, useCallback, ReactNode } from 'react';
import { AuthUser, QuyenType } from '@/types';
import { mockTaiKhoan, mockQuyen } from '@/data/mockData';

interface AuthContextType {
  user: AuthUser | null;
  isAuthenticated: boolean;
  login: (username: string, password: string) => Promise<{ success: boolean; error?: string }>;
  logout: () => void;
  hasPermission: (requiredRoles: QuyenType[]) => boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<AuthUser | null>(() => {
    const savedUser = localStorage.getItem('auth_user');
    return savedUser ? JSON.parse(savedUser) : null;
  });

  const login = useCallback(async (username: string, password: string) => {
    // Simulate API call delay
    await new Promise(resolve => setTimeout(resolve, 500));

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

    const authUser: AuthUser = {
      id: account.Id,
      username: account.Ten,
      role: role.TenQuyen,
    };

    setUser(authUser);
    localStorage.setItem('auth_user', JSON.stringify(authUser));

    return { success: true };
  }, []);

  const logout = useCallback(() => {
    setUser(null);
    localStorage.removeItem('auth_user');
  }, []);

  const hasPermission = useCallback((requiredRoles: QuyenType[]) => {
    if (!user) return false;
    return requiredRoles.includes(user.role);
  }, [user]);

  return (
    <AuthContext.Provider value={{
      user,
      isAuthenticated: !!user,
      login,
      logout,
      hasPermission,
    }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
