-- Схема базы Avena под PostgreSQL (Supabase).
-- Вставь и выполни это целиком в Supabase: Project -> SQL Editor -> New query.

CREATE TABLE "Genre" (
    "ID" SERIAL PRIMARY KEY,
    "NameGenre" TEXT NOT NULL
);

CREATE TABLE "User" (
    "ID" SERIAL PRIMARY KEY,
    "Username" TEXT NOT NULL UNIQUE,
    "Name" TEXT NOT NULL,
    "Password" TEXT NOT NULL,
    "AvatarImg" TEXT NOT NULL DEFAULT ''
);

-- Usernames are treated case-insensitively by the application.
CREATE UNIQUE INDEX IF NOT EXISTS "UX_User_Username_Lower"
ON "User" (LOWER("Username"));

CREATE TABLE "News" (
    "ID" SERIAL PRIMARY KEY,
    "Title" TEXT NOT NULL,
    "Info" TEXT NOT NULL,
    "Image" TEXT,
    "Views" INTEGER NOT NULL DEFAULT 0,
    "CountOfLikes" INTEGER NOT NULL DEFAULT 0,
    "GenreID" INTEGER NOT NULL REFERENCES "Genre"("ID"),
    "DateOfPost" TIMESTAMP NOT NULL DEFAULT NOW(),
    "DeletedAt" TIMESTAMP
);

CREATE TABLE "Comment" (
    "ID" SERIAL PRIMARY KEY,
    "NewsID" INTEGER NOT NULL REFERENCES "News"("ID"),
    "UserID" INTEGER NOT NULL REFERENCES "User"("ID"),
    "Text" TEXT NOT NULL,
    "CountOfLikes" INTEGER NOT NULL DEFAULT 0,
    "DateOfPost" TIMESTAMP NOT NULL DEFAULT NOW()
);

-- (Опционально) те же тестовые жанры, что были в сид-данных:
INSERT INTO "Genre" ("NameGenre") VALUES
    ('Politics'), ('Technology'), ('Sports'), ('World');
