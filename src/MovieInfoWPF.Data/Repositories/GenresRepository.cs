using MovieInfo.Data.Interfaces;
using MovieInto.Domain.Configurations;
using MovieInto.Domain.Constants;
using MovieInto.Domain.Entities;
using Npgsql;

namespace MovieInfo.Data.Repositories
{
    public class GenresRepository : IGenresRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);

        public async Task<bool> CreateAsync(Genres entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO genreses(name) VALUES(@Name)";

                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("Name", entity.Name),
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
                string query = $"DELETE FROM genreses WHERE id = {id}";
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

        public async Task<IEnumerable<Genres>> ReadAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM genreses ORDER BY id ASC OFFSET {@params.SkipCount} LIMIT {@params.PageSize}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();

                ICollection<Genres> genreses = new List<Genres>();
                while (await reader.ReadAsync())
                {
                    Genres genres = new Genres();
                    genres.Id = reader.GetInt64(0);
                    genres.Name = reader.GetString(1);
                    genreses.Add(genres);
                }
                return genreses;
            }
            catch
            {
                return new List<Genres>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<Genres> ReadAsync(Int64 id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM genreses WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                return new Genres()
                {
                    Id = reader.GetInt64(0),
                    Name = reader.GetString(1)
                };
            }
            catch
            {
                return new Genres();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateAsync(Int64 id, Genres entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "UPDATE genreses SET name = @Name " +
                                $"WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("Name", entity.Name)
                    }
                };
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return true;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
    }
}
