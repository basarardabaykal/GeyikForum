import { api } from "./api";

const CONTROLLER_NAME = "auth"

export const authService = {
    async login(email: string, password: string) {
        const response = await api.post(`/${CONTROLLER_NAME}/login`, {
            email,
            password,
        })
        return response
    },
    async register(data: any) {
        const response = await api.post(`/${CONTROLLER_NAME}/register`, data)
        return response
    },
}