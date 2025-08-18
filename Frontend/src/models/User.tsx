export interface User {
    id: string;
    email: string;
    nickname: string;
    karma: number;
    isAdmin: boolean;
    isModerator: boolean;
    isBanned: boolean;
}