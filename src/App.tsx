import { Toaster } from "@/components/ui/toaster";
import { Toaster as Sonner } from "@/components/ui/sonner";
import { TooltipProvider } from "@/components/ui/tooltip";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthProvider } from "@/contexts/AuthContext";
import { ProtectedRoute } from "@/components/ProtectedRoute";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import CanBoManagement from "./pages/CanBoManagement";
import PhamNhanManagement from "./pages/PhamNhanManagement";
import KhenThuongKyLuatManagement from "./pages/KhenThuongKyLuatManagement";
import BaoCaoThongKe from "./pages/BaoCaoThongKe";
import { SucKhoePage, ThamGapPage, LaoDongPage, SuCoPage } from "./pages/ComingSoonPages";
import NotFound from "./pages/NotFound";

const queryClient = new QueryClient();

const App = () => (
  <QueryClientProvider client={queryClient}>
    <AuthProvider>
      <TooltipProvider>
        <Toaster />
        <Sonner />
        <BrowserRouter>
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="/" element={<ProtectedRoute><Dashboard /></ProtectedRoute>} />
            <Route path="/can-bo" element={<ProtectedRoute><CanBoManagement /></ProtectedRoute>} />
            <Route path="/pham-nhan" element={<ProtectedRoute><PhamNhanManagement /></ProtectedRoute>} />
            <Route path="/khen-thuong-ky-luat" element={<ProtectedRoute><KhenThuongKyLuatManagement /></ProtectedRoute>} />
            <Route path="/bao-cao" element={<ProtectedRoute><BaoCaoThongKe /></ProtectedRoute>} />
            <Route path="/suc-khoe" element={<ProtectedRoute allowedRoles={['Admin', 'CanBo']}><SucKhoePage /></ProtectedRoute>} />
            <Route path="/tham-gap" element={<ProtectedRoute allowedRoles={['Admin', 'CanBo']}><ThamGapPage /></ProtectedRoute>} />
            <Route path="/lao-dong" element={<ProtectedRoute allowedRoles={['Admin', 'CanBo']}><LaoDongPage /></ProtectedRoute>} />
            <Route path="/su-co" element={<ProtectedRoute allowedRoles={['Admin', 'CanBo']}><SuCoPage /></ProtectedRoute>} />
            <Route path="*" element={<NotFound />} />
          </Routes>
        </BrowserRouter>
      </TooltipProvider>
    </AuthProvider>
  </QueryClientProvider>
);

export default App;
