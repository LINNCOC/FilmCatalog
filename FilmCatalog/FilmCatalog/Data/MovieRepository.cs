using FilmCatalog.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace FilmCatalog.Data
{
    public class MovieRepository
    {
        private readonly string _connectionString =
            @"Data Source = LINN\SQLEXPRESS01;Database=MovieCatalogDB;Integrated Security = True; Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name = ""SQL Server Management Studio""; Command Timeout = 0";

        public List<Movie> GetAllMovies(string searchTitle = null, bool sortByGenre = false)
        {
            var list = new List<Movie>();
            var query = "SELECT Id, Title, ReleaseYear, Genre, Duration, Rating, Description FROM Movies WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(searchTitle))
                query += " AND Title LIKE @Title";
            query += sortByGenre
                ? " ORDER BY Genre, Title"
                : " ORDER BY Id DESC";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                if (!string.IsNullOrWhiteSpace(searchTitle))
                    cmd.Parameters.AddWithValue("@Title", $"%{searchTitle}%");

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(Map(reader));
                }
            }
            return list;
        }

        public Movie GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "SELECT Id, Title, ReleaseYear, Genre, Duration, Rating, Description FROM Movies WHERE Id=@Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    return reader.Read() ? Map(reader) : null;
            }
        }

        private static Movie Map(SqlDataReader reader) => new Movie
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            ReleaseYear = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
            Genre = reader.IsDBNull(3) ? null : reader.GetString(3),
            Duration = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
            Rating = reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
            Description = reader.IsDBNull(6) ? null : reader.GetString(6)
        };

        public void AddMovie(Movie movie)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "INSERT INTO Movies (Title, ReleaseYear, Genre, Duration, Rating, Description) " +
                "VALUES (@Title, @ReleaseYear, @Genre, @Duration, @Rating, @Description)", conn))
            {
                AddParams(cmd, movie);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateMovie(Movie movie)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "UPDATE Movies SET Title=@Title, ReleaseYear=@ReleaseYear, Genre=@Genre, " +
                "Duration=@Duration, Rating=@Rating, Description=@Description WHERE Id=@Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", movie.Id);
                AddParams(cmd, movie);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static void AddParams(SqlCommand cmd, Movie m)
        {
            cmd.Parameters.AddWithValue("@Title", m.Title);
            cmd.Parameters.AddWithValue("@ReleaseYear", (object)m.ReleaseYear ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Genre", (object)m.Genre ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Duration", (object)m.Duration ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Rating", (object)m.Rating ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", (object)m.Description ?? DBNull.Value);
        }

        public void DeleteMovie(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM Movies WHERE Id=@Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}