# MiniSocial API — Answer Key and Implemented Code

This README contains a possible implementation for the **MiniSocial API** practical project.

The solution uses:

- ASP.NET Core Web API;
- PostgreSQL 15;
- Entity Framework Core;
- Npgsql provider for EF Core;
- ADO.NET with `Npgsql`;
- layered architecture with Controller, Service, Repository, DTOs and Entities.

---

## 1. Project Creation

```bash
dotnet new webapi -n MiniSocialApi
cd MiniSocialApi
```

Install packages:

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql
```

---

## 2. Docker Compose

Create a file named `docker-compose.yml`:

```yaml
services:
  minisocial-postgres:
    image: postgres:15
    container_name: minisocial-postgres
    restart: unless-stopped
    environment:
      POSTGRES_DB: minisocial_db
      POSTGRES_USER: minisocial_user
      POSTGRES_PASSWORD: minisocial_password
    ports:
      - "5432:5432"
    volumes:
      - minisocial_postgres_data:/var/lib/postgresql/data
    networks:
      - minisocial-network

volumes:
  minisocial_postgres_data:

networks:
  minisocial-network:
    driver: bridge
```

Start the database:

```bash
docker compose up -d
```

---

## 3. `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=minisocial_db;Username=minisocial_user;Password=minisocial_password"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## 4. Project Structure

```text
MiniSocialApi/
├── Controllers/
├── Application/
│   ├── DTOs/
│   └── Services/
├── Domain/
│   ├── Entities/
│   └── Enums/
├── Infrastructure/
│   ├── Data/
│   └── Repositories/
├── appsettings.json
├── docker-compose.yml
└── Program.cs
```

---

# 5. Domain Layer

## 5.1. `Domain/Enums/ReactionType.cs`

```csharp
namespace MiniSocialApi.Domain.Enums;

public enum ReactionType
{
    Like = 1,
    Love = 2,
    Funny = 3
}
```

---

## 5.2. `Domain/Entities/User.cs`

```csharp
namespace MiniSocialApi.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Post> Posts { get; set; } = new List<Post>();
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public List<Reaction> Reactions { get; set; } = new List<Reaction>();

    public List<Follow> Following { get; set; } = new List<Follow>();
    public List<Follow> Followers { get; set; } = new List<Follow>();
}
```

---

## 5.3. `Domain/Entities/Post.cs`

```csharp
namespace MiniSocialApi.Domain.Entities;

public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public List<Reaction> Reactions { get; set; } = new List<Reaction>();
}
```

---

## 5.4. `Domain/Entities/Comment.cs`

```csharp
namespace MiniSocialApi.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Post? Post { get; set; }
    public User? User { get; set; }
}
```

---

## 5.5. `Domain/Entities/Reaction.cs`

```csharp
using MiniSocialApi.Domain.Enums;

namespace MiniSocialApi.Domain.Entities;

public class Reaction
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public ReactionType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Post? Post { get; set; }
    public User? User { get; set; }
}
```

---

## 5.6. `Domain/Entities/Follow.cs`

```csharp
namespace MiniSocialApi.Domain.Entities;

public class Follow
{
    public int Id { get; set; }
    public int FollowerId { get; set; }
    public int FollowedId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? Follower { get; set; }
    public User? Followed { get; set; }
}
```

---

# 6. DTOs

## 6.1. `Application/DTOs/CreateUserRequest.cs`

```csharp
namespace MiniSocialApi.Application.DTOs;

public class CreateUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
```

---

## 6.2. `Application/DTOs/UserResponse.cs`

```csharp
namespace MiniSocialApi.Application.DTOs;

public class UserResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
```

---

## 6.3. `Application/DTOs/CreatePostRequest.cs`

```csharp
namespace MiniSocialApi.Application.DTOs;

public class CreatePostRequest
{
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
}
```

---

## 6.4. `Application/DTOs/PostResponse.cs`

```csharp
namespace MiniSocialApi.Application.DTOs;

public class PostResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
```

---

## 6.5. `Application/DTOs/CreateCommentRequest.cs`

```csharp
namespace MiniSocialApi.Application.DTOs;

public class CreateCommentRequest
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
}
```

---

## 6.6. `Application/DTOs/CommentResponse.cs`

```csharp
namespace MiniSocialApi.Application.DTOs;

public class CommentResponse
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
```

---

