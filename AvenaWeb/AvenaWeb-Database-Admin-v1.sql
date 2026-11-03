-- Avena admin publishing v1
-- Run once against the existing PostgreSQL/Supabase database before deploying.
BEGIN;

ALTER TABLE "User"
    ADD COLUMN IF NOT EXISTS "IsAdmin" BOOLEAN NOT NULL DEFAULT FALSE;

ALTER TABLE "News"
    ADD COLUMN IF NOT EXISTS "Summary" TEXT NULL,
    ADD COLUMN IF NOT EXISTS "Slug" TEXT NULL,
    ADD COLUMN IF NOT EXISTS "Status" TEXT NOT NULL DEFAULT 'published',
    ADD COLUMN IF NOT EXISTS "PublishedAt" TIMESTAMPTZ NULL,
    ADD COLUMN IF NOT EXISTS "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    ADD COLUMN IF NOT EXISTS "UpdatedAt" TIMESTAMPTZ NULL;

UPDATE "News"
SET "PublishedAt" = "DateOfPost"
WHERE "Status" = 'published' AND "PublishedAt" IS NULL;

UPDATE "News"
SET "Summary" = LEFT(BTRIM("Info"), 320)
WHERE "Summary" IS NULL;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'CK_News_Status'
          AND conrelid = '"News"'::regclass
    ) THEN
        ALTER TABLE "News"
            ADD CONSTRAINT "CK_News_Status"
            CHECK ("Status" IN ('draft', 'published'));
    END IF;
END $$;

CREATE UNIQUE INDEX IF NOT EXISTS "UX_News_Slug"
    ON "News" (LOWER("Slug"))
    WHERE "Slug" IS NOT NULL;

CREATE INDEX IF NOT EXISTS "IX_News_Status_DateOfPost"
    ON "News" ("Status", "DateOfPost" DESC)
    WHERE "DeletedAt" IS NULL;

COMMIT;
