import { api } from "./api";

const CONTROLLER_NAME = "PostVote"

export const postVoteService = {
    async getAll() {
        const response = await api.get(`/${CONTROLLER_NAME}/GetAll`)
        return response
    }
}