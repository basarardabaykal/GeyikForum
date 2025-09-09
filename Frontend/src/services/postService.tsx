import type { PostVote } from "@/models/PostVote";
import { api } from "./api";
import type { Post } from "@/models/Post";

const CONTROLLER_NAME = "post";

export const postService = {
  async getAll() {
    try {
      const token = localStorage.getItem("token")
      const response = await api.get(`/${CONTROLLER_NAME}/get-all`, {
        headers: {
          Authorization: `Bearer ${token}`,
        }
      });
      return response
    } catch (error) {
      console.log(error) //replace with popup message etc
    }
  },

  async createPost(post: Post) {
    try {
      const token = localStorage.getItem("token")
      const response = await api.post(`/${CONTROLLER_NAME}/create-post`, post, {
        headers: {
          Authorization: `Bearer ${token}`,
        }
      });
      return response
    } catch (error) {
      console.log(error) //replace with popup message etc
    }
  },

  async votePost(postVote: PostVote) {
    try {
      const token = localStorage.getItem("token")
      const response = await api.patch(`/${CONTROLLER_NAME}/vote-post`, postVote, {
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