using AvenaCore.Entities;
using AvenaCore.Repositories;
using Npgsql;

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
            var genres = new List<Genre>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = """SELECT "ID", "NameGenre" FROM "Genre" """;
                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var genre = new Genre()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            NameGenre = (string)reader["NameGenre"]
                        };
                        genres.Add(genre);
                    }
                }
            }
            return genres;
        }
        #endregion

        #region Insert
        public void Insert(Genre genre)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = """INSERT INTO "Genre" ("NameGenre") VALUES (@NameGenre)""";
                using (var command = new NpgsqlCommand(query, connection))
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
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = """SELECT "ID", "NameGenre" FROM "Genre" WHERE "ID" = @ID""";

                using (var command = new NpgsqlCommand(query, connection))
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

        #region GetOrCreate
        public int GetOrCreate(string nameGenre)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = """SELECT "ID" FROM "Genre" WHERE "NameGenre" = @NameGenre""";

                using (var selectCommand = new NpgsqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@NameGenre", nameGenre);

                    var existingId = selectCommand.ExecuteScalar();

                    if (existingId != null)
                        return Convert.ToInt32(existingId);
                }

                string insertQuery = """
                    INSERT INTO "Genre" ("NameGenre")
                    VALUES (@NameGenre)
                    RETURNING "ID"
                    """;

                using (var insertCommand = new NpgsqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@NameGenre", nameGenre);

                    return (int)insertCommand.ExecuteScalar()!;
                }
            }
        }
        #endregion
    }
}
