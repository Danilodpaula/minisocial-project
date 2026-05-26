namespace MiniSocial.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    
    /*
    public List<Post> Posts { get; set; } = new List<Post>();
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public List<Reaction> Reactions { get; set; } = new List<Reaction>();
    
    public List<Follow> Following { get; set; } = new List<Follow>();
    public List<Follow> Followers { get; set; } = new List<Follow>();
    */
}