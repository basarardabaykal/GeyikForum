import { useEffect, useState } from "react"
import { postService } from "../services/postService"
import { postVoteService } from "../services/postVoteService"
import { useAuth } from "@/context/AuthContext"
import { useNavigate } from "react-router-dom";
import toast from "react-hot-toast";
import type { Post } from "../models/Post"
import type { User } from "../models/User"
import PostItem from "../components/PostItem"
import PostCreator from "@/components/PostCreator"
import { MessageCircle } from "lucide-react"
import { userService } from "../services/userService"
import type { PostVote } from "../models/PostVote"


export default function Homepage() {
  const { isAuthenticated, user } = useAuth()
  const navigate = useNavigate()
  const [posts, setPosts] = useState<Post[]>([])
  const [users, setUsers] = useState<User[]>([])
  const [postVotes, setPostVotes] = useState<PostVote[]>([])
  const [loading, setLoading] = useState<boolean>(true)

  const fetchUsers = async () => {
    if (!isAuthenticated) {
      navigate("/login")
    }
    const response = await userService.getAll()
    if (response?.data.success) {
      const mappedUsers: User[] = response.data.data
      setUsers(mappedUsers)
    }
  }

  const fetchPostVotes = async (): Promise<void> => {
    if (!isAuthenticated) {
      navigate("/login")
    }
    const response = await postVoteService.getAll()
    if (response?.data.success) {
      const mappedPostVotes: PostVote[] = response.data.data
      setPostVotes(mappedPostVotes)
    }
  }

  const fetchPosts = async (): Promise<void> => {
    if (!isAuthenticated) {
      navigate("/login")
    }
    const response = await postService.getAll()
    if (response?.data.success) {
      const mappedPosts: Post[] = response.data.data
      setPosts(mappedPosts)
    }
  }

  const getUserNickname = (userId: string): string => {
    const user = users.find(u => {
      return u.id === userId
    })

    return user?.nickname || "anon"
  }

  const getUserVoteForPost = (postId: string): number => {
    const userVote = postVotes.find(v => {
      return (v.postId == postId) && (v.userId == user?.id)
    })
    return userVote?.voteValue || 0
  }

  const handleVote = async (postId: string, newVote: number): Promise<void> => {
    if (!isAuthenticated) {
      navigate("/login")
    }

    const newPostVote: PostVote = {
      id: "00000000-0000-0000-0000-000000000000",
      userId: user?.id || "00000000-0000-0000-0000-000000000000",
      postId: postId,
      voteValue: newVote,
    }

    const response = await postService.votePost(newPostVote)

    if (response?.data?.data?.voteScore != undefined) {
      setPosts(prevPosts =>
        prevPosts.map(post =>
          post.id === postId ? { ...post, voteScore: response.data.data.voteScore } : post
        )
      )
    }
  }


  const handleCreatePost = async (parentId: string, depth: number, title: string, content: string): Promise<void> => {
    if (!isAuthenticated) {
      navigate("/login")
    }

    const newPost: Post = {
      id: "00000000-0000-0000-0000-000000000000",
      userId: user?.id || "00000000-0000-0000-0000-000000000000",
      parentId: parentId || null,
      depth: depth,
      title: title,
      content: content,
      voteScore: 0,
      commentCount: 0,
      isPinned: false,
      isEdited: false,
      isDeleted: false,
    }

    const response = await postService.createPost(newPost)
  }


  useEffect(() => {
    if (!isAuthenticated) {
      navigate("/login")
      toast.error("İçerikleri görebilmek için giriş yapın.")
      return
    }

    const fetchData = async () => {
      try {
        await Promise.all([fetchUsers(), fetchPostVotes(), fetchPosts()])
      } catch (error) {
        toast.error("Veri aktarımında bir hatayla karşılaşıldı.")
      } finally {
        setLoading(false)
      }
    }

    fetchData()
  }, [])

  const mainPosts: Post[] = posts
    .filter(post => post.parentId === null)
    .sort((a, b) => {
      // Pinned post logic might change later if I decide to GET posts by small amounts.
      if (a.isPinned && !b.isPinned) return -1
      if (!a.isPinned && b.isPinned) return 1
      return b.voteScore - a.voteScore
    })

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Loading posts...</p>
        </div>
      </div>
    )
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="bg-white border-b border-gray-200 sticky top-0 z-10">
        <div className="max-w-4xl mx-auto px-4 py-3">
          <h1 className="text-xl font-bold text-gray-900">Discussion Forum</h1>
        </div>
      </header>

      <PostCreator isOpen={true} parentId="" depth={0} onSubmit={handleCreatePost}></PostCreator>

      <main className="max-w-4xl mx-auto px-4 py-6">
        {mainPosts.length === 0 ? (
          <div className="text-center py-12">
            <MessageCircle size={48} className="mx-auto text-gray-400 mb-4" />
            <p className="text-gray-600">No posts yet. Be the first to start a discussion!</p>
          </div>
        ) : (
          <div className="space-y-0">
            {mainPosts.map(post => (
              <PostItem
                key={post.id}
                post={post}
                posts={posts}
                onVote={handleVote}
                getUserNickname={getUserNickname}
                getUserVoteForPost={getUserVoteForPost}
                onSubmitReply={handleCreatePost}
              />
            ))}
          </div>
        )}
      </main>
    </div>
  )
}