import { useEffect, useState } from "react"
import { href, Link, useNavigate } from "react-router-dom"
import { useAuth } from "../context/AuthContext"
import { useTheme } from "../context/ThemeContext"
import { FloatingDock } from "./ui/floating-dock"
import { IconHome, IconUser, IconLogin2, IconLogout2, IconBrightnessDownFilled, IconBrightnessDown } from "@tabler/icons-react";
import logo from "../../public/logo.svg"


export default function Navbar() {
  const navigate = useNavigate()
  const { isAuthenticated, logout } = useAuth()
  const { darkMode, toggleDarkMode } = useTheme()

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
      <div
        className="
          fixed z-50
          right-4 bottom-[max(1rem,env(safe-area-inset-bottom))]
          translate-x-0
          md:right-40 md:bottom-1/4 md:-translate-x-1/2
        "
      >
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
              title: "Profil",
              icon: <IconUser className="w-full h-full" />,
              onClick: () => {
                navigate("/profile")
              }
            },
            isAuthenticated ?
              {
                title: "Çıkış",
                icon: <IconLogout2 className="w-full h-full" />,
                onClick: () => {
                  logout()
                  navigate("/login")
                }
              } :

              {
                title: "Giriş",
                icon: <IconLogin2 className="w-full h-full" />,
                onClick: () => {
                  navigate("/login")
                }
              },

            darkMode ?
              {
                title: "Aydınlık Tema",
                icon: <IconBrightnessDownFilled className="w-full h-full" />,
                onClick: () => {
                  toggleDarkMode()
                }
              } :
              {
                title: "Karanlık Tema",
                icon: <IconBrightnessDown className="w-full h-full" />,
                onClick: () => {
                  toggleDarkMode()
                }
              },
            {
              title: "Harget",
              icon: <img src={logo} alt="" />,
              onClick: () => {
                window.open("https://harget.com.tr", "_blank")
              }
            },

          ]}
        ></FloatingDock >
      </div>
    </>
  )
}