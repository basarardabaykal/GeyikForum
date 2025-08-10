import { useEffect } from 'react'
import './App.css'
import { postService } from './services/postService'

function App() {

  useEffect(() => {
    const fetchPosts = () => {
      postService.getAll()
    }

    fetchPosts()
  }, [])

  return (
    <>
      <div className='bg-green-500 w-full h-full'>
        <p>Hello World</p>
      </div>
    </>
  )
}

export default App