## 6.7. `Application/DTOs/CreateReactionRequest.cs`

```csharp
using MiniSocialApi.Domain.Enums;

namespace MiniSocialApi.Application.DTOs;

public class CreateReactionRequest
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public ReactionType Type { get; set; }
}
```

---

## 6.8. `Application/DTOs/ReactionResponse.cs`

```csharp
namespace MiniSocialApi.Application.DTOs;

public class ReactionResponse
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
```

---

## 6.9. `Application/DTOs/FollowUserRequest.cs`

```csharp
namespace MiniSocialApi.Application.DTOs;

public class FollowUserRequest
{
    public int FollowerId { get; set; }
    public int FollowedId { get; set; }
}
```

---

## 6.10. `Application/DTOs/FollowResponse.cs`

```csharp
namespace MiniSocialApi.Application.DTOs;

public class FollowResponse
{
    public int Id { get; set; }
    public int FollowerId { get; set; }
    public int FollowedId { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

---

## 6.11. `Application/DTOs/FeedItemResponse.cs`

```csharp
namespace MiniSocialApi.Application.DTOs;

public class FeedItemResponse
{
    public int PostId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int CommentsCount { get; set; }
    public int ReactionsCount { get; set; }
}
```

---

# 7. Infrastructure Layer

## 7.1. `Infrastructure/Data/AppDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using MiniSocialApi.Domain.Entities;

namespace MiniSocialApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Reaction> Reactions => Set<Reaction>();
    public DbSet<Follow> Follows => Set<Follow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(user => user.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<Post>()
            .HasOne(post => post.User)
            .WithMany(user => user.Posts)
            .HasForeignKey(post => post.UserId);

        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.Post)
            .WithMany(post => post.Comments)
            .HasForeignKey(comment => comment.PostId);

        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.User)
            .WithMany(user => user.Comments)
            .HasForeignKey(comment => comment.UserId);

        modelBuilder.Entity<Reaction>()
            .HasOne(reaction => reaction.Post)
            .WithMany(post => post.Reactions)
            .HasForeignKey(reaction => reaction.PostId);

        modelBuilder.Entity<Reaction>()
            .HasOne(reaction => reaction.User)
            .WithMany(user => user.Reactions)
            .HasForeignKey(reaction => reaction.UserId);

        modelBuilder.Entity<Reaction>()
            .HasIndex(reaction => new { reaction.PostId, reaction.UserId })
            .IsUnique();

        modelBuilder.Entity<Follow>()
            .HasOne(follow => follow.Follower)
            .WithMany(user => user.Following)
            .HasForeignKey(follow => follow.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Follow>()
            .HasOne(follow => follow.Followed)
            .WithMany(user => user.Followers)
            .HasForeignKey(follow => follow.FollowedId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Follow>()
            .HasIndex(follow => new { follow.FollowerId, follow.FollowedId })
            .IsUnique();
    }
}
```

---

## 7.2. `Infrastructure/Repositories/ISocialRepository.cs`

```csharp
using MiniSocialApi.Domain.Entities;

namespace MiniSocialApi.Infrastructure.Repositories;

public interface ISocialRepository
{
    Task<User> CreateUserAsync(User user);
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);

    Task<Post> CreatePostAsync(Post post);
    Task<List<Post>> GetPostsAsync();
    Task<Post?> GetPostByIdAsync(int id);
    Task<List<Post>> GetPostsByUserIdAsync(int userId);

    Task<Comment> CreateCommentAsync(Comment comment);
    Task<List<Comment>> GetCommentsByPostIdAsync(int postId);

    Task<Reaction> CreateReactionAsync(Reaction reaction);
    Task<bool> ReactionExistsAsync(int postId, int userId);
    Task<List<Reaction>> GetReactionsByPostIdAsync(int postId);

    Task<Follow> CreateFollowAsync(Follow follow);
    Task<bool> FollowExistsAsync(int followerId, int followedId);
    Task<List<User>> GetFollowingAsync(int userId);
}
```

---

## 7.3. `Infrastructure/Repositories/SocialRepository.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using MiniSocialApi.Domain.Entities;
using MiniSocialApi.Infrastructure.Data;

namespace MiniSocialApi.Infrastructure.Repositories;

public class SocialRepository : ISocialRepository
{
    private readonly AppDbContext _context;

