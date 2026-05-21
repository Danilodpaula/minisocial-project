# MiniSocial API — Instructions

## 1. Objective

Build a small **ASP.NET Core Web API** for a simplified social network.

The project must demonstrate:

- API architecture using layers;
- DTOs for input and output models;
- services for business rules;
- repositories for data access;
- Entity Framework Core with PostgreSQL;
- one ADO.NET query using `Npgsql`;
- basic relationships between entities.

The project should follow this flow:

```text
Client / Swagger / Postman
        ↓
Controller
        ↓
Service
        ↓
Repository
        ↓
EF Core or ADO.NET
        ↓
PostgreSQL Database
```

---

## 2. Project Theme

The API will be called:

```text
MiniSocialApi
```

It will represent a small social network where users can:

- create accounts;
- publish posts;
- comment on posts;
- react to posts;
- follow other users;
- view a feed with posts from followed users.

---

## 3. Maximum Number of Entities

The project must use **at most 5 domain entities**:

| Entity | Description |
|---|---|
| `User` | Represents a social network user. |
| `Post` | Represents a post created by a user. |
| `Comment` | Represents a comment made by a user on a post. |
| `Reaction` | Represents a reaction made by a user on a post. |
| `Follow` | Represents the relationship of one user following another user. |

There is also one enum:

| Enum | Description |
|---|---|
| `ReactionType` | Defines possible reaction types: `Like`, `Love`, `Funny`. |

---

## 4. Required Technologies

Use:

```text
ASP.NET Core Web API
Entity Framework Core
PostgreSQL 15
Npgsql.EntityFrameworkCore.PostgreSQL
Npgsql
ADO.NET
Swagger
Docker Compose
```

