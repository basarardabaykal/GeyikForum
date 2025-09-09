import { useState } from "react";
import { useAuth } from "../context/AuthContext";
import axios from "axios"
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
import { Link, useNavigate } from "react-router-dom";
import { set, z } from "zod"
import { authService } from "../services/authService";

const signupSchema = z.object({
  email: z.email("Geçersiz e-posta"),
  password: z.string()
    .min(6, "Şifre en az 6 karakter olmalıdır.")
    .max(64, "Şifre 64 karakterden fazla olamaz.")
    .regex(/[0-9]/, "Şifre en az bir rakam içermelidir.")
    .regex(/[A-Z]/, "Şifre en az bir büyük harf içermelidir.")
    .regex(/[a-z]/, "Şifre en az bir küçük harf içermelidir.")
    .regex(/[^A-Za-z0-9]/, "Şifre en az bir özel karakter içermelidir."),
  confirmedPassword: z.string(),
  nickname: z.string()
    .min(3, "Kullanıcı adı en az 3 karakter olmalıdır.")
    .max(20, "Kullanıcı adı 20 karakterden fazla olamaz.")
    .regex(/^[a-zA-Z0-9_-]+$/, "Kullanıcı adı sadece harf, rakam, alt çizgi veya tire içerebilir")
    .regex(/^(?![_-])(?!.*[_-]{2})(?!.*[_-]$).+$/, "Kullanıcı adı özel karakterle başlayamaz/bitemez veya ardışık özel karakter içeremez"),
}).refine((data) => data.password === data.confirmedPassword, {
  message: "Şifreler eşleşmiyor",
  path: ["confirmedPassword"],
})

export default function Signup() {
  const { login } = useAuth()
  const navigate = useNavigate()

  const [nickname, setNickname] = useState("")
  const [email, setEmail] = useState("")
  const [password, setPassword] = useState("")
  const [confirmPassword, setConfirmPassword] = useState("")

  const [errorMessage, setErrorMessage] = useState("")
  const [isError, setIsError] = useState<boolean>(false)

  const handleSubmit = async () => {
    setErrorMessage("")
    setIsError(false);

    const validation = signupSchema.safeParse({ nickname, email, password, confirmedPassword: confirmPassword, })

    if (!validation.success) {
      const issues = validation.error.issues
      const firstError = issues[0]?.message || "Geçersiz Bilgiler"
      setErrorMessage(firstError)
      setIsError(true)
      return
    }

    const response = await authService.register({
      email: email,
      password: password,
      confirmPassword: confirmPassword,
      nickname: nickname,
    })

    if (response.data.success) {
      login(response.data.data.token)
      window.dispatchEvent(new Event("storage"))
      setIsError(false)
      setErrorMessage(response.data.message)
      await new Promise((resolve) => setTimeout(resolve, 2000));
      navigate("/")
    }
    else {
      //backend validation errors
      if (response.data.errors && typeof response.data.errors === "object") {
        const allErrors = Object.values(response.data.errors).flat() as string[]
        setErrorMessage(allErrors[0] || "Doğrulama Hatası")
      }
      else {
        setErrorMessage(response.data.Message)
      }

      setIsError(true)
    }
  }
  return (
    <>
      <div className="flex justify-center items-center align-middle h-screen">
        <Card className="w-3/4 max-w-sm m-auto">
          <CardHeader>
            <CardTitle>Yeni hesap oluştur</CardTitle>
            <CardDescription>
              Yeni bir hesap oluşturmak için bilgilerinizi doldurun
            </CardDescription>
          </CardHeader>
          <CardContent>
            <form>
              <div className="flex flex-col gap-4">
                <div className="grid gap-2">
                  <Label htmlFor="email">E-posta</Label>
                  <Input
                    id="email"
                    type="email"
                    required
                    value={email}
                    onChange={(e) => (setEmail(e.target.value))}
                  />
                </div>
                <div className="grid gap-2">
                  <div className="flex items-center">
                    <Label htmlFor="password">Şifre</Label>
                  </div>
                  <Input id="password" type="password" required
                    value={password} onChange={(e) => (setPassword(e.target.value))} />
                </div>
                <div className="grid gap-2">
                  <div className="flex items-center">
                    <Label htmlFor="confirmPassword">Şifreyi Onayla</Label>
                  </div>
                  <Input id="confirmPassword" type="password" required
                    value={confirmPassword} onChange={(e) => (setConfirmPassword(e.target.value))} />
                </div>
                <div className="grid gap-2">
                  <Label htmlFor="nickname">Kullanıcı Adı</Label>
                  <Input
                    id="nickname"
                    type="text"
                    required
                    value={nickname}
                    onChange={(e) => (setNickname(e.target.value))}
                  />
                </div>
              </div>
            </form>
          </CardContent>
          <CardFooter>
            {isError ? <p className="text-red-500">{errorMessage}</p> : <p className="text-green-500">{errorMessage}</p>}
          </CardFooter>
          <CardFooter className="flex-col gap-2">
            <Button type="submit" onClick={handleSubmit} className="w-full">
              Kayıt Ol
            </Button>
          </CardFooter>
          <CardFooter className="flex justify-center">
            <CardAction className="flex justify-center items-center">
              <Button variant="link"><Link to={"/login"}>Giriş Yap</Link> </Button>
            </CardAction>
          </CardFooter>
        </Card>
      </div>
    </>
  )
}