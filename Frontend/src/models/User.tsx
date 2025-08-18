export interface User {
    Id: string;
    Email: string;
    Nickname: string;
    Karma: number;
    IsAdmin: boolean;
    IsModerator: boolean;
    IsBanned: boolean;
}