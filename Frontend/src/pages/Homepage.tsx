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
import { Card, CardContent } from "@/components/ui/card"


export default function Homepage() {
  const { isAuthenticated, user } = useAuth()
  const navigate = useNavigate()
  const [posts, setPosts] = useState<Post[]>([])
  const [users, setUsers] = useState<User[]>([])
  const [postVotes, setPostVotes] = useState<PostVote[]>([])
  const [loading, setLoading] = useState<boolean>(true)
  const [page, setPage] = useState<number>(1)
  const [hasMore, setHasMore] = useState<boolean>(true)
  const [loadingMore, setLoadingMore] = useState<boolean>(false)
  const [showMainCreator, setShowMainCreator] = useState<boolean>(false)

  const ZERO_GUID = "00000000-0000-0000-0000-000000000000";
  const isAdmin = !!(user?.isAdmin || (user?.roles || []).includes("Admin"));

  const fetchUsers = async () => {
    const response = await userService.getAll()
    if (response?.data.success) {
      const mappedUsers: User[] = response.data.data
      setUsers(mappedUsers)
    }
  }

  const fetchPostVotes = async (): Promise<void> => {
    const response = await postVoteService.getAll()
    if (response?.data.success) {
      const mappedPostVotes: PostVote[] = response.data.data
      setPostVotes(mappedPostVotes)
    }
  }

  const fetchPosts = async (): Promise<void> => {
    const response = await postService.getAll()
    if (response?.data.success) {
      const mappedPosts: Post[] = response.data.data
      setPosts(mappedPosts)
    }
  }

  const fetchPage = async (pageToFetch: number): Promise<void> => {
    const response = await postService.getPage(pageToFetch, 10)
    if (response?.data?.success) {
      const newPosts = response.data.data as Post[];
      setPosts(prev => {
        const seen = new Set(prev.map(p => p.id))
        const merged = [...prev]
        for (const p of newPosts) {
          if (!seen.has(p.id)) {
            merged.push(p)
            seen.add(p.id)
          }
        }
        return merged
      })
      setHasMore((newPosts.filter(p => p.parentId === null).length) === 10)
      setPage(pageToFetch)
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


    const newPostVote: PostVote = {
      id: ZERO_GUID,
      userId: user?.id || ZERO_GUID,
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
    const newPost: Post = {
      id: ZERO_GUID,
      userId: user?.id || ZERO_GUID,
      parentId: parentId || null,
      depth,
      title: title || null,
      content,
      voteScore: 0,
      commentCount: 0,
      isPinned: false,
      isEdited: false,
      isDeleted: false,
    };

    const response = await postService.createPost(newPost);

    if (response?.data?.success) {
      const created: Post = response.data.data || newPost;
      setPosts(prev => {
        const updated = [...prev, created];
        if (created.parentId) {
          return updated.map(p =>
            p.id === created.parentId
              ? { ...p, commentCount: p.commentCount + 1 }
              : p
          );
        }
        return updated;
      });
      if (!created.parentId) {
        setShowMainCreator(false);
      }
      toast.success("Gönderi oluşturuldu.");
    }
  };

  const handleTogglePin = async (postId: string, current: boolean) => {
    const response = await postService.setPinStatus(postId, !current, user?.id || ZERO_GUID);
    if (response?.data?.success) {
      const updated = response.data.data as Post;
      setPosts(prev => prev.map(p => p.id === postId ? { ...p, isPinned: updated.isPinned } : p));
    }
  };

  const handleToggleDelete = async (postId: string, current: boolean) => {
    const response = await postService.setDeleteStatus(postId, !current, user?.id || ZERO_GUID);
    if (response?.data?.success) {
      const updated = response.data.data as Post;
      setPosts(prev => prev.map(p => p.id === postId ? { ...p, isDeleted: updated.isDeleted } : p));
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        await Promise.all([fetchUsers(), fetchPostVotes(), fetchPage(1)])
      } catch (error) {
        toast.error("Veri aktarımında bir hatayla karşılaşıldı.")
      } finally {
        setLoading(false)
      }
    }

    fetchData()
  }, [])

  // Build parent list and float pinned to the top (no backend change)
  const mainPosts = posts.filter(p => p.parentId === null)
  const mainPostsSorted = [...mainPosts].sort((a, b) => {
    const pinDelta = Number(b.isPinned) - Number(a.isPinned)
    if (pinDelta !== 0) return pinDelta
    const aTime = a.createdAt ? Date.parse(a.createdAt) : 0
    const bTime = b.createdAt ? Date.parse(b.createdAt) : 0
    return bTime - aTime
  })

  const handleLoadMore = async (): Promise<void> => {
    if (!hasMore || loadingMore) return
    try {
      setLoadingMore(true)
      await fetchPage(page + 1)
    } finally {
      setLoadingMore(false)
    }
  }

  if (loading) {
    return (
      <div className="min-h-screen bg-background">
        <main className="max-w-4xl mx-auto px-4 py-6">
          <Card>
            <CardContent className="py-8 text-center text-muted-foreground">
              Yükleniyor...
            </CardContent>
          </Card>
        </main>
      </div>
    )
  }

  return (
    <div className="min-h-screen">
      <main className="max-w-4xl mx-auto px-4 py-6">
        <div className="mb-4 flex justify-end">
          {!showMainCreator && (
            <button
              onClick={() => setShowMainCreator(true)}
              className="px-4 py-2 rounded-md bg-blue-600 text-white hover:bg-blue-700"
            >
              Gönderi Oluştur
            </button>
          )}
        </div>

        <div className="mb-4">
          <PostCreator
            isOpen={showMainCreator}
            parentId={""}
            depth={0}
            onSubmit={handleCreatePost}
            onClose={() => setShowMainCreator(false)}
          />
        </div>

        {mainPostsSorted.length === 0 ? (
          <Card>
            <CardContent className="py-10 text-center text-muted-foreground">
              Henüz gönderi yok.
            </CardContent>
          </Card>
        ) : (
          <div className="space-y-4">
            {mainPostsSorted.map(post => (
              <PostItem
                key={post.id}
                post={post}
                posts={posts}
                onVote={handleVote}
                getUserNickname={getUserNickname}
                getUserVoteForPost={getUserVoteForPost}
                onSubmitReply={handleCreatePost}
                isAdmin={isAdmin}
                onTogglePin={handleTogglePin}
                onToggleDelete={handleToggleDelete}
              />
            ))}
            <div className="flex justify-center pt-2">
              {hasMore ? (
                <button
                  onClick={handleLoadMore}
                  disabled={loadingMore}
                  className="px-4 py-2 rounded-md bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-60"
                >
                  {loadingMore ? "Yükleniyor..." : "Daha fazla yükle"}
                </button>
              ) : (
                <div className="text-sm text-muted-foreground py-2">Hepsi yüklendi.</div>
              )}
            </div>
          </div>
        )}
      </main>
    </div>
  )
}