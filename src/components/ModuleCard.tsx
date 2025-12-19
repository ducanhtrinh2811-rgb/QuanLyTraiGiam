import React from 'react';
import { Link } from 'react-router-dom';
import { cn } from '@/lib/utils';

interface ModuleCardProps {
  icon: React.ReactNode;
  title: string;
  description: string;
  to: string;
  delay?: number;
}

export const ModuleCard: React.FC<ModuleCardProps> = ({
  icon,
  title,
  description,
  to,
  delay = 0,
}) => {
  return (
    <Link
      to={to}
      className={cn(
        "block bg-card rounded-xl p-6 card-elevated",
        "border border-border/50",
        "group cursor-pointer animate-slide-up"
      )}
      style={{ animationDelay: `${delay}ms` }}
    >
      <div className="flex flex-col items-start gap-3">
        <div className="text-4xl group-hover:scale-110 transition-transform duration-300">
          {icon}
        </div>
        <div>
          <h3 className="font-semibold text-lg text-card-foreground group-hover:text-primary transition-colors">
            {title}
          </h3>
          <p className="text-sm text-muted-foreground mt-1">
            {description}
          </p>
        </div>
      </div>
    </Link>
  );
};
