using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaPersistentSqlServer.Repositories
{
    public class MSSqlCommentRepository : ICommentRepository
    {
        private readonly string _connectionString;
        public MSSqlCommentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region GetAll
        public List<Comment> GetAll()
        {
            var Comment = new List<Comment>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Comment";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var comment = new Comment()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Text = (string)reader["Text"],
                            DateOfPost = Convert.ToDateTime(reader["Date"]),
                            CountOfLikes = Convert.ToInt32(reader["CountOfLikes"]),
                            NewsID = Convert.ToInt32(reader["NewsID"]),
                            UserID = Convert.ToInt32(reader["UserID"])
                        };
                        Comment.Add(comment);
                    }
                }
            }
            return Comment;
        }
        #endregion

        #region Insert
        public void Insert(Comment comment)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Comment (Text, Date, CountOfLikes, NewsID, UserID) " +
                    "VALUES(@Text,@Date,@CountOfLikes,@NewsID,@UserID)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Text", comment.Text);
                    command.Parameters.AddWithValue("@Date", comment.DateOfPost);
                    command.Parameters.AddWithValue("@CountOfLikes", comment.CountOfLikes);
                    command.Parameters.AddWithValue("@NewsID", comment.NewsID);
                    command.Parameters.AddWithValue("@UserID", comment.UserID);

                    int rowsCount = command.ExecuteNonQuery();
                    if (rowsCount != 1)
                        throw new Exception("Something went wrong while updating post");
                    else
                        Console.WriteLine("The operation was completed successfully");
                }
            }
        }
        #endregion

        #region GetByID
        public Comment? GetByID(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
            SELECT ID, NewsID, UserID, Text, CountOfLikes, DateOfPost
            FROM Comment
            WHERE ID = @ID
            """;

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Comment
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                NewsID = Convert.ToInt32(reader["NewsID"]),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Text = (string)reader["Text"],
                                CountOfLikes = Convert.ToInt32(reader["CountOfLikes"]),
                                DateOfPost = Convert.ToDateTime(reader["DateOfPost"])
                            };
                        }
                    }
                }
            }

            return null;
        }
        #endregion
    }
}
