using MovieInfo.Data.Interfaces;
using MovieInto.Domain.Configurations;
using MovieInto.Domain.Constants;
using MovieInto.Domain.Entities;
using Npgsql;

namespace MovieInfo.Data.Repositories
{
    public class MovieDirectorRepository : IMovieDirectorRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);
        public async Task<bool> CreateAsync(MovieDirector entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO movie_directors(director_id, movie_id) VALUES(@DirectorId, @MovieId)";
                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("DirectorId", entity.DirectorId),
                        new("MovieId", entity.MovieId)
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
                string query = $"DELETE FROM movie_directors WHERE id = {id}";
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

        public async Task<IEnumerable<MovieDirector>> ReadAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM movie_directors ORDER BY id ASC OFFSET {@params.SkipCount} LIMIT {@params.PageSize}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();

                ICollection<MovieDirector> movieDirectors = new List<MovieDirector>();
                while (await reader.ReadAsync())
                {
                    MovieDirector movieDirector = new MovieDirector();
                    movieDirector.Id = reader.GetInt64(0);
                    movieDirector.DirectorId = reader.GetInt64(1);
                    movieDirector.MovieId = reader.GetInt64(2);
                    movieDirectors.Add(movieDirector);
                }
                return movieDirectors;
            }
            catch
            {
                return new List<MovieDirector>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<MovieDirector> ReadAsync(Int64 id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM movie_directors WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                return new MovieDirector()
                {
                    Id = reader.GetInt64(0),
                    DirectorId = reader.GetInt64(1),
                    MovieId = reader.GetInt64(2)
                };
            }
            catch
            {
                return new MovieDirector();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateAsync(Int64 id, MovieDirector entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"UPDATE movie_directors SET director_id = @DirectorId, movie_id = @MovieId WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("DirectorId", entity.DirectorId),
                        new("MovieId", entity.MovieId)
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
