import { api } from "./api";
import { toast } from "react-hot-toast";

const CONTROLLER_NAME = "auth"

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
    try {
      const token = localStorage.getItem("token")
      const response = await api.get(`/${CONTROLLER_NAME}/get-current-user`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          }
        })

      if (!response.data.success) {
        toast.error(response.data.message)
      }

      return response
    } catch (error: any) {
      toast.error("Bir hata ile karşılaşıldı.")
      return null
    }
  }
}