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
                            ID = Convert.ToInt32(reader["UserID"]),
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

        //#region Insert
        //public void Insert(User user)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        string query = "INSERT INTO User (Username, Name, Password, AvatarImg) " +
        //            "VALUES(@CustomerID,@AccountNumber,@Balance,@AccountType,@Currency, GETDATE() )";
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@CustomerID", account.CustomerID);
        //            command.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
        //            command.Parameters.AddWithValue("@Balance", account.Balance);
        //            command.Parameters.AddWithValue("@AccountType", account.AccountType ?? (object)DBNull.Value);
        //            command.Parameters.AddWithValue("@Currency", account.Currency);

        //            int rowsCount = command.ExecuteNonQuery();
        //            if (rowsCount != 1)
        //                throw new Exception("Something went wrong while updating user");
        //            else
        //                Console.WriteLine("The operation was completed successfully");
        //        }
        //    }
        //}
        //#endregion
    }
}
