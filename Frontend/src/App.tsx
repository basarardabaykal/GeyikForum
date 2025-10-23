import './App.css'
import { Routes, Route } from "react-router-dom"
import Navbar from './components/Navbar'
import { Toaster } from 'react-hot-toast'
import Homepage from './pages/Homepage'
import Signup from './pages/Signup'
import Login from './pages/Login'
import Profile from './pages/Profile'
import VerifyEmail from './pages/VerifyEmail'
import ForgotPassword from './pages/ForgotPassword'
import ResetPassword from './pages/ResetPassword'
import { useEffect } from 'react'
import ProtectedRoute from "@/routes/ProtectedRoute"

function App() {
  return (
    <>
      <Toaster position="top-right" />
      <Navbar></Navbar>
      <div>
        <Routes>
          <Route path="/signup" element={<Signup />} />
          <Route path="/login" element={<Login />} />
          <Route path="/forgot-password" element={<ForgotPassword />} />
          <Route path="/reset-password" element={<ResetPassword />} />
          <Route path="/verify-email" element={<VerifyEmail />} />
          <Route path="/" element={
            <ProtectedRoute>
              <Homepage />
            </ProtectedRoute>
          } />
          <Route path="/profile" element={
            <ProtectedRoute>
              <Profile />
            </ProtectedRoute>
          } />
        </Routes>
      </div>
    </>
  )
}

export default App
