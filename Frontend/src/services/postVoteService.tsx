import type { PostVote } from "@/models/PostVote";
import { api } from "./api";

const CONTROLLER_NAME = "PostVote"

export const postVoteService = {
    async getAll() {
        const response = await api.get(`/${CONTROLLER_NAME}/get-all`)
        return response
    },

    async createPostVote(postVote: PostVote) {
        try {
            const response = await api.post(`/${CONTROLLER_NAME}/create-post-vote`, postVote)
            return response
        } catch (error) {
            console.log(error) //replace with popup message etc
        }
    }
}