Required packages:

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql
```

---

## 5. Database with Docker Compose

Create a `docker-compose.yml` file to start PostgreSQL 15.

Suggested values:

| Item | Value |
|---|---|
| Database | `minisocial_db` |
| User | `minisocial_user` |
| Password | `minisocial_password` |
| Port | `5432` |

Expected connection string:

```text
Host=localhost;Port=5432;Database=minisocial_db;Username=minisocial_user;Password=minisocial_password
```

---

## 6. Suggested Project Structure

Create the following structure:

```text
MiniSocialApi/
├── Controllers/
│   ├── UsersController.cs
│   ├── PostsController.cs
│   ├── CommentsController.cs
│   ├── ReactionsController.cs
│   ├── FollowsController.cs
│   └── FeedController.cs
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
└── Program.cs
```

---

## 7. API Endpoints

### 7.1. Users

#### Create User

```http
POST /api/users
```

Description:

Creates a new user account.

Input model:

```json
{
  "name": "Maria Silva",
  "username": "maria",
  "email": "maria@email.com"
}
```

Expected output:

```json
{
  "id": 1,
  "name": "Maria Silva",
  "username": "maria",
  "email": "maria@email.com",
  "createdAt": "2026-05-20T10:00:00"
}
```

---

#### List Users

```http
GET /api/users
```

Description:

Returns all users.

---

#### Get User by ID

```http
GET /api/users/{id}
```

Description:

Returns a user by ID.

---

### 7.2. Posts

#### Create Post

```http
POST /api/posts
```

Description:

Creates a post for an existing user.

Input model:

```json
{
  "userId": 1,
  "content": "My first post in MiniSocial!"
}
```

Expected output:

```json
{
  "id": 1,
  "userId": 1,
  "username": "maria",
  "content": "My first post in MiniSocial!",
  "createdAt": "2026-05-20T10:10:00"
}
```

---

#### List Posts

```http
GET /api/posts
```

Description:

Returns all posts ordered by creation date.

---

#### Get Post by ID

```http
GET /api/posts/{id}
```

Description:

Returns a post by ID.

---

#### List Posts by User

```http
GET /api/posts/user/{userId}
```

Description:

Returns all posts created by a specific user.

---

### 7.3. Comments

#### Create Comment

```http
POST /api/comments
```

Description:

Creates a comment in an existing post.

Input model:

```json
{
  "postId": 1,
  "userId": 2,
  "content": "Great post!"
}
```

Expected output:

```json
{
  "id": 1,
  "postId": 1,
  "userId": 2,
  "username": "joao",
  "content": "Great post!",
  "createdAt": "2026-05-20T10:15:00"
}
```

---

#### List Comments by Post

```http
GET /api/posts/{postId}/comments
```

Description:

Returns all comments of a specific post.

---

### 7.4. Reactions

#### Create Reaction

```http
POST /api/reactions
```

Description:

Adds a reaction to a post.

Input model:

```json
{
  "postId": 1,
  "userId": 2,
  "type": 1
}
```

Reaction type values:

| Value | Reaction |
|---:|---|
| `1` | `Like` |
| `2` | `Love` |
| `3` | `Funny` |

Expected output:

```json
{
  "id": 1,
  "postId": 1,
  "userId": 2,
  "username": "joao",
  "type": "Like",
  "createdAt": "2026-05-20T10:20:00"
}
```

---

#### List Reactions by Post

```http
GET /api/posts/{postId}/reactions
```

Description:

Returns all reactions of a specific post.

---

### 7.5. Follows

#### Follow User

```http
POST /api/follows
```

Description:

Makes one user follow another user.

Input model:

```json
{
  "followerId": 1,
  "followedId": 2
}
```

Expected output:

```json
{
  "id": 1,
  "followerId": 1,
  "followedId": 2,
  "createdAt": "2026-05-20T10:25:00"
}
```

Business rules:

- A user cannot follow himself.
- A user cannot follow the same user more than once.
- Both users must exist.

---

#### List Following

```http
GET /api/users/{userId}/following
```

Description:

Returns the users followed by a specific user.

---

### 7.6. Feed

#### Get Feed using ADO.NET

```http
GET /api/feed/{userId}
```

Description:

Returns posts from users followed by the informed user.

This endpoint must use **ADO.NET with Npgsql** and a manual SQL query with parameters.

Expected output:

```json
[
  {
    "postId": 2,
    "username": "joao",
    "content": "Hello from João!",
    "createdAt": "2026-05-20T10:30:00",
    "commentsCount": 1,
    "reactionsCount": 2
  }
]
```

---

## 8. Required Input Models

Create the following request DTOs:

```text
CreateUserRequest
CreatePostRequest
CreateCommentRequest
CreateReactionRequest
FollowUserRequest
```

---

## 9. Required Output Models

Create the following response DTOs:

```text
UserResponse
PostResponse
CommentResponse
ReactionResponse
FollowResponse
FeedItemResponse
```

---

## 10. Business Rules

### Users

- `Name`, `Username` and `Email` cannot be empty.
- `Username` must be unique.
- `Email` must be unique.

### Posts

- A post must belong to an existing user.
- `Content` cannot be empty.

### Comments

- A comment must belong to an existing post.
- A comment must belong to an existing user.
- `Content` cannot be empty.

### Reactions

- A reaction must belong to an existing post.
- A reaction must belong to an existing user.
- A user cannot react more than once to the same post.

### Follows

- The follower user must exist.
- The followed user must exist.
- A user cannot follow himself.
- A user cannot follow the same user more than once.

---

## 11. What Must Be Implemented with EF Core

Use EF Core for:

- creating users;
- listing users;
- creating posts;
- listing posts;
- creating comments;
- creating reactions;
- creating follows;
- validating relationships;
- saving data to PostgreSQL.

---

## 12. What Must Be Implemented with ADO.NET

Use ADO.NET for:

```http
GET /api/feed/{userId}
```

This endpoint must:

- open a PostgreSQL connection using `NpgsqlConnection`;
- create a SQL command using `NpgsqlCommand`;
- use the parameter `@userId`;
- read the result using `NpgsqlDataReader`;
- return a list of `FeedItemResponse`.

---

## 13. Suggested Implementation Order

1. Create the API project.
2. Add EF Core and Npgsql packages.
3. Create `docker-compose.yml`.
4. Configure `appsettings.json`.
5. Create entities and enum.
6. Create `AppDbContext`.
7. Register EF Core in `Program.cs`.
8. Create DTOs.
9. Create repository.
10. Create service.
11. Create controllers.
12. Test user creation.
13. Test post creation.
14. Test follow creation.
15. Implement and test the feed using ADO.NET.

---

## 14. Suggested Test Flow

### 1. Create Maria

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

### 2. Create João

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

### 3. João creates a post

```http
POST /api/posts
```

```json
{
  "userId": 2,
  "content": "Hello from João!"
}
```

### 4. Maria follows João

```http
POST /api/follows
```

```json
{
  "followerId": 1,
  "followedId": 2
}
```

### 5. Maria opens her feed

```http
GET /api/feed/1
```

Expected result:

```json
[
  {
    "postId": 1,
    "username": "joao",
    "content": "Hello from João!",
    "createdAt": "2026-05-20T10:30:00",
    "commentsCount": 0,
    "reactionsCount": 0
  }
]
```

---

## 15. Deliverables

The student must submit:

- project source code;
- screenshot of Swagger with endpoints;
- screenshot of at least one created user;
- screenshot of at least one created post;
- screenshot of the feed endpoint;
- short explanation answering:
  - where EF Core is used;
  - where ADO.NET is used;
  - why DTOs were used;
  - why services and repositories were separated.
