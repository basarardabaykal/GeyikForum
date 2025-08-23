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
import axios from "axios"
import { Link, useNavigate } from "react-router-dom";
import { z } from "zod"
import { useAuth } from "../context/AuthContext";
import { authService } from "../services/authService";

const loginSchema = z.object({
  email: z.email("Invalid email adress"),
  password: z.string("Invalid password"),
})

export default function Login() {
  const navigate = useNavigate();
  const { login } = useAuth();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [isError, setIsError] = useState<boolean>(false);

  const handleSubmit = async () => {
    setErrorMessage("");
    setIsError(false);

    const validation = loginSchema.safeParse({ email, password })
    if (!validation.success) {
      const issues = validation.error.issues
      const firstError = issues[0]?.message || "Invalid input"
      setErrorMessage(firstError)
      setIsError(true)
      return
    }

    try {
      const response = await authService.login(email, password)
      if (response.data.success) {
        login(response.data.data.token)
        setIsError(false)
        setErrorMessage("Successfully logged in, you will be redirected shortly.")
        await new Promise((resolve) => setTimeout(resolve, 2000));
        navigate("/")
      }
      else {
        setErrorMessage("Login failed")
        setIsError(true);
      }
    } catch (error) {
      if (axios.isAxiosError(error)) {
        setErrorMessage(error.response?.data?.message || error.message)
      } else if (error instanceof Error) {
        setErrorMessage(error.message)
      } else {
        setErrorMessage("An unknown error occurred")
      }
      setIsError(true)
    }
  }
  return (
    <>
      <div className="flex justify-center items-center align-middle h-screen">
        <Card className="max-w-sm m-auto w-3/4">
          <CardHeader>
            <CardTitle>Login to your account</CardTitle>
            <CardDescription>
              Enter your email and password to login to your account
            </CardDescription>
          </CardHeader>
          <CardContent>
            <form>
              <div className="flex flex-col gap-6">
                <div className="grid gap-2">
                  <Label htmlFor="email">Email</Label>
                  <Input
                    id="email"
                    type="email"
                    placeholder="m@example.com"
                    required
                    value={email}
                    onChange={(e) => (setEmail(e.target.value))}
                  />
                </div>
                <div className="grid gap-2">
                  <div className="flex items-center">
                    <Label htmlFor="password">Password</Label>
                  </div>
                  <Input id="password" type="password" required
                    value={password} onChange={(e) => (setPassword(e.target.value))} />
                </div>
              </div>
            </form>
          </CardContent>
          <CardFooter>
            {isError ? <p className="text-red-500">{errorMessage}</p> : <p className="text-green-500">{errorMessage}</p>}
          </CardFooter>
          <CardFooter className="flex-col gap-2">
            <Button type="submit" onClick={handleSubmit} className="w-full">
              Login
            </Button>
          </CardFooter>
          <CardFooter className="flex justify-center">
            <CardAction className="flex justify-center items-center">
              <Button variant="link"><Link to={"/signup"}>Sign Up</Link> </Button>
            </CardAction>
          </CardFooter>
        </Card>
      </div>

    </>
  )
}
