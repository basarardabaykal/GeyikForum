import { Navigate, Outlet, useLocation } from "react-router-dom"
import LoadingScreen from "@/components/LoadingScreen"
import { useAuth } from "@/context/AuthContext"

export default function ProtectedRoute({ children }: { children?: React.ReactNode }) {
    const { isAuthenticated, authReady } = useAuth()
    const location = useLocation()

    if (!authReady) return <LoadingScreen />
    if (!isAuthenticated) return <Navigate to="/login" replace state={{ from: location.pathname }} />

    return children ?? <Outlet />
}