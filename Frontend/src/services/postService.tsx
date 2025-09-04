import type { PostVote } from "@/models/PostVote";
import { api } from "./api";
import type { Post } from "@/models/Post";

const CONTROLLER_NAME = "post";

export const postService = {
  async getAll() {
    const response = await api.get(`/${CONTROLLER_NAME}/get-all`); //pass token here later
    return response
  },

  async createPost(post: Post) {
    try {
      const response = await api.post(`/${CONTROLLER_NAME}/create-post`, post);
      return response
    } catch (error) {
      console.log(error) //replace with popup message etc
    }
  },

  async votePost(postVote: PostVote) {
    try {
      const response = await api.patch(`/${CONTROLLER_NAME}/vote-post`, postVote)
      return response
    } catch (error) {
      console.log(error) //replace with popup message etc
    }
  }
}