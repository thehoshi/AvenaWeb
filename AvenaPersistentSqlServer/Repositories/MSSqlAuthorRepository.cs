using AvenaCore.Entities;
using AvenaCore.Repositories;
using Npgsql;

namespace AvenaPersistentSqlServer.Repositories;

public class MSSqlAuthorRepository : IAuthorRepository
{
    private readonly string _connectionString;

    public MSSqlAuthorRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Author> GetAll()
    {
        var authors = new List<Author>();
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = """
            SELECT "ID", "FullName", "Role", "Affiliation", "Location"
            FROM "Author"
            ORDER BY "FullName"
            """;

        using var command = new NpgsqlCommand(query, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
            authors.Add(ReadAuthor(reader));

        return authors;
    }

    public Author? GetByID(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = """
            SELECT "ID", "FullName", "Role", "Affiliation", "Location"
            FROM "Author"
            WHERE "ID" = @ID
            """;

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@ID", id);
        using var reader = command.ExecuteReader();
        return reader.Read() ? ReadAuthor(reader) : null;
    }

    private static Author ReadAuthor(NpgsqlDataReader reader) => new()
    {
        ID = Convert.ToInt32(reader["ID"]),
        FullName = (string)reader["FullName"],
        Role = reader["Role"] == DBNull.Value ? null : (string)reader["Role"],
        Affiliation = reader["Affiliation"] == DBNull.Value ? null : (string)reader["Affiliation"],
        Location = reader["Location"] == DBNull.Value ? null : (string)reader["Location"]
    };
}
