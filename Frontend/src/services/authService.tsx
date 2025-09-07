import { api } from "./api";
import { useAuth } from "@/context/AuthContext";
import { useNavigate } from "react-router-dom";

const CONTROLLER_NAME = "auth"
const { isAuthenticated } = useAuth();
const navigate = useNavigate()

export const authService = {

  async login(email: string, password: string) {
    try {
      const response = await api.post(`/${CONTROLLER_NAME}/login`, {
        email,
        password,
      })
      return response
    } catch (error: any) {
      if (error.response) {
        return error.response
      }
    }

  },

  async register(data: any) {
    try {
      const response = await api.post(`/${CONTROLLER_NAME}/register`, data)
      return response
    } catch (error: any) {
      if (error.response) {
        return error.response
      }
    }
  },

  async getCurrentUser() {
    if (!isAuthenticated) {
      navigate("/login")
    }

    try {
      const token = localStorage.getItem("token")
      const response = await api.get(`/${CONTROLLER_NAME}/get-current-user`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          }
        })
      return response
    } catch (error) {
      console.log(error) //replace with popup message etc
    }

  }
}