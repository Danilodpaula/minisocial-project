CREATE TABLE IF NOT EXISTS users (
                                     id SERIAL PRIMARY KEY,
                                     name VARCHAR(120) NOT NULL,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(150) NOT NULL UNIQUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
    );

CREATE TABLE IF NOT EXISTS posts (
                                     id SERIAL PRIMARY KEY,
                                     user_id INTEGER NOT NULL,
                                     content TEXT NOT NULL,
                                     created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

                                     CONSTRAINT fk_posts_users
                                     FOREIGN KEY (user_id)
    REFERENCES users(id)
    ON DELETE CASCADE
    );

CREATE TABLE IF NOT EXISTS comments (
                                        id SERIAL PRIMARY KEY,
                                        post_id INTEGER NOT NULL,
                                        user_id INTEGER NOT NULL,
                                        content TEXT NOT NULL,
                                        created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

                                        CONSTRAINT fk_comments_posts
                                        FOREIGN KEY (post_id)
    REFERENCES posts(id)
    ON DELETE CASCADE,

    CONSTRAINT fk_comments_users
    FOREIGN KEY (user_id)
    REFERENCES users(id)
    ON DELETE CASCADE
    );

CREATE TABLE IF NOT EXISTS reactions (
                                         id SERIAL PRIMARY KEY,
                                         post_id INTEGER NOT NULL,
                                         user_id INTEGER NOT NULL,
                                         type VARCHAR(20) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_reactions_posts
    FOREIGN KEY (post_id)
    REFERENCES posts(id)
    ON DELETE CASCADE,

    CONSTRAINT fk_reactions_users
    FOREIGN KEY (user_id)
    REFERENCES users(id)
    ON DELETE CASCADE,

    CONSTRAINT ck_reactions_type
    CHECK (type IN ('Like', 'Love', 'Funny')),

    CONSTRAINT uq_reactions_user_post
    UNIQUE (user_id, post_id)
    );

CREATE TABLE IF NOT EXISTS follows (
                                       id SERIAL PRIMARY KEY,
                                       follower_id INTEGER NOT NULL,
                                       followed_id INTEGER NOT NULL,
                                       created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

                                       CONSTRAINT fk_follows_follower
                                       FOREIGN KEY (follower_id)
    REFERENCES users(id)
    ON DELETE CASCADE,

    CONSTRAINT fk_follows_followed
    FOREIGN KEY (followed_id)
    REFERENCES users(id)
    ON DELETE CASCADE,

    CONSTRAINT ck_follows_not_self
    CHECK (follower_id <> followed_id),

    CONSTRAINT uq_follows_follower_followed
    UNIQUE (follower_id, followed_id)
    );