    public SocialRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.Users
            .OrderBy(user => user.Name)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<Post> CreatePostAsync(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return await _context.Posts
            .Include(item => item.User)
            .FirstAsync(item => item.Id == post.Id);
    }

    public async Task<List<Post>> GetPostsAsync()
    {
        return await _context.Posts
            .Include(post => post.User)
            .OrderByDescending(post => post.CreatedAt)
            .ToListAsync();
    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await _context.Posts
            .Include(post => post.User)
            .FirstOrDefaultAsync(post => post.Id == id);
    }

    public async Task<List<Post>> GetPostsByUserIdAsync(int userId)
    {
        return await _context.Posts
            .Include(post => post.User)
            .Where(post => post.UserId == userId)
            .OrderByDescending(post => post.CreatedAt)
            .ToListAsync();
    }

    public async Task<Comment> CreateCommentAsync(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return await _context.Comments
            .Include(item => item.User)
            .FirstAsync(item => item.Id == comment.Id);
    }

    public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        return await _context.Comments
            .Include(comment => comment.User)
            .Where(comment => comment.PostId == postId)
            .OrderBy(comment => comment.CreatedAt)
            .ToListAsync();
    }

    public async Task<Reaction> CreateReactionAsync(Reaction reaction)
    {
        _context.Reactions.Add(reaction);
        await _context.SaveChangesAsync();

        return await _context.Reactions
            .Include(item => item.User)
            .FirstAsync(item => item.Id == reaction.Id);
    }

    public async Task<bool> ReactionExistsAsync(int postId, int userId)
    {
        return await _context.Reactions
            .AnyAsync(reaction => reaction.PostId == postId && reaction.UserId == userId);
    }

    public async Task<List<Reaction>> GetReactionsByPostIdAsync(int postId)
    {
        return await _context.Reactions
            .Include(reaction => reaction.User)
            .Where(reaction => reaction.PostId == postId)
            .OrderBy(reaction => reaction.CreatedAt)
            .ToListAsync();
    }

    public async Task<Follow> CreateFollowAsync(Follow follow)
    {
        _context.Follows.Add(follow);
        await _context.SaveChangesAsync();
        return follow;
    }

    public async Task<bool> FollowExistsAsync(int followerId, int followedId)
    {
        return await _context.Follows
            .AnyAsync(follow => follow.FollowerId == followerId && follow.FollowedId == followedId);
    }

    public async Task<List<User>> GetFollowingAsync(int userId)
    {
        return await _context.Follows
            .Where(follow => follow.FollowerId == userId)
            .Include(follow => follow.Followed)
            .Select(follow => follow.Followed!)
            .OrderBy(user => user.Name)
            .ToListAsync();
    }
}
```

---

## 7.4. `Infrastructure/Repositories/FeedAdoRepository.cs`

```csharp
using MiniSocialApi.Application.DTOs;
using Npgsql;

namespace MiniSocialApi.Infrastructure.Repositories;

public class FeedAdoRepository
{
    private readonly string _connectionString;

    public FeedAdoRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found.");
    }

    public async Task<List<FeedItemResponse>> GetFeedByUserIdAsync(int userId)
    {
        const string sql = @"
            SELECT 
                p.""Id"",
                u.""Username"",
                p.""Content"",
                p.""CreatedAt"",
                (
                    SELECT COUNT(*) 
                    FROM ""Comments"" c 
                    WHERE c.""PostId"" = p.""Id""
                ) AS CommentsCount,
                (
                    SELECT COUNT(*) 
                    FROM ""Reactions"" r 
                    WHERE r.""PostId"" = p.""Id""
                ) AS ReactionsCount
            FROM ""Posts"" p
            INNER JOIN ""Users"" u ON u.""Id"" = p.""UserId""
            INNER JOIN ""Follows"" f ON f.""FollowedId"" = p.""UserId""
            WHERE f.""FollowerId"" = @userId
            ORDER BY p.""CreatedAt"" DESC;
        ";

        List<FeedItemResponse> feed = new List<FeedItemResponse>();

        await using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@userId", userId);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            FeedItemResponse item = new FeedItemResponse
            {
                PostId = reader.GetInt32(0),
                Username = reader.GetString(1),
                Content = reader.GetString(2),
                CreatedAt = reader.GetDateTime(3),
                CommentsCount = reader.GetInt32(4),
                ReactionsCount = reader.GetInt32(5)
            };

            feed.Add(item);
        }

        return feed;
    }
}
```

---

# 8. Application Service

## `Application/Services/SocialService.cs`

```csharp
using MiniSocialApi.Application.DTOs;
using MiniSocialApi.Domain.Entities;
using MiniSocialApi.Infrastructure.Repositories;

