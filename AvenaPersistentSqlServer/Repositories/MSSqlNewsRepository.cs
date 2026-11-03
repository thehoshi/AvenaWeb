using AvenaCore.Entities;
using AvenaCore.Repositories;
using Npgsql;

namespace AvenaPersistentSqlServer.Repositories
{
    public class MSSqlNewsRepository : INewsRepository
    {
        private readonly string _connectionString;

        private const string SelectSql = """
            SELECT n."ID", n."Title", n."Info", n."Summary", n."Slug", n."Status",
                   n."Image", n."DateOfPost", n."PublishedAt", n."CreatedAt", n."UpdatedAt",
                   n."Views", n."CountOfLikes", n."GenreID", n."AuthorID", n."DeletedAt",
                   a."ID" AS "AuthorRowID", a."FullName" AS "AuthorFullName",
                   a."Role" AS "AuthorRole", a."Affiliation" AS "AuthorAffiliation",
                   a."Location" AS "AuthorLocation"
            FROM "News" n
            LEFT JOIN "Author" a ON a."ID" = n."AuthorID"
            """;

        public MSSqlNewsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<News> GetByGenreId(int genreId) => QueryNews(
            "n.\"GenreID\" = @GenreID AND n.\"Status\" = 'published' AND n.\"DeletedAt\" IS NULL",
            command => command.Parameters.AddWithValue("@GenreID", genreId));

        public List<News> GetAll() => QueryNews(
            "n.\"Status\" = 'published' AND n.\"DeletedAt\" IS NULL");

        public List<News> GetAllForAdmin() => QueryNews(
            "n.\"DeletedAt\" IS NULL");

        public News? GetByID(int id) => QueryNews(
            "n.\"ID\" = @ID AND n.\"Status\" = 'published' AND n.\"DeletedAt\" IS NULL",
            command => command.Parameters.AddWithValue("@ID", id)).FirstOrDefault();

        public News? GetByIDForAdmin(int id) => QueryNews(
            "n.\"ID\" = @ID AND n.\"DeletedAt\" IS NULL",
            command => command.Parameters.AddWithValue("@ID", id)).FirstOrDefault();

        public News? GetBySlug(string slug) => QueryNews(
            "n.\"Slug\" = @Slug AND n.\"Status\" = 'published' AND n.\"DeletedAt\" IS NULL",
            command => command.Parameters.AddWithValue("@Slug", slug)).FirstOrDefault();

        public bool SlugExists(string slug, int? exceptId = null)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            const string query = """
                SELECT EXISTS (
                    SELECT 1 FROM "News"
                    WHERE LOWER("Slug") = LOWER(@Slug)
                      AND "ID" <> @ExceptID
                )
                """;

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@Slug", slug);
            command.Parameters.AddWithValue("@ExceptID", exceptId ?? 0);
            return Convert.ToBoolean(command.ExecuteScalar());
        }

        public void Insert(News news) => _ = Create(news);

        public int Create(News news)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var createdAt = news.CreatedAt == default ? DateTime.UtcNow : news.CreatedAt;
            var dateOfPost = news.DateOfPost == default ? createdAt : news.DateOfPost;
            DateTime? publishedAt = news.PublishedAt;
            if (publishedAt == null && news.Status == NewsStatuses.Published)
                publishedAt = dateOfPost;

            const string query = """
                INSERT INTO "News"
                    ("Title", "Info", "Summary", "Slug", "Status", "Image", "Views",
                     "CountOfLikes", "GenreID", "AuthorID", "DateOfPost", "PublishedAt",
                     "CreatedAt", "UpdatedAt", "DeletedAt")
                VALUES
                    (@Title, @Info, @Summary, @Slug, @Status, @Image, @Views,
                     @CountOfLikes, @GenreID, @AuthorID, @DateOfPost, @PublishedAt,
                     @CreatedAt, @UpdatedAt, @DeletedAt)
                RETURNING "ID"
                """;

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", news.Title);
            command.Parameters.AddWithValue("@Info", news.Info);
            command.Parameters.AddWithValue("@Summary", (object?)news.Summary ?? DBNull.Value);
            command.Parameters.AddWithValue("@Slug", (object?)news.Slug ?? DBNull.Value);
            command.Parameters.AddWithValue("@Status", news.Status);
            command.Parameters.AddWithValue("@Image", (object?)news.Image ?? DBNull.Value);
            command.Parameters.AddWithValue("@Views", news.Views);
            command.Parameters.AddWithValue("@CountOfLikes", news.CountOfLikes);
            command.Parameters.AddWithValue("@GenreID", news.GenreID);
            command.Parameters.AddWithValue("@AuthorID", (object?)news.AuthorID ?? DBNull.Value);
            command.Parameters.AddWithValue("@DateOfPost", dateOfPost);
            command.Parameters.AddWithValue("@PublishedAt", (object?)publishedAt ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedAt", createdAt);
            command.Parameters.AddWithValue("@UpdatedAt", (object?)news.UpdatedAt ?? DBNull.Value);
            command.Parameters.AddWithValue("@DeletedAt", (object?)news.DeletedAt ?? DBNull.Value);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        public void UpdateArticle(News news)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            const string query = """
                UPDATE "News"
                SET "Title" = @Title,
                    "Info" = @Info,
                    "Summary" = @Summary,
                    "Slug" = @Slug,
                    "Image" = @Image,
                    "GenreID" = @GenreID,
                    "UpdatedAt" = @UpdatedAt
                WHERE "ID" = @ID AND "DeletedAt" IS NULL
                """;

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", news.ID);
            command.Parameters.AddWithValue("@Title", news.Title);
            command.Parameters.AddWithValue("@Info", news.Info);
            command.Parameters.AddWithValue("@Summary", (object?)news.Summary ?? DBNull.Value);
            command.Parameters.AddWithValue("@Slug", (object?)news.Slug ?? DBNull.Value);
            command.Parameters.AddWithValue("@Image", (object?)news.Image ?? DBNull.Value);
            command.Parameters.AddWithValue("@GenreID", news.GenreID);
            command.Parameters.AddWithValue("@UpdatedAt", news.UpdatedAt ?? DateTime.UtcNow);

            if (command.ExecuteNonQuery() != 1)
                throw new InvalidOperationException("News article was not found.");
        }

