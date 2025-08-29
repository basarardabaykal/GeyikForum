import { api } from "./api";
import type { Post } from "@/models/Post";

const CONTROLLER_NAME = "post";

export const postService = {
    async getAll() {
        const response = await api.get(`/${CONTROLLER_NAME}/GetAll`); //pass token here later
        return response
    },

    async create(post: Post) {
        console.log(post);
    }
}