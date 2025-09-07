import { api } from "./api";
import { useAuth } from "@/context/AuthContext";
import { useNavigate } from "react-router-dom";

const CONTROLLER_NAME = "user";
const { isAuthenticated } = useAuth();
const navigate = useNavigate()

export const userService = {

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

  }
}