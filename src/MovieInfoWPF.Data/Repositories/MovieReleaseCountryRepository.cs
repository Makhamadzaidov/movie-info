using MovieInfo.Data.Interfaces;
using MovieInto.Domain.Configurations;
using MovieInto.Domain.Constants;
using MovieInto.Domain.Entities;
using Npgsql;

namespace MovieInfo.Data.Repositories
{
    public class MovieReleaseCountryRepository : IMovieReleaseCountryRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);

        public async Task<bool> CreateAsync(MovieReleaseCountry entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO movies_release_countries(movie_id, country_id) VALUES(@MovieId, @CountryId)";
                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("MovieId", entity.MovieId),
                        new("CountryId", entity.CountryId)
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
                string query = $"DELETE FROM movies_release_countries WHERE id = {id}";
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

        public async Task<IEnumerable<MovieReleaseCountry>> ReadAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM movies_release_countries ORDER BY id ASC OFFSET {@params.SkipCount} LIMIT {@params.PageSize}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();

                ICollection<MovieReleaseCountry> movieReleaseCountries = new List<MovieReleaseCountry>();
                while (await reader.ReadAsync())
                {
                    MovieReleaseCountry movieReleaseCountry = new MovieReleaseCountry();
                    movieReleaseCountry.Id = reader.GetInt64(0);
                    movieReleaseCountry.MovieId = reader.GetInt64(1);
                    movieReleaseCountry.CountryId = reader.GetInt64(2);
                    movieReleaseCountries.Add(movieReleaseCountry);
                }
                return movieReleaseCountries;
            }
            catch
            {
                return new List<MovieReleaseCountry>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<MovieReleaseCountry> ReadAsync(Int64 id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM movies_release_countries WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                return new MovieReleaseCountry()
                {
                    Id = reader.GetInt64(0),
                    MovieId = reader.GetInt64(1),
                    CountryId = reader.GetInt64(2)
                };
            }
            catch
            {
                return new MovieReleaseCountry();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateAsync(Int64 id, MovieReleaseCountry entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"UPDATE movies_release_countries SET movie_id = @MovieId, country_id = @CountryId WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("MovieId", entity.MovieId),
                        new("CountryId", entity.CountryId)
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
