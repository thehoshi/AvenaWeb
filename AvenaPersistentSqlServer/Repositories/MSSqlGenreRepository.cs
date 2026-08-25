using AvenaCore.Entities;
using AvenaCore.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvenaPersistentSqlServer.Repositories
{
    public class MSSqlGenreRepository : IGenreRepository
    {
        private readonly string _connectionString;
        public MSSqlGenreRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region GetAll
        public List<Genre> GetAll()
        {
            var Genre = new List<Genre>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Genre";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var genre = new Genre()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            NameGenre = (string)reader["Namegenre"]
                        };
                        Genre.Add(genre);
                    }
                }
            }
            return Genre;
        }
        #endregion

        #region Insert
        public void Insert(Genre genre)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Genre (NameGenre) " +
                    "VALUES(@NameGenre)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NameGenre", genre.NameGenre);

                    int rowsCount = command.ExecuteNonQuery();
                    if (rowsCount != 1)
                        throw new Exception("Something went wrong while updating genre");
                    else
                        Console.WriteLine("The operation was completed successfully");
                }
            }
        }
        #endregion

        #region GetById
        public Genre? GetByID(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT ID, NameGenre FROM Genre WHERE ID = @ID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Genre
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                NameGenre = (string)reader["NameGenre"]
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
