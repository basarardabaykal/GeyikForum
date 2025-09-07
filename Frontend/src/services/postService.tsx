import type { PostVote } from "@/models/PostVote";
import { api } from "./api";
import type { Post } from "@/models/Post";
import { useAuth } from "@/context/AuthContext";
import { useNavigate } from "react-router-dom";

const CONTROLLER_NAME = "post";
const { isAuthenticated } = useAuth();
const navigate = useNavigate()

export const postService = {
  async getAll() {
    if (!isAuthenticated) {
      navigate("/login")
    }
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
    const token = localStorage.getItem("token")
    const response = await api.get(`/${CONTROLLER_NAME}/get-all`, {
      headers: {
        Authorization: `Bearer ${token}`,
      }
    });
    return response
  },

  async createPost(post: Post) {
    if (!isAuthenticated) {
      navigate("/login")
    }
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
    if (!isAuthenticated) {
      navigate("/login")
    }
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