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
import { Loader2 } from "lucide-react"

export default function ForgotPassword() {
  const navigate = useNavigate()
  const [email, setEmail] = useState("")
  const [message, setMessage] = useState<string>("E-posta adresinizi girin.")
  const [isError, setIsError] = useState(false)
  const [isLoading, setIsLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (isLoading) return
    setMessage("")
    setIsError(false)
    setIsLoading(true)
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
          <form onSubmit={handleSubmit} className="space-y-6">
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
              <Button type="submit" className="w-full disabled:opacity-50 disabled:cursor-not-allowed" disabled={isLoading}>
                {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                {isLoading ? "Gönderiliyor..." : "Bağlantı Gönder"}
              </Button>
              <Button
                type="button"
                variant="link"
                onClick={() => navigate("/login")}
                className="w-full"
                disabled={isLoading}
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