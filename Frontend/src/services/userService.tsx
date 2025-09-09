import { api } from "./api";

const CONTROLLER_NAME = "user";

export const userService = {

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

  }
}