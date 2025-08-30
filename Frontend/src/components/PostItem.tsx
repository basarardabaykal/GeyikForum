import type { Post } from "../models/Post";
import VoteButtons from "./VoteButtons";
import { MessageCircle, Pin, Edit } from "lucide-react"
import PostCreator from "./PostCreator";
import { useState } from "react";


interface PostItemProps {
  post: Post;
  posts: Post[];
  onVote: (postId: string, change: number) => void;
  getUserNickname: (userId: string) => string;
  //for submittin a reply to this post
  onSubmitReply: (parentId: string, depth: number, title: string, content: string) => void;
}

export default function PostItem({ post, posts, onVote, getUserNickname, onSubmitReply }: PostItemProps) {
  const isMainPost = post.parentId === null
  const replies = posts.filter(p => p.parentId === post.id).sort((a, b) => b.voteScore - a.voteScore)

  const marginLeft: number = post.depth * 24

  const [showReplyCreator, setShowReplyCreator] = useState<boolean>(false)

  return (
    <div className={`${isMainPost ? 'border rounded-lg mb-4 bg-white' : ''}`}>
      <div
        className={`flex p-4 ${!isMainPost ? 'border-l-2 border-gray-200' : ''}`}
        style={{ marginLeft: isMainPost ? 0 : marginLeft }}
      >
        <VoteButtons
          score={post.voteScore}
          onUpvote={(change) => onVote(post.id, change)}
          onDownvote={(change) => onVote(post.id, change)}
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
          onSubmitReply={onSubmitReply}
        />
      ))}
    </div>
  )
}