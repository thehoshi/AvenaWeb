using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.Data.SqlClient;

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
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
                    SELECT ID, Username, Name, Password, AvatarImg
                    FROM [User]
                    """;

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Username = (string)reader["Username"],
                            Name = (string)reader["Name"],
                            Password = (string)reader["Password"],
                            AvatarImg = (string)reader["AvatarImg"]
                        });
                    }
                }
            }

            return users;
        }

        #endregion

        #region GetByUsername

        public User? GetByUsername(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
                    SELECT ID, Username, Name, Password, AvatarImg
                    FROM [User]
                    WHERE Username = @Username
                    """;

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

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

        #region GetByID

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

        #region Insert

        public void Insert(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
                    INSERT INTO [User]
                        (Username, Name, Password, AvatarImg)
                    VALUES
                        (@Username, @Name, @Password, @AvatarImg)
                    """;

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@AvatarImg", user.AvatarImg);

                    int rowsCount = command.ExecuteNonQuery();

                    if (rowsCount != 1)
                        throw new Exception("Something went wrong while creating the user.");
                }
            }
        }

        #endregion

        #region UpdateProfile

        public void UpdateProfile(int id, string name, string avatarImg)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = """
                    UPDATE [User]
                    SET Name = @Name, AvatarImg = @AvatarImg
                    WHERE ID = @ID
                    """;

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@AvatarImg", avatarImg);

                    int rowsCount = command.ExecuteNonQuery();

                    if (rowsCount != 1)
                        throw new Exception("User was not found.");
                }
            }
        }

        #endregion
    }
}