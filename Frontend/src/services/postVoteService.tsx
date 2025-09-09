import { api } from "./api";
import { toast } from "react-hot-toast";

const CONTROLLER_NAME = "PostVote"

export const postVoteService = {
  async getAll() {
    try {
      const token = localStorage.getItem("token")
      const response = await api.get(`/${CONTROLLER_NAME}/get-all`, {
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
  },
}