namespace MiniSocialApi.Application.Services;

public class SocialService
{
    private readonly ISocialRepository _repository;

    public SocialService(ISocialRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new InvalidOperationException("Name is required.");

        if (string.IsNullOrWhiteSpace(request.Username))
            throw new InvalidOperationException("Username is required.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new InvalidOperationException("Email is required.");

        if (await _repository.GetUserByUsernameAsync(request.Username) is not null)
            throw new InvalidOperationException("Username already exists.");

        if (await _repository.GetUserByEmailAsync(request.Email) is not null)
            throw new InvalidOperationException("Email already exists.");

        User user = new User
        {
            Name = request.Name,
            Username = request.Username,
            Email = request.Email
        };

        User createdUser = await _repository.CreateUserAsync(user);
        return MapUser(createdUser);
    }

    public async Task<List<UserResponse>> GetUsersAsync()
    {
        List<User> users = await _repository.GetUsersAsync();
        return users.Select(MapUser).ToList();
    }

    public async Task<UserResponse?> GetUserByIdAsync(int id)
    {
        User? user = await _repository.GetUserByIdAsync(id);
        return user is null ? null : MapUser(user);
    }

    public async Task<PostResponse> CreatePostAsync(CreatePostRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
            throw new InvalidOperationException("Content is required.");

        User? user = await _repository.GetUserByIdAsync(request.UserId);

        if (user is null)
            throw new InvalidOperationException("User not found.");

        Post post = new Post
        {
            UserId = request.UserId,
            Content = request.Content
        };

        Post createdPost = await _repository.CreatePostAsync(post);
        return MapPost(createdPost);
    }

    public async Task<List<PostResponse>> GetPostsAsync()
    {
        List<Post> posts = await _repository.GetPostsAsync();
        return posts.Select(MapPost).ToList();
    }

    public async Task<PostResponse?> GetPostByIdAsync(int id)
    {
        Post? post = await _repository.GetPostByIdAsync(id);
        return post is null ? null : MapPost(post);
    }

    public async Task<List<PostResponse>> GetPostsByUserIdAsync(int userId)
    {
        List<Post> posts = await _repository.GetPostsByUserIdAsync(userId);
        return posts.Select(MapPost).ToList();
    }

    public async Task<CommentResponse> CreateCommentAsync(CreateCommentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
            throw new InvalidOperationException("Content is required.");

        if (await _repository.GetUserByIdAsync(request.UserId) is null)
            throw new InvalidOperationException("User not found.");

        if (await _repository.GetPostByIdAsync(request.PostId) is null)
            throw new InvalidOperationException("Post not found.");

        Comment comment = new Comment
        {
            PostId = request.PostId,
            UserId = request.UserId,
            Content = request.Content
        };

        Comment createdComment = await _repository.CreateCommentAsync(comment);
        return MapComment(createdComment);
    }

    public async Task<List<CommentResponse>> GetCommentsByPostIdAsync(int postId)
    {
        List<Comment> comments = await _repository.GetCommentsByPostIdAsync(postId);
        return comments.Select(MapComment).ToList();
    }

    public async Task<ReactionResponse> CreateReactionAsync(CreateReactionRequest request)
    {
        if (await _repository.GetUserByIdAsync(request.UserId) is null)
            throw new InvalidOperationException("User not found.");

        if (await _repository.GetPostByIdAsync(request.PostId) is null)
            throw new InvalidOperationException("Post not found.");

        if (await _repository.ReactionExistsAsync(request.PostId, request.UserId))
            throw new InvalidOperationException("User already reacted to this post.");

        Reaction reaction = new Reaction
        {
            PostId = request.PostId,
            UserId = request.UserId,
            Type = request.Type
        };

        Reaction createdReaction = await _repository.CreateReactionAsync(reaction);
        return MapReaction(createdReaction);
    }

    public async Task<List<ReactionResponse>> GetReactionsByPostIdAsync(int postId)
    {
        List<Reaction> reactions = await _repository.GetReactionsByPostIdAsync(postId);
        return reactions.Select(MapReaction).ToList();
    }

    public async Task<FollowResponse> FollowUserAsync(FollowUserRequest request)
    {
        if (request.FollowerId == request.FollowedId)
            throw new InvalidOperationException("A user cannot follow himself.");

        if (await _repository.GetUserByIdAsync(request.FollowerId) is null)
            throw new InvalidOperationException("Follower user not found.");

        if (await _repository.GetUserByIdAsync(request.FollowedId) is null)
            throw new InvalidOperationException("Followed user not found.");

        if (await _repository.FollowExistsAsync(request.FollowerId, request.FollowedId))
            throw new InvalidOperationException("User already follows this user.");

        Follow follow = new Follow
        {
            FollowerId = request.FollowerId,
            FollowedId = request.FollowedId
        };

        Follow createdFollow = await _repository.CreateFollowAsync(follow);

        return new FollowResponse
        {
            Id = createdFollow.Id,
            FollowerId = createdFollow.FollowerId,
            FollowedId = createdFollow.FollowedId,
            CreatedAt = createdFollow.CreatedAt
        };
    }

    public async Task<List<UserResponse>> GetFollowingAsync(int userId)
    {
        List<User> following = await _repository.GetFollowingAsync(userId);
        return following.Select(MapUser).ToList();
    }

    private static UserResponse MapUser(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }

    private static PostResponse MapPost(Post post)
    {
        return new PostResponse
        {
            Id = post.Id,
            UserId = post.UserId,
            Username = post.User?.Username ?? string.Empty,
            Content = post.Content,
            CreatedAt = post.CreatedAt
        };
    }

    private static CommentResponse MapComment(Comment comment)
    {
        return new CommentResponse
        {
            Id = comment.Id,
            PostId = comment.PostId,
            UserId = comment.UserId,
            Username = comment.User?.Username ?? string.Empty,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt
        };
    }

    private static ReactionResponse MapReaction(Reaction reaction)
    {
        return new ReactionResponse
        {
            Id = reaction.Id,
            PostId = reaction.PostId,
            UserId = reaction.UserId,
            Username = reaction.User?.Username ?? string.Empty,
            Type = reaction.Type.ToString(),
            CreatedAt = reaction.CreatedAt
        };
    }
}
```

---

# 9. Controllers

## 9.1. `Controllers/UsersController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using MiniSocialApi.Application.DTOs;
using MiniSocialApi.Application.Services;

namespace MiniSocialApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly SocialService _service;