        public void PublishArticle(int newsId, DateTime publishedAt)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            const string query = """
                UPDATE "News"
                SET "Status" = 'published',
                    "DateOfPost" = COALESCE("PublishedAt", @PublishedAt),
                    "PublishedAt" = COALESCE("PublishedAt", @PublishedAt),
                    "UpdatedAt" = @PublishedAt
                WHERE "ID" = @ID AND "DeletedAt" IS NULL
                """;

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", newsId);
            command.Parameters.AddWithValue("@PublishedAt", publishedAt);
            if (command.ExecuteNonQuery() != 1)
                throw new InvalidOperationException("News article was not found.");
        }

        public void AddLike(int newsId) => UpdatePublicCount(
            "\"CountOfLikes\" = \"CountOfLikes\" + 1", newsId);

        public void IncrementViews(int newsId) => UpdatePublicCount(
            "\"Views\" = \"Views\" + 1", newsId);

        private void UpdatePublicCount(string setClause, int newsId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var query = $"""
                UPDATE "News"
                SET {setClause}
                WHERE "ID" = @ID AND "Status" = 'published' AND "DeletedAt" IS NULL
                """;
            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", newsId);
            command.ExecuteNonQuery();
        }

        private List<News> QueryNews(string condition, Action<NpgsqlCommand>? configure = null)
        {
            var newsList = new List<News>();
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var query = $"""
                {SelectSql}
                WHERE {condition}
                ORDER BY COALESCE(n."PublishedAt", n."DateOfPost") DESC, n."ID" DESC
                """;

            using var command = new NpgsqlCommand(query, connection);
            configure?.Invoke(command);

            using var reader = command.ExecuteReader();
            while (reader.Read())
                newsList.Add(ReadNews(reader));

            return newsList;
        }

        private static News ReadNews(NpgsqlDataReader reader)
        {
            var dateOfPost = Convert.ToDateTime(reader["DateOfPost"]);
            var status = (string)reader["Status"];
            DateTime? publishedAt = null;
            if (reader["PublishedAt"] != DBNull.Value)
                publishedAt = Convert.ToDateTime(reader["PublishedAt"]);
            if (publishedAt == null && status == NewsStatuses.Published)
                publishedAt = dateOfPost;

            return new News
            {
                ID = Convert.ToInt32(reader["ID"]),
                Title = (string)reader["Title"],
                Info = (string)reader["Info"],
                Summary = reader["Summary"] == DBNull.Value ? null : (string)reader["Summary"],
                Slug = reader["Slug"] == DBNull.Value ? null : (string)reader["Slug"],
                Status = status,
                Image = reader["Image"] == DBNull.Value ? null : (string)reader["Image"],
                DateOfPost = dateOfPost,
                PublishedAt = publishedAt,
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? null : Convert.ToDateTime(reader["UpdatedAt"]),
                Views = Convert.ToInt32(reader["Views"]),
                CountOfLikes = Convert.ToInt32(reader["CountOfLikes"]),
                GenreID = Convert.ToInt32(reader["GenreID"]),
                AuthorID = reader["AuthorID"] == DBNull.Value ? null : Convert.ToInt32(reader["AuthorID"]),
                Author = reader["AuthorRowID"] == DBNull.Value ? null : new Author
                {
                    ID = Convert.ToInt32(reader["AuthorRowID"]),
                    FullName = (string)reader["AuthorFullName"],
                    Role = reader["AuthorRole"] == DBNull.Value ? null : (string)reader["AuthorRole"],
                    Affiliation = reader["AuthorAffiliation"] == DBNull.Value ? null : (string)reader["AuthorAffiliation"],
                    Location = reader["AuthorLocation"] == DBNull.Value ? null : (string)reader["AuthorLocation"]
                },
                DeletedAt = reader["DeletedAt"] == DBNull.Value ? null : Convert.ToDateTime(reader["DeletedAt"])
            };
        }
    }
}
