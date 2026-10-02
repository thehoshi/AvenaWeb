-- Avena v6 database migration
-- Run once against the existing PostgreSQL database before deploying v6.

ALTER TABLE "News"
    ADD COLUMN IF NOT EXISTS "AuthorID" INTEGER NULL;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'FK_News_Author'
    ) THEN
        ALTER TABLE "News"
            ADD CONSTRAINT "FK_News_Author"
            FOREIGN KEY ("AuthorID") REFERENCES "User"("ID")
            ON DELETE SET NULL;
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS "IX_News_AuthorID" ON "News"("AuthorID");
