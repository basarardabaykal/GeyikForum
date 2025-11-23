import type { Post } from "../models/Post";
import VoteButtons from "./VoteButtons";
import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import { cn } from "@/lib/utils"
import { MessageCircle, Pin, Edit, Trash2, RotateCcw } from "lucide-react"
import PostCreator from "./PostCreator";
import { useState } from "react";


interface PostItemProps {
  post: Post;
  posts: Post[];
  onVote: (postId: string, newVote: number) => void;
  getUserNickname: (userId: string) => string;
  getUserVoteForPost: (postId: string) => number;
  onSubmitReply: (parentId: string, depth: number, title: string, content: string) => Promise<void>;
  isAdmin?: boolean;
  onTogglePin?: (postId: string, current: boolean) => void;
  onToggleDelete?: (postId: string, current: boolean) => void;
}

export default function PostItem({ post, posts, onVote, getUserNickname, getUserVoteForPost, onSubmitReply, isAdmin, onTogglePin, onToggleDelete }: PostItemProps) {
  const isMainPost = post.parentId === null
  const replies = posts.filter(p => p.parentId === post.id).sort((a, b) => b.voteScore - a.voteScore)

  const marginLeft: number = post.depth * 24
  const userVote = getUserVoteForPost(post.id)

  const [showReplyCreator, setShowReplyCreator] = useState<boolean>(false)

  const createdAtText = post.createdAt ? new Date(post.createdAt).toLocaleString() : null

  const HeaderMeta = (
    <div className="flex items-center gap-2 text-sm text-muted-foreground">
      {post.isPinned && <Pin size={16} className="text-green-600" />}
      <span>{getUserNickname(post.userId)}</span>
      {createdAtText && <span>• {createdAtText}</span>}
      {post.isEdited && (
        <span className="flex items-center gap-1 text-xs text-muted-foreground">
          <Edit size={12} />
          değiştirilmiş
        </span>
      )}
    </div>
  )

  const Actions = (
    <div className="flex items-center gap-4 text-sm text-muted-foreground w-full">
      <button
        className="flex items-center gap-1 hover:text-foreground"
        onClick={() => { setShowReplyCreator(!showReplyCreator) }}
      >
        <MessageCircle size={16} />Yanıtla
      </button>

      {/* Admin actions */}
      {isAdmin && (
        <div className="ml-auto flex items-center gap-3">
          <button
            className={cn(
              "flex items-center gap-1 hover:text-foreground disabled:opacity-50",
              post.isPinned && "text-blue-700"
            )}
            onClick={() => onTogglePin && onTogglePin(post.id, post.isPinned)}
            disabled={!isMainPost || post.isDeleted}
            title={
              !isMainPost
                ? "Yorumlar sabitlenemez"
                : post.isDeleted
                  ? "Silinmiş gönderi sabitlenemez"
                  : (post.isPinned ? "Unpin" : "Pin")
            }
          >
            <Pin size={16} /> {post.isPinned ? "Unpin" : "Pin"}
          </button>

          <button
            className={cn(
              "flex items-center gap-1 hover:text-foreground",
              post.isDeleted ? "text-green-700" : "text-red-700"
            )}
            onClick={() => onToggleDelete && onToggleDelete(post.id, post.isDeleted)}
          >
            {post.isDeleted ? <RotateCcw size={16} /> : <Trash2 size={16} />}
            {post.isDeleted ? "Restore" : "Delete"}
          </button>
        </div>
      )}
    </div>
  )

  if (isMainPost) {
    return (
      <Card className="mb-4">
        <CardHeader className="pb-2">
          {HeaderMeta}
          {post.title && (
            <CardTitle className="text-lg mt-4">{post.title}</CardTitle>
          )}
        </CardHeader>

        <CardContent className="flex gap-4">
          <VoteButtons
            score={post.voteScore}
            userVote={userVote}
            onVote={(newVote) => onVote(post.id, newVote)}
          />
          <div className="flex-1">
            <p className="mb-3 leading-relaxed">
              {post.isDeleted ? "[deleted]" : post.content}
            </p>
          </div>
        </CardContent>

        <CardFooter className="gap-4">
          {Actions}
        </CardFooter>

        <PostCreator isOpen={showReplyCreator} parentId={post.id} depth={post.depth} onSubmit={onSubmitReply} />
        {replies.map(reply => (
          <div
            key={reply.id}
          >
            <PostItem
              post={reply}
              posts={posts}
              onVote={onVote}
              getUserNickname={getUserNickname}
              getUserVoteForPost={getUserVoteForPost}
              onSubmitReply={onSubmitReply}
              isAdmin={isAdmin}
              onTogglePin={onTogglePin}
              onToggleDelete={onToggleDelete}
            />
          </div>
        ))}
      </Card>
    )
  }

  // Reply block (non-main post)
  return (
    <div
      className="rounded-md border bg-card text-card-foreground shadow-sm p-4 mt-2"
      style={{ marginLeft: `clamp(0px, ${marginLeft}px, 24px)` }}
    >
      <div className="flex gap-4">
        <VoteButtons
          score={post.voteScore}
          userVote={userVote}
          onVote={(newVote) => onVote(post.id, newVote)}
        />
        <div className="flex-1">
          <div className="mb-2">{HeaderMeta}</div>
          {post.title && (
            <h4 className="font-semibold text-base text-foreground mb-2 break-words">{post.title}</h4>
          )}
          <p className="text-foreground/90 mb-3 leading-relaxed break-words whitespace-pre-wrap">
            {post.isDeleted ? "[deleted]" : post.content}
          </p>
          {Actions}
        </div>
      </div>
    </div>
  )
}