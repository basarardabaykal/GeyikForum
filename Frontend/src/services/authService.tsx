import { api } from "./api";

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

  async get(token: string) {
    const response = await api.post(`/${CONTROLLER_NAME}/get`, {
      token,
    },
      {
        headers: {
          Authorization: `Bearer ${token}`,
        }
      })
    return response
  }
}