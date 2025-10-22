import { useEffect, useState } from "react"
import { useNavigate, useSearchParams } from "react-router-dom"
import { authService } from "../services/authService"
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "../components/ui/card"
import { Button } from "../components/ui/button"
import { Input } from "../components/ui/input"
import { Label } from "../components/ui/label"

export default function ResetPassword() {
  const navigate = useNavigate()
  const [searchParams] = useSearchParams()
  const [userId, setUserId] = useState("")
  const [token, setToken] = useState("")
  const [newPassword, setNewPassword] = useState("")
  const [confirmPassword, setConfirmPassword] = useState("")
  const [message, setMessage] = useState("Yeni şifrenizi belirleyin.")
  const [isError, setIsError] = useState(false)
  const [isLoading, setIsLoading] = useState(false)

  useEffect(() => {
    const uid = searchParams.get("userId") || ""
    const t = searchParams.get("token") || ""
    setUserId(uid)
    setToken(t)
  }, [searchParams])

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    if (!userId || !token) {
      setMessage("Geçersiz bağlantı.")
      setIsError(true)
      return
    }

    if (newPassword !== confirmPassword) {
      setMessage("Şifreler eşleşmiyor.")
      setIsError(true)
      return
    }

    setIsLoading(true)
    setIsError(false)
    setMessage("Gönderiliyor...")

    try {
      const res = await authService.resetPassword(userId, token, newPassword, confirmPassword)
      if (res?.data?.success) {
        setMessage("Şifre başarıyla sıfırlandı. Giriş yapabilirsiniz.")
        setIsError(false)
      } else {
        setMessage(res?.data?.message || "Şifre sıfırlama başarısız.")
        setIsError(true)
      }
    } catch {
      setMessage("Bir hata oluştu. Lütfen tekrar deneyin.")
      setIsError(true)
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className="flex h-screen items-center justify-center">
      <Card className="m-auto w-3/4 max-w-sm">
        <CardHeader>
          <CardTitle>Şifreyi Sıfırla</CardTitle>
          <CardDescription>Yeni şifrenizi girerek hesabınızı güncelleyin.</CardDescription>
        </CardHeader>

        <CardContent>
          <form onSubmit={onSubmit} className="space-y-6">
            <div className="grid gap-2">
              <Label htmlFor="newPassword">Yeni şifre</Label>
              <Input
                id="newPassword"
                type="password"
                placeholder="Yeni şifre"
                required
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
              />
            </div>

            <div className="grid gap-2">
              <Label htmlFor="confirmPassword">Yeni şifre (tekrar)</Label>
              <Input
                id="confirmPassword"
                type="password"
                placeholder="Yeni şifre (tekrar)"
                required
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
              />
            </div>

            <CardFooter className="px-0">
              <p className={isError ? "text-sm text-red-500" : "text-sm text-green-600"}>
                {message}
              </p>
            </CardFooter>

            <CardFooter className="flex w-full flex-col gap-2 px-0">
              <Button type="submit" className="w-full" disabled={isLoading}>
                {isLoading ? "Gönderiliyor..." : "Şifreyi Sıfırla"}
              </Button>
              <Button
                type="button"
                variant="link"
                onClick={() => navigate("/login")}
                className="w-full"
              >
                Girişe dön
              </Button>
            </CardFooter>
          </form>
        </CardContent>
      </Card>
    </div>
  )
}