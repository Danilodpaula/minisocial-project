namespace MiniSocial.Domain.Entities;

public class Follow
{
    public int Id { get; set; }
    public int FollowerId { get; set; }
    public int FolloweeId { get; set; }
    public DateTime CreatedAt { get; set; }
}