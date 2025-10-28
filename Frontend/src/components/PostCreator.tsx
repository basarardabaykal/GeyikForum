import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { X } from "lucide-react";

interface PostCreatorProps {
  isOpen: boolean,
  parentId: string;
  depth: number;
  onSubmit: (parentId: string, depth: number, title: string, content: string) => void;
  onClose?: () => void;
}

export default function PostCreator({ isOpen, parentId, depth, onSubmit, onClose }: PostCreatorProps) {
  const isMainPost = !parentId;
  const [title, setTitle] = useState<string>("");
  const [content, setContent] = useState<string>("");

  if (!isOpen) return null;

  const canSubmit = content.trim().length > 0 && (!isMainPost || title.trim().length > 0);

  return (
    <Card className="max-w-2xl mx-auto">
      <CardHeader className="pb-3">
        <div className="flex items-center justify-between">
          <CardTitle className="text-base">
            {isMainPost ? "Yeni bir gönderi oluştur" : "Yanıtla"}
          </CardTitle>
          {onClose && (
            <Button
              type="button"
              variant="ghost"
              size="sm"
              onClick={onClose}
              className="h-8 px-2 text-gray-600 hover:text-gray-900"
            >
              <X className="h-4 w-4" />
              <span className="sr-only">Kapat</span>
            </Button>
          )}
        </div>
      </CardHeader>

      <CardContent className="space-y-4">
        {isMainPost && (
          <div className="grid gap-2">
            <Label htmlFor="title">Başlık</Label>
            <Input
              id="title"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              placeholder="Gönderi başlığını girin..."
            />
          </div>
        )}

        <div className="grid gap-2">
          <Label htmlFor="content">İçerik</Label>
          <Textarea
            id="content"
            value={content}
            onChange={(e) => setContent(e.target.value)}
            placeholder="Aklınızda ne var?"
            rows={8}
            className="resize-vertical"
          />
        </div>
      </CardContent>

      <CardFooter className="flex justify-end">
        <Button
          type="button"
          disabled={!canSubmit}
          onClick={() => onSubmit(parentId, depth, title, content)}
        >
          Gönderi oluştur
        </Button>
      </CardFooter>
    </Card>
  );
}