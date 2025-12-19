import React from 'react';
import { ArrowLeft } from 'lucide-react';
import { Link } from 'react-router-dom';
import { cn } from '@/lib/utils';

interface PageHeaderProps {
  title: string;
  subtitle?: string;
  icon?: React.ReactNode;
  showBackButton?: boolean;
  backTo?: string;
  action?: React.ReactNode;
}

export const PageHeader: React.FC<PageHeaderProps> = ({
  title,
  subtitle = 'Trại Giam Số 1',
  icon,
  showBackButton = false,
  backTo = '/',
  action,
}) => {
  return (
    <header className="gradient-header text-primary-foreground">
      <div className="container mx-auto px-4 py-6">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-4">
            {icon && (
              <div className="text-3xl animate-fade-in">{icon}</div>
            )}
            <div>
              <h1 className="text-2xl md:text-3xl font-bold tracking-tight animate-fade-in">
                {title}
              </h1>
              <p className="text-primary-foreground/80 text-sm mt-1 animate-fade-in">
                {subtitle}
              </p>
            </div>
          </div>
          <div className="flex items-center gap-3">
            {action}
            {showBackButton && (
              <Link
                to={backTo}
                className={cn(
                  "inline-flex items-center gap-2 px-4 py-2 rounded-lg",
                  "bg-primary-foreground/10 hover:bg-primary-foreground/20",
                  "text-primary-foreground font-medium text-sm",
                  "transition-all duration-200 backdrop-blur-sm",
                  "border border-primary-foreground/20"
                )}
              >
                <ArrowLeft className="w-4 h-4" />
                Về trang chính
              </Link>
            )}
          </div>
        </div>
      </div>
    </header>
  );
};
