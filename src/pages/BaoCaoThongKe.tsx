import React, { useState, useEffect } from 'react';
import { PageHeader } from '@/components/PageHeader';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Progress } from '@/components/ui/progress';
import { thongKeApi } from '@/api/mockApi';
import { ThongKePhongGiam } from '@/types';
import { toast } from 'sonner';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, Cell } from 'recharts';
import { cn } from '@/lib/utils';

const BaoCaoThongKe: React.FC = () => {
  const [phongGiamStats, setPhongGiamStats] = useState<ThongKePhongGiam[]>([]);
  const [dashboardStats, setDashboardStats] = useState({
    tongPhamNhan: 0,
    tongCanBo: 0,
    tongPhongGiam: 0,
    tongKhenThuong: 0,
    tongKyLuat: 0,
  });
  const [isLoading, setIsLoading] = useState(true);

  const loadData = async () => {
    setIsLoading(true);
    try {
      const [pgStats, dsStats] = await Promise.all([
        thongKeApi.getPhongGiam(),
        thongKeApi.getDashboardStats(),
      ]);
      setPhongGiamStats(pgStats);
      setDashboardStats(dsStats);
    } catch {
      toast.error('Không thể tải dữ liệu');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const getCapacityColor = (ratio: number) => {
    if (ratio >= 90) return 'hsl(0, 84%, 60%)'; // destructive
    if (ratio >= 70) return 'hsl(38, 92%, 50%)'; // warning
    return 'hsl(142, 76%, 36%)'; // success
  };

  const statCards = [
    { label: 'Tổng phạm nhân', value: dashboardStats.tongPhamNhan, icon: '👤', color: 'bg-primary/10 text-primary' },
    { label: 'Tổng cán bộ', value: dashboardStats.tongCanBo, icon: '👮', color: 'bg-accent/10 text-accent' },
    { label: 'Phòng giam hoạt động', value: dashboardStats.tongPhongGiam, icon: '🏠', color: 'bg-success/10 text-success' },
    { label: 'Khen thưởng', value: dashboardStats.tongKhenThuong, icon: '🏆', color: 'bg-warning/10 text-warning' },
    { label: 'Kỷ luật', value: dashboardStats.tongKyLuat, icon: '⚠️', color: 'bg-destructive/10 text-destructive' },
  ];

  if (isLoading) {
    return (
      <div className="min-h-screen bg-background">
        <PageHeader title="Báo cáo - Thống kê" icon="📊" showBackButton />
        <main className="container mx-auto px-4 py-8">
          <div className="flex items-center justify-center py-20">
            <div className="w-8 h-8 border-4 border-primary border-t-transparent rounded-full animate-spin" />
          </div>
        </main>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-background">
      <PageHeader title="Báo cáo - Thống kê" icon="📊" showBackButton />

      <main className="container mx-auto px-4 py-8 space-y-8">
        {/* Stats Cards */}
        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4 animate-slide-up">
          {statCards.map((stat, index) => (
            <Card key={stat.label} className="card-elevated" style={{ animationDelay: `${index * 50}ms` }}>
              <CardContent className="p-4">
                <div className="flex items-center gap-3">
                  <div className={cn('w-12 h-12 rounded-lg flex items-center justify-center text-2xl', stat.color)}>
                    {stat.icon}
                  </div>
                  <div>
                    <p className="text-2xl font-bold">{stat.value}</p>
                    <p className="text-xs text-muted-foreground">{stat.label}</p>
                  </div>
                </div>
              </CardContent>
            </Card>
          ))}
        </div>

        {/* Room Statistics Chart */}
        <Card className="card-elevated animate-slide-up" style={{ animationDelay: '200ms' }}>
          <CardHeader>
            <CardTitle className="text-lg font-semibold">Tỷ lệ sử dụng phòng giam</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="h-80">
              <ResponsiveContainer width="100%" height="100%">
                <BarChart data={phongGiamStats} margin={{ top: 20, right: 30, left: 20, bottom: 5 }}>
                  <CartesianGrid strokeDasharray="3 3" stroke="hsl(var(--border))" />
                  <XAxis 
                    dataKey="TenPhong" 
                    tick={{ fontSize: 12, fill: 'hsl(var(--muted-foreground))' }}
                  />
                  <YAxis 
                    tick={{ fontSize: 12, fill: 'hsl(var(--muted-foreground))' }}
                    domain={[0, 100]}
                    tickFormatter={(value) => `${value}%`}
                  />
                  <Tooltip 
                    formatter={(value: number) => [`${value}%`, 'Tỷ lệ sử dụng']}
                    contentStyle={{
                      backgroundColor: 'hsl(var(--card))',
                      border: '1px solid hsl(var(--border))',
                      borderRadius: '8px',
                    }}
                  />
                  <Bar dataKey="TyLeSuDung" radius={[4, 4, 0, 0]}>
                    {phongGiamStats.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={getCapacityColor(entry.TyLeSuDung)} />
                    ))}
                  </Bar>
                </BarChart>
              </ResponsiveContainer>
            </div>
          </CardContent>
        </Card>

        {/* Room Details Table */}
        <Card className="card-elevated animate-slide-up" style={{ animationDelay: '300ms' }}>
          <CardHeader>
            <CardTitle className="text-lg font-semibold">Chi tiết phòng giam</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead>
                  <tr className="border-b border-border">
                    <th className="text-left p-3 font-semibold text-sm text-muted-foreground">Mã phòng</th>
                    <th className="text-left p-3 font-semibold text-sm text-muted-foreground">Tên phòng</th>
                    <th className="text-left p-3 font-semibold text-sm text-muted-foreground">Hiện tại / Sức chứa</th>
                    <th className="text-left p-3 font-semibold text-sm text-muted-foreground">Tỷ lệ sử dụng</th>
                    <th className="text-left p-3 font-semibold text-sm text-muted-foreground">Trạng thái</th>
                  </tr>
                </thead>
                <tbody>
                  {phongGiamStats.map((pg) => (
                    <tr key={pg.MaPhong} className="border-b border-border last:border-0 hover:bg-muted/30 transition-colors">
                      <td className="p-3 text-sm font-mono">{pg.MaPhong}</td>
                      <td className="p-3 text-sm">{pg.TenPhong}</td>
                      <td className="p-3 text-sm">{pg.SoLuongHienTai} / {pg.SucChua}</td>
                      <td className="p-3">
                        <div className="flex items-center gap-3">
                          <Progress 
                            value={pg.TyLeSuDung} 
                            className="w-24 h-2"
                          />
                          <span className={cn(
                            'text-sm font-medium',
                            pg.TyLeSuDung >= 90 ? 'text-destructive' :
                            pg.TyLeSuDung >= 70 ? 'text-warning' : 'text-success'
                          )}>
                            {pg.TyLeSuDung}%
                          </span>
                        </div>
                      </td>
                      <td className="p-3">
                        <Badge 
                          variant={pg.TrangThai === 'Hoạt động' ? 'default' : 'secondary'}
                          className={pg.TrangThai === 'Hoạt động' ? 'bg-success' : ''}
                        >
                          {pg.TrangThai}
                        </Badge>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </CardContent>
        </Card>
      </main>
    </div>
  );
};

export default BaoCaoThongKe;
