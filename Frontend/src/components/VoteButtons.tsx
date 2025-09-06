import { useState } from "react";
import { ChevronUp, ChevronDown } from "lucide-react"


interface VoteButtonsProps {
  score: number;
  userVote: number;
  onVote: (newVote: number) => void;
}

export default function VoteButtons({ score, userVote, onVote }: VoteButtonsProps) {
  const [currentVote, setCurrentVote] = useState<number>(userVote)

  const handleUpvote = (): void => {
    onVote(1)
    if (currentVote === 1) {
      setCurrentVote(0)
    } else {
      setCurrentVote(1)
    }
  }

  const handleDownvote = (): void => {
    onVote(-1)
    if (currentVote === -1) {
      setCurrentVote(0)
    } else {
      setCurrentVote(-1)
    }
  }

  return (
    <div className="flex flex-col items-center mr-3">
      <button
        onClick={handleUpvote}
        className={`p-1 rounded hover:bg-gray-100 ${currentVote === 1 ? 'text-orange-500' : 'text-gray-500'
          }`}
      >
        <ChevronUp size={20} />
      </button>
      <span className={`font-medium text-sm ${currentVote === 1 ? 'text-orange-500' :
        currentVote === -1 ? 'text-purple-500' : 'text-gray-700'
        }`}>
        {score}
      </span>
      <button
        onClick={handleDownvote}
        className={`p-1 rounded hover:bg-gray-100 ${currentVote === -1 ? 'text-purple-500' : 'text-gray-500'
          }`}
      >
        <ChevronDown size={20} />
      </button>
    </div>
  )
}