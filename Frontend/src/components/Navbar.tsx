import { useEffect, useState } from "react"
import { href, Link, useNavigate } from "react-router-dom"
import { useAuth } from "../context/AuthContext"
//import { useTheme } from "../context/ThemeContext"
import { FloatingDock } from "./ui/floating-dock"
import { IconHome, IconUser, IconLogin2, IconLogout2, IconBrightnessDownFilled, IconBrightnessDown } from "@tabler/icons-react";


export default function Navbar() {
  const navigate = useNavigate()
  const { isAuthenticated, logout } = useAuth()
  //const { darkMode, toggleDarkMode } = useTheme()
  const [darkMode, toggleDarkMode] = useState(false)

  useEffect(() => {
    const handleStorageChange = (e: StorageEvent) => {
      if (e.key === "token") {
        window.location.reload()
      }
    }

    window.addEventListener("storage", handleStorageChange)

    return () => {
      window.removeEventListener("storage", handleStorageChange)
    }
  }, [])


  return (
    <>
      <div className="fixed left-1/2 -translate-x-1/2 bottom-4 z-50">
        <FloatingDock
          items={[
            {
              title: "Home",
              icon: <IconHome className="w-full h-full" />,
              onClick: () => {
                navigate("/")
              }
            },
            {
              title: "Profile",
              icon: <IconUser className="w-full h-full" />,
              onClick: () => {
                navigate("/profile")
              }
            },
            isAuthenticated ?
              {
                title: "Logout",
                icon: <IconLogout2 className="w-full h-full" />,
                onClick: () => {
                  logout()
                  navigate("/login")
                }
              } :

              {
                title: "Login",
                icon: <IconLogin2 className="w-full h-full" />,
                onClick: () => {
                  navigate("/login")
                }
              },

            darkMode ?
              {
                title: "Bright Mode",
                icon: <IconBrightnessDownFilled className="w-full h-full" />,
                onClick: () => {
                  toggleDarkMode(false)
                }
              } :
              {
                title: "Dark Mode",
                icon: <IconBrightnessDown className="w-full h-full" />,
                onClick: () => {
                  toggleDarkMode(true)
                }
              },

          ]}
        ></FloatingDock >
      </div>
    </>
  )
}