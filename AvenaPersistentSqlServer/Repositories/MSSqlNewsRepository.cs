using AvenaCore.Entities;
using AvenaCore.Repositories;
using Npgsql;

namespace AvenaPersistentSqlServer.Repositories
{
    public class MSSqlNewsRepository : INewsRepository
    {
        private readonly string _connectionString;
        public MSSqlNewsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region GetByGenreId
        public List<News> GetByGenreId(int genreId)
        {
            var newsList = new List<News>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
            SELECT n."ID", n."Title", n."Info", n."Image", n."DateOfPost",
                   n."Views", n."CountOfLikes", n."GenreID", n."AuthorID", n."DeletedAt",
                   a."ID" AS "AuthorRowID", a."FullName" AS "AuthorFullName",
                   a."Role" AS "AuthorRole", a."Affiliation" AS "AuthorAffiliation",
                   a."Location" AS "AuthorLocation"
            FROM "News" n
            LEFT JOIN "Author" a ON a."ID" = n."AuthorID"
            WHERE n."GenreID" = @GenreID
              AND n."DeletedAt" IS NULL
            ORDER BY n."DateOfPost" DESC, n."ID" DESC
            """;

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GenreID", genreId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            newsList.Add(new News
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Title = (string)reader["Title"],
                                Info = (string)reader["Info"],

                                Image = reader["Image"] == DBNull.Value
                                    ? null
                                    : (string)reader["Image"],

                                DateOfPost = Convert.ToDateTime(reader["DateOfPost"]),
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

                                DeletedAt = reader["DeletedAt"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(reader["DeletedAt"])
                            });
                        }
                    }
                }
            }

            return newsList;
        }
        #endregion

        #region GetAll
        public List<News> GetAll()
        {
            var newsList = new List<News>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
            SELECT n."ID", n."Title", n."Info", n."Image", n."DateOfPost",
                   n."Views", n."CountOfLikes", n."GenreID", n."AuthorID", n."DeletedAt",
                   a."ID" AS "AuthorRowID", a."FullName" AS "AuthorFullName",
                   a."Role" AS "AuthorRole", a."Affiliation" AS "AuthorAffiliation",
                   a."Location" AS "AuthorLocation"
            FROM "News" n
            LEFT JOIN "Author" a ON a."ID" = n."AuthorID"
            WHERE n."DeletedAt" IS NULL
            ORDER BY n."DateOfPost" DESC, n."ID" DESC
            """;

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var news = new News
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Title = (string)reader["Title"],
                            Info = (string)reader["Info"],

                            Image = reader["Image"] == DBNull.Value
                                ? null
                                : (string)reader["Image"],

                            DateOfPost = Convert.ToDateTime(reader["DateOfPost"]),
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

                            DeletedAt = reader["DeletedAt"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["DeletedAt"])
                        };

                        newsList.Add(news);
                    }
                }
            }

            return newsList;
        }
        #endregion

        #region Insert
        public void Insert(News news)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
                    INSERT INTO "News" ("Title", "Info", "Image", "DateOfPost", "Views", "CountOfLikes", "GenreID", "AuthorID", "DeletedAt")
                    VALUES (@Title, @Info, @Image, @DateOfPost, @Views, @CountOfLikes, @GenreID, @AuthorID, @DeletedAt)
                    """;
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", news.Title);
                    command.Parameters.AddWithValue("@Info", news.Info);
                    command.Parameters.AddWithValue("@Image", (object?)news.Image ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfPost", news.DateOfPost);
                    command.Parameters.AddWithValue("@Views", news.Views);
                    command.Parameters.AddWithValue("@CountOfLikes", news.CountOfLikes);
                    command.Parameters.AddWithValue("@GenreID", news.GenreID);
                    command.Parameters.AddWithValue("@AuthorID", (object?)news.AuthorID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DeletedAt", (object?)news.DeletedAt ?? DBNull.Value);

                    int rowsCount = command.ExecuteNonQuery();
                    if (rowsCount != 1)
                        throw new Exception("Something went wrong while updating post");
                    else
                        Console.WriteLine("The operation was completed successfully");
                }
            }
        }
        #endregion

        #region GetById
        public News? GetByID(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
            SELECT n."ID", n."Title", n."Info", n."Image", n."Views", n."CountOfLikes",
                   n."GenreID", n."AuthorID", n."DateOfPost", n."DeletedAt",
                   a."ID" AS "AuthorRowID", a."FullName" AS "AuthorFullName",
                   a."Role" AS "AuthorRole", a."Affiliation" AS "AuthorAffiliation",
                   a."Location" AS "AuthorLocation"
            FROM "News" n
            LEFT JOIN "Author" a ON a."ID" = n."AuthorID"
            WHERE n."ID" = @ID
            """;

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new News
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Title = (string)reader["Title"],
                                Info = (string)reader["Info"],
                                Image = reader["Image"] == DBNull.Value
                                    ? null
                                    : (string)reader["Image"],
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
                                DateOfPost = Convert.ToDateTime(reader["DateOfPost"]),
                                DeletedAt = reader["DeletedAt"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(reader["DeletedAt"])
                            };
                        }
                    }
                }
            }

            return null;
        }
        #endregion

        #region AddLike

        public void AddLike(int newsId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
            UPDATE "News"
            SET "CountOfLikes" = "CountOfLikes" + 1
            WHERE "ID" = @ID
            """;

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", newsId);

                    int rowsCount = command.ExecuteNonQuery();

                    if (rowsCount != 1)
                        throw new Exception("News article was not found.");
                }
            }
        }

        #endregion

        #region IncrementViews

        public void IncrementViews(int newsId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
            UPDATE "News"
            SET "Views" = "Views" + 1
            WHERE "ID" = @ID
            """;

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", newsId);
                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Create

        public int Create(News news)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
            INSERT INTO "News"
                ("Title", "Info", "Image", "Views", "CountOfLikes", "GenreID", "AuthorID", "DateOfPost", "DeletedAt")
            VALUES
                (@Title, @Info, @Image, @Views, @CountOfLikes, @GenreID, @AuthorID, @DateOfPost, @DeletedAt)
            RETURNING "ID"
            """;

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", news.Title);
                    command.Parameters.AddWithValue("@Info", news.Info);

                    command.Parameters.AddWithValue(
                        "@Image",
                        (object?)news.Image ?? DBNull.Value);

                    command.Parameters.AddWithValue("@Views", news.Views);
                    command.Parameters.AddWithValue("@CountOfLikes", news.CountOfLikes);
                    command.Parameters.AddWithValue("@GenreID", news.GenreID);
                    command.Parameters.AddWithValue("@AuthorID", (object?)news.AuthorID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfPost", news.DateOfPost);

                    command.Parameters.AddWithValue(
                        "@DeletedAt",
                        (object?)news.DeletedAt ?? DBNull.Value);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        #endregion
    }
}
