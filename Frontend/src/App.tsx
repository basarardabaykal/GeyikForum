import './App.css'
import { Routes, Route } from "react-router-dom"
import Navbar from './components/Navbar'
import { Toaster } from 'react-hot-toast'
import Homepage from './pages/Homepage'
import Signup from './pages/Signup'
import Login from './pages/Login'
import Profile from './pages/Profile'
import VerifyEmail from './pages/VerifyEmail'

function App() {

  return (
    <>
      <Toaster position="top-right" />
      <Navbar></Navbar>
      <div>
        <Routes>
          <Route path="/" element={<Homepage></Homepage>}></Route>
          <Route path="/signup" element={<Signup></Signup>}></Route>
          <Route path="/login" element={<Login></Login>}></Route>
          <Route path="/profile" element={<Profile></Profile>}></Route>
          <Route path="/verify-email" element={<VerifyEmail></VerifyEmail>}></Route>
        </Routes>
      </div>
    </>
  )
}

export default App
