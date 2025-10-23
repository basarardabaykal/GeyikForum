import { createContext, useContext, useEffect, useState } from "react";
import type { User } from "../models/User"
import { jwtDecode } from "jwt-decode"
import axios from "axios";
import { authService } from "../services/authService";
import { useNavigate } from "react-router-dom";
//import { toast } from "react-hot-toast"

interface DecodedToken {
  exp: number
}

interface AuthContextType {
  user: User | null
  isAuthenticated: boolean;
  login: (token: string) => void;
  logout: () => void;
  isAdmin: () => boolean;
  authReady: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [authReady, setAuthReady] = useState(false);
  const navigate = useNavigate()


  const fetchUser = async () => {
    setAuthReady(false);
    const token = localStorage.getItem("token")
    if (!token) {
      setUser(null);
      setIsAuthenticated(false);
      setAuthReady(true);
      return
    }
    try {
      const decoded: DecodedToken = jwtDecode(token)
      if (decoded.exp * 1000 < Date.now()) {
        logout()
        setAuthReady(true)
        return
      }

      const response = await authService.getCurrentUser()
      if (!response || !response.data?.success) {
        logout()
        setAuthReady(true)
        return
      }

      const userData = response.data.data
      const user: User = {
        id: userData.id,
        email: userData.email,
        nickname: userData.nickname,
        karma: userData.karma,
        isAdmin: userData.isAdmin,
        isModerator: userData.isModerator,
        isBanned: userData.isBanned,
        roles: userData.roles,
      };

      setUser(user)
      setIsAuthenticated(true)
    } catch (error) {
      logout()
    } finally {
      setAuthReady(true)
    }
  }

  useEffect(() => {
    fetchUser()
  }, []);

  const login = async (token: string) => {
    localStorage.setItem("token", token)
    await fetchUser()
  };

  const logout = () => {
    localStorage.removeItem("token");
    setUser(null);
    setIsAuthenticated(false);
  };

  const isAdmin = () => {
    return user?.roles.includes("Admin") ?? false;
  }

  return (
    <AuthContext.Provider value={{ user, isAuthenticated, login, logout, isAdmin, authReady }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth must be used within AuthProvider");
  return context;
};
