using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace AvenaPersistentSqlServer.Repositories
{
    public class MSSqlUserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public MSSqlUserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #region GetAll
        public List<User> GetAll()
        {
            var User = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM User";
                using(var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Username = (string)reader["Username"],
                            Name = (string)reader["Nickname"],
                            Password = (string)reader["Password"],
                            AvatarImg = (string)reader["IMG"]
                        };
                        User.Add(user);
                    }
                }
            }
            return User;
        }
        #endregion

        #region Insert
        public void Insert(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO User (Username, Name, PasswordHash, AvatarImg) " +
                    "VALUES(@Name,@PasswordHash,@AvatarImg)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@PasswordHash", user.Password);
                    command.Parameters.AddWithValue("@AvatarImg", user.AvatarImg);

                    int rowsCount = command.ExecuteNonQuery();
                    if (rowsCount != 1)
                        throw new Exception("Something went wrong while updating user");
                    else
                        Console.WriteLine("The operation was completed successfully");
                }
            }
        }
        #endregion

        #region GetById
        public User? GetByID(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
            SELECT ID, Username, Name, Password, AvatarImg
            FROM [User]
            WHERE ID = @ID
            """;

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Username = (string)reader["Username"],
                                Name = (string)reader["Name"],
                                Password = (string)reader["Password"],
                                AvatarImg = (string)reader["AvatarImg"]
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
