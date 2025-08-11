import { useEffect, useState } from "react"
import { postService } from "../services/postService"

export default function Homepage() {
  const [posts, setPosts] = useState()

  useEffect(() => {
    const fetchPosts = () => {

      const posts = postService.getAll()
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