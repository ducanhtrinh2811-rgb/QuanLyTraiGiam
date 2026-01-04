// API Configuration - Update this URL to match your C# backend
export const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:7000/api';

// Helper to get auth token
export const getAuthToken = (): string | null => {
  const user = localStorage.getItem('auth_user');
  if (user) {
    const parsed = JSON.parse(user);
    return parsed.token || null;
  }
  return null;
};
