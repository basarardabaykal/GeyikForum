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
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { z } from "zod"
import { useAuth } from "../context/AuthContext";
import { authService } from "../services/authService";
import { Loader2 } from "lucide-react"

const loginSchema = z.object({
  email: z.email("Geçersiz e-posta").nonempty("E-posta boş olamaz."),
  password: z.string("Geçersiz şifre").nonempty("Şifre boş olamaz."),
})

export default function Login() {
  const navigate = useNavigate();
  const { login } = useAuth();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [isError, setIsError] = useState<boolean>(false);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

  const handleSubmit = async () => {
    if (isSubmitting) return;
    setErrorMessage("");
    setIsError(false);

    const validation = loginSchema.safeParse({ email, password })
    if (!validation.success) {
      const issues = validation.error.issues
      const firstError = issues[0]?.message || "Geçersiz Bilgiler"
      setErrorMessage(firstError)
      setIsError(true)
      return
    }

    setIsSubmitting(true);
    try {
      const response = await authService.login(email, password)
      if (!response?.data?.success) {
        const msg = response.data?.message || "Giriş başarısız."
        const isUnverified = response.status === 403 || /doğrulanmamış|doğrulayın/i.test(msg)
        setIsError(true)
        setErrorMessage(isUnverified
          ? "E-posta adresiniz doğrulanmamış. Lütfen e-postanızı doğrulayıp tekrar giriş yapın."
          : msg)
        return
      }
      // Success → proceed
      login(response.data.data.token)
      setIsError(false)
      setErrorMessage(response.data.message)
      await new Promise((resolve) => setTimeout(resolve, 2000));
      navigate("/")
    } finally {
      setIsSubmitting(false)
    }
  }
  return (
    <>
      <div className="flex justify-center items-center align-middle h-screen">
        <Card className="max-w-sm m-auto w-3/4">
          <CardHeader>
            <CardTitle>Hesabınıza Giriş Yapın</CardTitle>
            <CardDescription>
              Hesabınıza giriş yapmak için e-posta ve şifrenizi girin
            </CardDescription>
          </CardHeader>
          <CardContent>
            <form onSubmit={(e) => { e.preventDefault(); handleSubmit(); }}>
              <div className="flex flex-col gap-4">
                <div className="grid gap-2">
                  <Label htmlFor="email">E-posta</Label>
                  <Input
                    id="email"
                    type="email"
                    required
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    disabled={isSubmitting}
                  />
                </div>
                <div className="grid gap-2">
                  <div className="flex items-center">
                    <Label htmlFor="password">Şifre</Label>
                  </div>
                  <Input
                    id="password"
                    type="password"
                    required
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    disabled={isSubmitting}
                  />
                </div>
              </div>
            </form>
          </CardContent>
          <CardFooter className="flex-col gap-2">
            {isError ? <p className="text-red-500">{errorMessage}</p> : <p className="text-green-500">{errorMessage}</p>}
          </CardFooter>
          <CardFooter className="flex-col gap-2">
            <Button
              type="submit"
              onClick={handleSubmit}
              className="w-full disabled:opacity-50 disabled:cursor-not-allowed"
              disabled={isSubmitting}
            >
              {isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
              {isSubmitting ? "Giriş yapılıyor..." : "Giriş Yap"}
            </Button>
          </CardFooter>
          <CardFooter className="flex justify-center">
            <CardAction className="flex justify-center items-center">
              <Button variant="link"><Link to={"/signup"}>Kayıt Ol</Link> </Button>
            </CardAction>
          </CardFooter>
          <CardFooter className="flex justify-center">
            <CardAction className="flex justify-center items-center">
              <Button
                variant="link"
                onClick={() => navigate('/forgot-password')}
              >
                Şifrenizi mi unuttunuz?
              </Button>
            </CardAction>
          </CardFooter>
        </Card>
      </div>

    </>
  )
}
