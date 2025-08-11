export interface Post {
    id: string;
    userId: string;
    parentId: string | null; //null for main posts
    depth: number;
    title: string | null; //null for comments
    content: string;
    voteScore: number;
    commentCount: number;
    isPinned: boolean;
    isEdited: boolean;
    isDeleted: boolean;
}