import React from 'react';
import { cn } from '@/lib/utils';
import { Search, Plus } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';

interface DataTableProps<T> {
  data: T[];
  columns: {
    key: string;
    header: string;
    render?: (item: T, index: number) => React.ReactNode;
  }[];
  searchValue: string;
  onSearchChange: (value: string) => void;
  searchPlaceholder?: string;
  onAdd?: () => void;
  addLabel?: string;
  isLoading?: boolean;
  emptyMessage?: string;
  canAdd?: boolean;
}

export function DataTable<T extends { Id: number }>({
  data,
  columns,
  searchValue,
  onSearchChange,
  searchPlaceholder = 'Tìm kiếm...',
  onAdd,
  addLabel = 'Thêm mới',
  isLoading = false,
  emptyMessage = 'Chưa có dữ liệu',
  canAdd = true,
}: DataTableProps<T>) {
  return (
    <div className="bg-card rounded-xl border border-border shadow-sm animate-slide-up">
      <div className="p-4 border-b border-border">
        <div className="flex flex-col sm:flex-row gap-4 items-start sm:items-center justify-between">
          <div className="relative flex-1 max-w-md">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-muted-foreground" />
            <Input
              value={searchValue}
              onChange={(e) => onSearchChange(e.target.value)}
              placeholder={searchPlaceholder}
              className="pl-10"
            />
          </div>
          {canAdd && onAdd && (
            <Button onClick={onAdd} className="gap-2">
              <Plus className="w-4 h-4" />
              {addLabel}
            </Button>
          )}
        </div>
      </div>

      <div className="overflow-x-auto">
        <table className="w-full">
          <thead>
            <tr className="border-b border-border bg-muted/50">
              <th className="text-left p-4 font-semibold text-sm text-muted-foreground">
                STT
              </th>
              {columns.map((col) => (
                <th
                  key={col.key}
                  className="text-left p-4 font-semibold text-sm text-muted-foreground"
                >
                  {col.header}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {isLoading ? (
              <tr>
                <td
                  colSpan={columns.length + 1}
                  className="text-center py-12 text-muted-foreground"
                >
                  <div className="flex items-center justify-center gap-2">
                    <div className="w-5 h-5 border-2 border-primary border-t-transparent rounded-full animate-spin" />
                    Đang tải...
                  </div>
                </td>
              </tr>
            ) : data.length === 0 ? (
              <tr>
                <td
                  colSpan={columns.length + 1}
                  className="text-center py-12 text-muted-foreground"
                >
                  {emptyMessage}
                </td>
              </tr>
            ) : (
              data.map((item, index) => (
                <tr
                  key={item.Id}
                  className={cn(
                    "border-b border-border last:border-0",
                    "hover:bg-muted/30 transition-colors"
                  )}
                >
                  <td className="p-4 text-sm">{index + 1}</td>
                  {columns.map((col) => (
                    <td key={col.key} className="p-4 text-sm">
                      {col.render
                        ? col.render(item, index)
                        : (item as Record<string, unknown>)[col.key] as React.ReactNode}
                    </td>
                  ))}
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
