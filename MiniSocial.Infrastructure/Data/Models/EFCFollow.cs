using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSocial.Infrastructure.Data.Models;

public class EfcFollow
{
    [Column("Id")]
    public int Id { get; set; }
    
    [Column("FollowerId")]
    public int FollowerId { get; set; }
    
    [Column("FolloweeId")]
    public int FolloweeId { get; set; }
    
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }
}