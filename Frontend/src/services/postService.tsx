import type { PostVote } from "@/models/PostVote";
import { api } from "./api";
import { toast } from "react-hot-toast";
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

      if (!response.data.success) {
        toast.error(response.data.message)
      }

      return response
    } catch (error) {
      toast.error("Bir hata ile karşılaşıldı.")
      return
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

      if (!response.data.success) {
        toast.error(response.data.message)
      }

      return response
    } catch (error) {
      toast.error("Bir hata ile karşılaşıldı.")
      return
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

      if (!response.data.success) {
        toast.error(response.data.message)
      }

      return response
    } catch (error) {
      toast.error("Bir hata ile karşılaşıldı.")
    }
  }
}