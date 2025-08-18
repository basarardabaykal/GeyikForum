export interface PostVote {
    Id: string;
    UserId: string;
    PostId: string;
    VoteValue: number;
    CreatedAt: Date;
    UpdatedAt: Date;
}