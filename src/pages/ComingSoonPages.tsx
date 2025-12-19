import React from 'react';
import { PageHeader } from '@/components/PageHeader';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Construction } from 'lucide-react';

interface ComingSoonPageProps {
  title: string;
  icon: string;
  description: string;
}

const ComingSoonPage: React.FC<ComingSoonPageProps> = ({ title, icon, description }) => {
  return (
    <div className="min-h-screen bg-background">
      <PageHeader title={title} icon={icon} showBackButton />
      
      <main className="container mx-auto px-4 py-8">
        <Card className="card-elevated max-w-lg mx-auto animate-scale-in">
          <CardHeader className="text-center">
            <div className="mx-auto w-16 h-16 rounded-full bg-muted flex items-center justify-center mb-4">
              <Construction className="w-8 h-8 text-muted-foreground" />
            </div>
            <CardTitle className="text-xl">Đang phát triển</CardTitle>
          </CardHeader>
          <CardContent className="text-center">
            <p className="text-muted-foreground">{description}</p>
            <p className="text-sm text-muted-foreground mt-4">
              Tính năng này sẽ sớm được ra mắt trong các phiên bản tiếp theo.
            </p>
          </CardContent>
        </Card>
      </main>
    </div>
  );
};

// Individual pages
export const SucKhoePage: React.FC = () => (
  <ComingSoonPage 
    title="Sức khỏe Phạm nhân" 
    icon="🏥" 
    description="Quản lý thông tin sức khỏe, khám bệnh và điều trị cho phạm nhân."
  />
);

export const ThamGapPage: React.FC = () => (
  <ComingSoonPage 
    title="Thăm gặp - Tiếp tế" 
    icon="👥" 
    description="Quản lý lịch thăm gặp, tiếp tế từ gia đình và người thân."
  />
);

export const LaoDongPage: React.FC = () => (
  <ComingSoonPage 
    title="Lao động - Học tập" 
    icon="🎓" 
    description="Quản lý các hoạt động lao động sản xuất và chương trình học tập."
  />
);

export const SuCoPage: React.FC = () => (
  <ComingSoonPage 
    title="Sự cố - An ninh" 
    icon="⚠️" 
    description="Quản lý các sự cố an ninh, vi phạm và biện pháp xử lý."
  />
);
