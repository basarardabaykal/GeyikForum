import { api } from "./api";

const CONTROLLER_NAME = "user";

export const userService = {
    async getAll() {
        const response = await api.get(`/${CONTROLLER_NAME}/get-all`); // pass token here later
        return response
    }
}