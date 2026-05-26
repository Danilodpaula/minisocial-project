using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSocial.Infrastructure.Data.Models;

[Table("Users")]
public class EfcUser
{
    [Column("Id")]
    public int Id { get; set; }
    
    [Column("Name")]
    public required string Name { get; set; }
    
    [Column("Username")]
    public required string Username { get; set; }
    
    [Column("Email")]
    public required string Email { get; set; }
    
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

}