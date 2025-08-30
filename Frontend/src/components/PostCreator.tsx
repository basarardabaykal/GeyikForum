import { useEffect, useState } from "react";

interface PostCreatorProps {
  isOpen: boolean,
  parentId: string;
  depth: number;
  onSubmit: (parentId: string, depth: number, title: string, content: string) => void;
}

export default function PostCreator({ isOpen, parentId, depth, onSubmit }: PostCreatorProps) {
  const isMainPost = parentId === null || parentId === "";
  const [title, setTitle] = useState<string>("")
  const [content, setContent] = useState<string>("")

  if (!isOpen) {
    return
  }
  return (
    <div className="max-w-2xl mx-auto p-6 bg-white border border-gray-200 rounded-lg shadow-sm">


      <div className="space-y-4">
        <div>
          <label htmlFor="title" className="block text-sm font-medium text-gray-700 mb-2">
            Title
          </label>
          <input
            type="text"
            id="title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            placeholder="Enter your post title..."
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
        </div>

        <div>
          <label htmlFor="content" className="block text-sm font-medium text-gray-700 mb-2">
            Content
          </label>
          <textarea
            id="content"
            value={content}
            onChange={(e) => setContent(e.target.value)}
            placeholder="What's on your mind?"
            rows={8}
            className="h-12 w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent resize-vertical"
          />
        </div>

        <div className="flex items-center justify-between pt-4">
          <button
            type="button"
            disabled={!title.trim() || !content.trim()}
            onClick={() => onSubmit(parentId, depth, title, content)}
            className="px-6 py-2 bg-blue-600 text-white font-medium rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:bg-gray-300 disabled:cursor-not-allowed transition-colors"
          >
            Create Post
          </button>
        </div>
      </div>
    </div>
  )
}