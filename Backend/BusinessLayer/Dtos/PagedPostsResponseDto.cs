using System.Collections.Generic;
using CoreLayer.Entities;

namespace BusinessLayer.Dtos;

public class PagedPostsResponseDto
{
    public List<Post> Posts { get; set; } = new();
    public int ReturnedParentCount { get; set; }
    public int TotalParentCount { get; set; }
    public bool HasMore { get; set; }
}