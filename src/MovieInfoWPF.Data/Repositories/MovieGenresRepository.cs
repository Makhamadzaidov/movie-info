using MovieInfo.Data.Interfaces;
using MovieInto.Domain.Configurations;
using MovieInto.Domain.Constants;
using MovieInto.Domain.Entities;
using Npgsql;

namespace MovieInfo.Data.Repositories
{
    public class MovieGenresRepository : IMovieGenresRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);
        public async Task<bool> CreateAsync(MovieGenres entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO movie_genreses(movie_id, genres_id) VALUES(@MovieId, @GenresId)";
                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("MovieId", entity.MovieId),
                        new("GenresId", entity.GenresId)
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
                string query = $"DELETE FROM movie_genreses WHERE id = {id}";
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

        public async Task<IEnumerable<MovieGenres>> ReadAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM movie_genreses ORDER BY id ASC OFFSET {@params.SkipCount} LIMIT {@params.PageSize}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();

                ICollection<MovieGenres> movieGenreses = new List<MovieGenres>();
                while (await reader.ReadAsync())
                {
                    MovieGenres movieGenres = new MovieGenres();
                    movieGenres.Id = reader.GetInt64(0);
                    movieGenres.MovieId = reader.GetInt64(1);
                    movieGenres.GenresId = reader.GetInt64(2);
                    movieGenreses.Add(movieGenres);
                }
                return movieGenreses;
            }
            catch
            {
                return new List<MovieGenres>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<MovieGenres> ReadAsync(Int64 id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM movie_genreses WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                return new MovieGenres()
                {
                    Id = reader.GetInt64(0),
                    MovieId = reader.GetInt64(1),
                    GenresId = reader.GetInt64(2)
                };
            }
            catch
            {
                return new MovieGenres();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateAsync(Int64 id, MovieGenres entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"UPDATE movie_genreses SET movie_id = @MovieId, genres_id = @GenresId WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("MovieId", entity.MovieId),
                        new("GenresId", entity.GenresId)
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
