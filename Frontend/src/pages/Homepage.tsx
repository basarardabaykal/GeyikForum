import { useEffect, useState } from "react"
import { postService } from "../services/postService"
import type { Post } from "../models/Post"

export default function Homepage() {
  const [posts, setPosts] = useState<Post[]>([])

  const fetchPosts = async () => {
    const response = await postService.getAll()
    if (response.data.success) {
      const mappedPosts = response.data.data
      console.log(mappedPosts)
      setPosts(mappedPosts)
    }
  }

  useEffect(() => {
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