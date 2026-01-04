import React, { createContext, useContext, useState, useCallback, ReactNode } from 'react';
import { AuthUser, QuyenType } from '@/types';
import { authApi } from '@/api/mockApi';

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
    if (savedUser) {
      try {
        const parsed = JSON.parse(savedUser);
        return parsed.user || parsed;
      } catch {
        return null;
      }
    }
    return null;
  });

  const login = useCallback(async (username: string, password: string) => {
    try {
      const result = await authApi.login(username, password);

      if (!result.success) {
        return { success: false, error: result.error || 'Đăng nhập thất bại' };
      }

      const authUser: AuthUser = {
        id: result.user.id,
        username: result.user.username,
        role: result.user.role as QuyenType,
        canBoId: result.user.canBoId,
      };

      setUser(authUser);
      localStorage.setItem('auth_user', JSON.stringify({
        user: authUser,
        token: result.token,
      }));

      return { success: true };
    } catch (error) {
      console.error('Login error:', error);
      return { success: false, error: 'Không thể kết nối đến server' };
    }
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
