import { useState } from "react"
import { useNavigate } from "react-router-dom"
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

export default function ForgotPassword() {
  const navigate = useNavigate()
  const [email, setEmail] = useState("")
  const [message, setMessage] = useState<string>("E-posta adresinizi girin.")
  const [isError, setIsError] = useState(false)
  const [isLoading, setIsLoading] = useState(false)

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsLoading(true)
    setIsError(false)
    setMessage("Gönderiliyor...")

    try {
      const res = await authService.forgotPassword(email)
      if (res && res.status >= 200 && res.status < 300) {
        setMessage("Eğer e-posta kayıtlıysa, şifre sıfırlama bağlantısı gönderildi.")
        setIsError(false)
      } else {
        setMessage(res?.data?.message || "Bir hata oluştu. Lütfen tekrar deneyin.")
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
          <CardTitle>Şifre Sıfırlama</CardTitle>
          <CardDescription>
            E-posta adresinizi girin, size sıfırlama bağlantısı gönderelim.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={onSubmit} className="space-y-6">
            <div className="grid gap-2">
              <Label htmlFor="email">E-posta</Label>
              <Input
                id="email"
                type="email"
                placeholder="örnek@hacettepe.edu.tr"
                required
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>

            <CardFooter className="px-0">
              <p className={isError ? "text-sm text-red-500" : "text-sm text-green-600"}>
                {message}
              </p>
            </CardFooter>

            <CardFooter className="flex w-full flex-col gap-2 px-0">
              <Button type="submit" className="w-full" disabled={isLoading}>
                {isLoading ? "Gönderiliyor..." : "Bağlantı Gönder"}
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