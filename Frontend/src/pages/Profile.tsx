import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "../components/ui/card"
import { Button } from "../components/ui/button"
import { useNavigate } from "react-router-dom"
import { useAuth } from "@/context/AuthContext"
import { useEffect, useState } from "react"
import { authService } from "../services/authService"

export default function Profile() {
  const { user } = useAuth()
  const navigate = useNavigate()
  const [message, setMessage] = useState<string>("")
  const [isError, setIsError] = useState<boolean>(false)
  const [isSending, setIsSending] = useState<boolean>(false)

  useEffect(() => {
    if (!user) navigate("/login")
  }, [user, navigate])

  const handleSendResetEmail = async () => {
    if (!user?.email) {
      setMessage("Kullanıcı e-postası bulunamadı.")
      setIsError(true)
      return
    }
    setIsSending(true)
    setIsError(false)
    setMessage("Gönderiliyor...")

    const res = await authService.forgotPassword(user.email)
    if (res && res.status >= 200 && res.status < 300) {
      setMessage("Eğer e-posta kayıtlıysa, şifre sıfırlama bağlantısı gönderildi.")
      setIsError(false)
    } else {
      setMessage(res?.data?.message || "Bir hata oluştu. Lütfen tekrar deneyin.")
      setIsError(true)
    }
    setIsSending(false)
  }

  if (!user) return null

  return (
    <div className="flex h-screen items-center justify-center">
      <Card className="m-auto w-3/4 max-w-xl">
        <CardHeader>
          <CardTitle>Profil</CardTitle>
          <CardDescription>Hesap bilgilerinizi görüntüleyin ve işlemleri yönetin.</CardDescription>
        </CardHeader>

        <CardContent>
          <div className="grid gap-3">
            <div className="flex items-center justify-between">
              <span className="text-sm text-muted-foreground">Kullanıcı Adı</span>
              <span className="font-medium">{user.nickname}</span>
            </div>
            <div className="flex items-center justify-between">
              <span className="text-sm text-muted-foreground">E-posta</span>
              <span className="font-medium">{user.email}</span>
            </div>
          </div>

          {message && (
            <p className={`mt-4 text-sm ${isError ? "text-red-500" : "text-green-600"}`}>
              {message}
            </p>
          )}
        </CardContent>

        <CardFooter className="flex flex-col justify-between gap-3 sm:flex-row">
          <Button onClick={handleSendResetEmail} disabled={isSending} className="w-full sm:w-auto">
            {isSending ? "Gönderiliyor..." : "Şifre Sıfırlama Bağlantısı Gönder"}
          </Button>
          <Button
            type="button"
            variant="secondary"
            onClick={() => navigate("/")}
            className="w-full sm:w-auto"
          >
            Ana sayfaya dön
          </Button>
        </CardFooter>
      </Card>
    </div>
  )
}