    public UsersController(SocialService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        try
        {
            UserResponse response = await _service.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetUsersAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        UserResponse? response = await _service.GetUserByIdAsync(id);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    [HttpGet("{userId:int}/following")]
    public async Task<IActionResult> GetFollowing(int userId)
    {
        return Ok(await _service.GetFollowingAsync(userId));
    }
}
```

---

## 9.2. `Controllers/PostsController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using MiniSocialApi.Application.DTOs;
using MiniSocialApi.Application.Services;

namespace MiniSocialApi.Controllers;

[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    private readonly SocialService _service;

    public PostsController(SocialService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePostRequest request)
    {
        try
        {
            PostResponse response = await _service.CreatePostAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetPostsAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        PostResponse? response = await _service.GetPostByIdAsync(id);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        return Ok(await _service.GetPostsByUserIdAsync(userId));
    }

    [HttpGet("{postId:int}/comments")]
    public async Task<IActionResult> GetComments(int postId)
    {
        return Ok(await _service.GetCommentsByPostIdAsync(postId));
    }

    [HttpGet("{postId:int}/reactions")]
    public async Task<IActionResult> GetReactions(int postId)
    {
        return Ok(await _service.GetReactionsByPostIdAsync(postId));
    }
}
```

---

## 9.3. `Controllers/CommentsController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using MiniSocialApi.Application.DTOs;
using MiniSocialApi.Application.Services;

namespace MiniSocialApi.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentsController : ControllerBase
{
    private readonly SocialService _service;

    public CommentsController(SocialService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCommentRequest request)
    {
        try
        {
            CommentResponse response = await _service.CreateCommentAsync(request);
            return Ok(response);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
```

---

## 9.4. `Controllers/ReactionsController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using MiniSocialApi.Application.DTOs;
using MiniSocialApi.Application.Services;

namespace MiniSocialApi.Controllers;

[ApiController]
[Route("api/reactions")]
public class ReactionsController : ControllerBase
{
    private readonly SocialService _service;

    public ReactionsController(SocialService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateReactionRequest request)
    {
        try
        {
            ReactionResponse response = await _service.CreateReactionAsync(request);
            return Ok(response);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
```

---

## 9.5. `Controllers/FollowsController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using MiniSocialApi.Application.DTOs;
using MiniSocialApi.Application.Services;

namespace MiniSocialApi.Controllers;

[ApiController]
[Route("api/follows")]
public class FollowsController : ControllerBase
{
    private readonly SocialService _service;

    public FollowsController(SocialService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Follow(FollowUserRequest request)
    {
        try
        {
            FollowResponse response = await _service.FollowUserAsync(request);
            return Ok(response);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
```

---

## 9.6. `Controllers/FeedController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using MiniSocialApi.Infrastructure.Repositories;

namespace MiniSocialApi.Controllers;

[ApiController]
[Route("api/feed")]
public class FeedController : ControllerBase
{
    private readonly FeedAdoRepository _feedRepository;

    public FeedController(FeedAdoRepository feedRepository)
    {
        _feedRepository = feedRepository;
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetFeed(int userId)
    {
        var feed = await _feedRepository.GetFeedByUserIdAsync(userId);
        return Ok(feed);
    }
}
```

---

# 10. `Program.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using MiniSocialApi.Application.Services;
using MiniSocialApi.Infrastructure.Data;
using MiniSocialApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<ISocialRepository, SocialRepository>();
builder.Services.AddScoped<SocialService>();
builder.Services.AddScoped<FeedAdoRepository>();

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
```

---

# 11. Running the Project

Start PostgreSQL:

```bash
docker compose up -d
```

Run the API:

```bash
dotnet run
```

Open Swagger:

```text
https://localhost:PORT/swagger
```

or:

```text
http://localhost:PORT/swagger
```

The port will be shown in the terminal output.

---

# 12. Test Sequence

## 12.1. Create Maria

```http
POST /api/users
```

```json
{
  "name": "Maria Silva",
  "username": "maria",
  "email": "maria@email.com"
}
```

---

## 12.2. Create João

```http
POST /api/users
```

```json
{
  "name": "João Souza",
  "username": "joao",
  "email": "joao@email.com"
}
```

---

## 12.3. João creates a post

```http
POST /api/posts
```

```json
{
  "userId": 2,
  "content": "Hello from João!"
}
```

---

## 12.4. Maria follows João

```http
POST /api/follows
```

```json
{
  "followerId": 1,
  "followedId": 2
}
```

---

## 12.5. Maria opens her feed

```http
GET /api/feed/1
```

Expected output:

```json
[
  {
    "postId": 1,
    "username": "joao",
    "content": "Hello from João!",
    "createdAt": "2026-05-20T10:30:00Z",
    "commentsCount": 0,
    "reactionsCount": 0
  }
]
```

---

# 13. Where EF Core Is Used

EF Core is used in:

```text
AppDbContext
SocialRepository
Program.cs
```

Main responsibilities:

- map C# entities to database tables;
- save new records;
- list users, posts, comments and reactions;
- validate existence of users and posts;
- execute LINQ queries.

Example:

```csharp
return await _context.Posts
    .Include(post => post.User)
    .OrderByDescending(post => post.CreatedAt)
    .ToListAsync();
```

---

# 14. Where ADO.NET Is Used

ADO.NET is used in:

```text
FeedAdoRepository
```

The feed endpoint uses:

```text
NpgsqlConnection
NpgsqlCommand
NpgsqlDataReader
SQL parameter @userId
```

Example:

```csharp
await using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
await connection.OpenAsync();

await using NpgsqlCommand command = new NpgsqlCommand(sql, connection);
command.Parameters.AddWithValue("@userId", userId);

await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
```

---

# 15. Why DTOs Are Used

DTOs are used to avoid exposing domain entities directly through the API.

They define:

- what the API receives;
- what the API returns;
- which fields are visible to the client;
- the input and output contract of each endpoint.

---

# 16. Why Services and Repositories Are Separated

The controller only handles HTTP.

The service handles business rules.

The repository handles database access.

This avoids putting all logic inside the controller.

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
Database
```

This structure makes the project easier to understand, test and maintain.
