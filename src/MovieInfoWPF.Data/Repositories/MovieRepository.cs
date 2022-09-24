using MovieInfo.Data.Interfaces;
using MovieInto.Domain.Configurations;
using MovieInto.Domain.Constants;
using MovieInto.Domain.Entities;
using Npgsql;

namespace MovieInfo.Data.Repositories
{
    #pragma warning disable
    public class MovieRepository : IMovieRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);

        public async Task<bool> CreateAsync(Movie entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO movies(name, movie_year, duration, language, premiere_date) " +
                               "VALUES(@Name, @MovieYear, @Duration, @Language, @PremiereDate)";

                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("Name", entity.Name),
                        new("MovieYear", entity.MovieYear),
                        new("Duration", entity.Duration),
                        new("Language", entity.Language),
                        new("PremiereDate", entity.PremiereDate),
                    }
                };

                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> DeleteAsync(Int64 id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"DELETE FROM movies WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection);
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<IEnumerable<Movie>> ReadAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM movies ORDER BY id ASC OFFSET {@params.SkipCount} LIMIT {@params.PageSize}";
                
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();

                ICollection<Movie> movies = new List<Movie>();
                while (await reader.ReadAsync())
                {
                    Movie movie = new Movie();
                    movie.Id = reader.GetInt64(0);
                    movie.Name = reader.GetString(1);
                    movie.MovieYear = DateOnly.Parse($"{reader.GetDate(2)}");
                    movie.Duration = reader.GetString(3);
                    movie.Language = reader.GetString(4);
                    movie.PremiereDate = DateOnly.Parse($"{reader.GetDate(5)}");
                    movie.CreatedDate = reader.GetDateTime(6);

                    if (!reader.IsDBNull(7))
                        movie.UpdatedDate = reader.GetDateTime(7);
                    movies.Add(movie);
                }

                return movies;
            }
            catch
            {
                return new List<Movie>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<Movie> ReadAsync(Int64 id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM movies WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                Movie movie = new Movie();
                movie.Id = reader.GetInt64(0);
                movie.Name = reader.GetString(1);
                movie.MovieYear = DateOnly.Parse($"{reader.GetDate(2)}");
                movie.Duration = reader.GetString(3);
                movie.Language = reader.GetString(4);
                movie.PremiereDate = DateOnly.Parse($"{reader.GetDate(5)}");
                movie.CreatedDate = reader.GetDateTime(6);

                if (!reader.IsDBNull(7))
                    movie.UpdatedDate = reader.GetDateTime(7);
                return movie;
            }
            catch
            {
                return new Movie();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateAsync(Int64 id, Movie entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "UPDATE movies " +
                               "SET name = @Name," +
                               "movie_year = @MovieYear," +
                               "duration = @Duration," +
                               "language = @Language," +
                               "premiere_date = @PremiereDate," +
                               $"updated_date = @UpdatedDate " +
                               $"WHERE id = {id}";

                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("Name", entity.Name),
                        new("MovieYear", entity.MovieYear),
                        new("Duration", entity.Duration),
                        new("Language", entity.Language),
                        new("PremiereDate", entity.PremiereDate),
                        new("UpdatedDate", entity.UpdatedDate = DateTime.Now)
                    }
                };
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
    }
}
