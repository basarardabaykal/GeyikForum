import './App.css'
import { Routes, Route } from "react-router-dom"
import Homepage from './pages/Homepage'
import Signup from './pages/Signup'
import Login from './pages/Login'

function App() {

  return (
    <>
      <div>
        <Routes>
          <Route path="/" element={<Homepage></Homepage>}></Route>
          <Route path="/signup" element={<Signup></Signup>}></Route>
          <Route path="/login" element={<Login></Login>}></Route>
        </Routes>
      </div>
      <Homepage></Homepage>
    </>
  )
}

export default App
