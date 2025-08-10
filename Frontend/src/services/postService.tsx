import { api } from "./api";

const CONTROLLER_NAME = "post";

export const postService = {
    async getAll() {
        const response = await api.get(`/${CONTROLLER_NAME}/GetAll`); //pass token here later
        console.log(response)
        return response
    }
}