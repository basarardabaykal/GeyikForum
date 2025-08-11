import { useState } from "react";
import { ChevronUp, ChevronDown } from "lucide-react"


interface VoteButtonsProps {
  score: number;
  onUpvote: (change: number) => void;
  onDownvote: (change: number) => void;
}

export default function VoteButtons({ score, onUpvote, onDownvote }: VoteButtonsProps) {
  const [userVote, setUserVote] = useState<'up' | 'down' | null>(null)

  const handleUpvote = (): void => {
    if (userVote === 'up') {
      setUserVote(null)
      onUpvote(-1)
    } else {
      const change = userVote === 'down' ? 2 : 1
      setUserVote('up')
      onUpvote(change)
    }
  }

  const handleDownvote = (): void => {
    if (userVote === 'down') {
      setUserVote(null)
      onDownvote(1)
    } else {
      const change = userVote === 'up' ? -2 : -1
      setUserVote('down')
      onDownvote(change)
    }
  }


  return (
    <div className="flex flex-col items-center mr-3">
      <button
        onClick={handleUpvote}
        className={`p-1 rounded hover:bg-gray-100 ${userVote === 'up' ? 'text-orange-500' : 'text-gray-500'
          }`}
      >
        <ChevronUp size={20} />
      </button>
      <span className={`font-medium text-sm ${userVote === 'up' ? 'text-orange-500' :
        userVote === 'down' ? 'text-purple-500' : 'text-gray-700'
        }`}>
        {score}
      </span>
      <button
        onClick={handleDownvote}
        className={`p-1 rounded hover:bg-gray-100 ${userVote === 'down' ? 'text-purple-500' : 'text-gray-500'
          }`}
      >
        <ChevronDown size={20} />
      </button>
    </div>
  )
}