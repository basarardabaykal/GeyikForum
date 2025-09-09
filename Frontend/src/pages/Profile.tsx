import {
  Card,
  CardAction,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "../components/ui/card"
import { Button } from "../components/ui/button"
import { Input } from "../components/ui/input"
import { Label } from "../components/ui/label"

import { useNavigate } from "react-router-dom"
import { useAuth } from "@/context/AuthContext"

export default function Profile() {
  const { user } = useAuth()
  const navigate = useNavigate()

  if (!user) {
    navigate("/login")
  }
  else {
    return (
      <>
        <div className="flex justify-center items-center align-middle h-screen">
          <Card className="max-w-5xl m-auto w-3/4">
            <CardHeader>
              <CardTitle>Profil</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="flex flex-row">
                <span>Kullanıcı Adı: </span>
                <span>{user.nickname}</span>
              </div>
            </CardContent>
          </Card>
        </div>

      </>
    )
  }
}