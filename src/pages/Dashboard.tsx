import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '@/contexts/AuthContext';
import { PageHeader } from '@/components/PageHeader';
import { ModuleCard } from '@/components/ModuleCard';
import { Button } from '@/components/ui/button';
import { LogOut, User } from 'lucide-react';
import { toast } from 'sonner';

const Dashboard: React.FC = () => {
  const { user, logout, hasPermission } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    toast.success('Đã đăng xuất');
    navigate('/login');
  };

  const modules = [
    {
      icon: '👤',
      title: 'Quản lý Phạm nhân',
      description: 'Quản lý thông tin phạm nhân',
      to: '/pham-nhan',
      roles: ['Admin', 'CanBo', 'GiamSat'] as const,
    },
    {
      icon: '👮',
      title: 'Quản lý Cán bộ',
      description: 'Quản lý thông tin cán bộ',
      to: '/can-bo',
      roles: ['Admin', 'CanBo', 'GiamSat'] as const,
    },
    {
      icon: '🏆',
      title: 'Khen thưởng - Kỷ luật',
      description: 'Quản lý khen thưởng và kỷ luật',
      to: '/khen-thuong-ky-luat',
      roles: ['Admin', 'CanBo', 'GiamSat'] as const,
    },
    {
      icon: '🏥',
      title: 'Sức khỏe Phạm nhân',
      description: 'Quản lý sức khỏe và y tế',
      to: '/suc-khoe',
      roles: ['Admin', 'CanBo'] as const,
    },
    {
      icon: '👥',
      title: 'Thăm gặp - Tiếp tế',
      description: 'Quản lý thăm nuôi và tiếp tế',
      to: '/tham-gap',
      roles: ['Admin', 'CanBo'] as const,
    },
    {
      icon: '🎓',
      title: 'Lao động - Học tập',
      description: 'Quản lý lao động và giáo dục',
      to: '/lao-dong',
      roles: ['Admin', 'CanBo'] as const,
    },
    {
      icon: '⚠️',
      title: 'Sự cố - An ninh',
      description: 'Quản lý sự cố và an ninh',
      to: '/su-co',
      roles: ['Admin', 'CanBo'] as const,
    },
    {
      icon: '📊',
      title: 'Báo cáo - Thống kê',
      description: 'Báo cáo và thống kê tổng hợp',
      to: '/bao-cao',
      roles: ['Admin', 'CanBo', 'GiamSat'] as const,
    },
  ];

  const visibleModules = modules.filter(m => hasPermission(m.roles as unknown as ('Admin' | 'CanBo' | 'GiamSat')[]));

  return (
    <div className="min-h-screen bg-background">
      <PageHeader
        title="Hệ Thống Quản Lý Trại Giam"
        action={
          <div className="flex items-center gap-3">
            <div className="hidden sm:flex items-center gap-2 text-primary-foreground/80 text-sm">
              <User className="w-4 h-4" />
              <span>{user?.username}</span>
              <span className="px-2 py-0.5 bg-primary-foreground/20 rounded text-xs font-medium">
                {user?.role}
              </span>
            </div>
            <Button
              variant="ghost"
              size="sm"
              onClick={handleLogout}
              className="text-primary-foreground hover:bg-primary-foreground/20"
            >
              <LogOut className="w-4 h-4 mr-2" />
              Đăng xuất
            </Button>
          </div>
        }
      />

      <main className="container mx-auto px-4 py-8">
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          {visibleModules.map((module, index) => (
            <ModuleCard
              key={module.to}
              icon={module.icon}
              title={module.title}
              description={module.description}
              to={module.to}
              delay={index * 50}
            />
          ))}
        </div>
      </main>
    </div>
  );
};

export default Dashboard;
