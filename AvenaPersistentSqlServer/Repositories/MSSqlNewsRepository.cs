using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaPersistentSqlServer.Repositories
{
    public class MSSqlNewsRepository :INewsRepository
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

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
            SELECT ID, Title, Info, Image, DateOfPost,
                   Views, CountOfLikes, GenreID, DeletedAt
            FROM News
            WHERE GenreID = @GenreID
              AND DeletedAt IS NULL
            ORDER BY DateOfPost DESC
            """;

                using (var command = new SqlCommand(query, connection))
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

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
            SELECT ID, Title, Info, Image, DateOfPost,
                   Views, CountOfLikes, GenreID, DeletedAt
            FROM News
            WHERE DeletedAt IS NULL
            ORDER BY DateOfPost DESC
            """;

                using (var command = new SqlCommand(query, connection))
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO News (Title, Info, Image, DateOfPost, Views, CountOfLikes, GenreID, DeletedAt) " +
                    "VALUES(@Title,@Info,@Image,@DateOfImage,@Views,@CountOfLikes,@GenreID,@DeletedAt)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", news.Title);
                    command.Parameters.AddWithValue("@Info", news.Info);
                    command.Parameters.AddWithValue("@Image", news.Image);
                    command.Parameters.AddWithValue("@DateOfPost", news.DateOfPost);
                    command.Parameters.AddWithValue("@Views", news.Views);
                    command.Parameters.AddWithValue("@CountOfLikes", news.CountOfLikes);
                    command.Parameters.AddWithValue("@GenreID", news.GenreID);
                    command.Parameters.AddWithValue("@DeletedAt", news.DeletedAt);

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
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
            SELECT ID, Title, Info, Image, Views, CountOfLikes,
                   GenreID, DateOfPost, DeletedAt
            FROM News
            WHERE ID = @ID
            """;

                using (var command = new SqlCommand(query, connection))
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
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
            UPDATE News
            SET CountOfLikes = CountOfLikes + 1
            WHERE ID = @ID
            """;

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", newsId);

                    int rowsCount = command.ExecuteNonQuery();

                    if (rowsCount != 1)
                        throw new Exception("News article was not found.");
                }
            }
        }

        #endregion

        #region Create

        public int Create(News news)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
            INSERT INTO News
                (Title, Info, Image, Views, CountOfLikes, GenreID, DateOfPost, DeletedAt)
            OUTPUT INSERTED.ID
            VALUES
                (@Title, @Info, @Image, @Views, @CountOfLikes, @GenreID, @DateOfPost, @DeletedAt)
            """;

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", news.Title);
                    command.Parameters.AddWithValue("@Info", news.Info);

                    command.Parameters.AddWithValue(
                        "@Image",
                        (object?)news.Image ?? DBNull.Value);

                    command.Parameters.AddWithValue("@Views", news.Views);
                    command.Parameters.AddWithValue("@CountOfLikes", news.CountOfLikes);
                    command.Parameters.AddWithValue("@GenreID", news.GenreID);
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
