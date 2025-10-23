import type { Post } from "../models/Post";
import VoteButtons from "./VoteButtons";
import { MessageCircle, Pin, Edit, Trash2, RotateCcw } from "lucide-react"
import PostCreator from "./PostCreator";
import { useState } from "react";


interface PostItemProps {
  post: Post;
  posts: Post[];
  onVote: (postId: string, newVote: number) => void;
  getUserNickname: (userId: string) => string;
  getUserVoteForPost: (postId: string) => number;
  onSubmitReply: (parentId: string, depth: number, title: string, content: string) => void;
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


  return (
    <div className={`${isMainPost ? 'border rounded-lg mb-4 bg-white' : ''}`}>
      <div
        className={`flex p-4 ${!isMainPost ? 'border-l-2 border-gray-200' : ''}`}
        style={{ marginLeft: isMainPost ? 0 : marginLeft }}
      >
        <VoteButtons
          score={post.voteScore}
          userVote={userVote}
          onVote={(newVote) => onVote(post.id, newVote)}
        />

        <div className="flex-1">
          <div className="flex items-center gap-2 mb-2">
            {post.isPinned && <Pin size={16} className="text-green-600" />}
            <span className="text-sm text-gray-600">
              u/{getUserNickname(post.userId)}
            </span>
            {post.isEdited && (
              <span className="flex items-center gap-1 text-xs text-gray-500">
                <Edit size={12} />
                edited
              </span>
            )}
          </div>

          {post.title && (
            <h3 className={`font-semibold mb-2 ${isMainPost ? 'text-lg text-gray-900' : 'text-base text-gray-800'
              }`}>
              {post.title}
            </h3>
          )}

          <p className="text-gray-700 mb-3 leading-relaxed">
            {post.isDeleted ? "[deleted]" : post.content}
          </p>

          <div className="flex items-center gap-4 text-sm text-gray-600">
            <button className="flex items-center gap-1 hover:text-gray-800"
              onClick={() => { setShowReplyCreator(!showReplyCreator) }}>
              <MessageCircle size={16} />Reply</button>
            {post.userId === 'currentUser' && (
              <button className="hover:text-gray-800">Edit</button>
            )}

            {/* Admin actions */}
            {isAdmin && (
              <div className="ml-auto flex items-center gap-3">
                <button
                  className={`flex items-center gap-1 hover:text-gray-800 ${post.isPinned ? 'text-blue-700' : ''} disabled:opacity-50`}
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
                  className={`flex items-center gap-1 hover:text-gray-800 ${post.isDeleted ? 'text-green-700' : 'text-red-700'}`}
                  onClick={() => onToggleDelete && onToggleDelete(post.id, post.isDeleted)}
                >
                  {post.isDeleted ? <RotateCcw size={16} /> : <Trash2 size={16} />}
                  {post.isDeleted ? "Restore" : "Delete"}
                </button>
              </div>
            )}
          </div>
        </div>
      </div>

      <PostCreator isOpen={showReplyCreator} parentId={post.id} depth={post.depth} onSubmit={onSubmitReply}></PostCreator>

      {replies.map(reply => (
        <PostItem
          key={reply.id}
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
      ))}
    </div>
  )
}