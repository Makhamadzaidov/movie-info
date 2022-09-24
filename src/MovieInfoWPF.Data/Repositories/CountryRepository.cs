using MovieInfo.Data.Interfaces;
using MovieInto.Domain.Configurations;
using MovieInto.Domain.Constants;
using MovieInto.Domain.Entities;
using Npgsql;

namespace MovieInfo.Data.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);
        
        public async Task<bool> CreateAsync(Country entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO countries(name) VALUES(@Name)";
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
                string query = $"DELETE FROM countries WHERE id = {id}";
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

        public async Task<IEnumerable<Country>> ReadAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM countries ORDER BY id ASC OFFSET {@params.SkipCount} LIMIT {@params.PageSize}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();

                ICollection<Country> countries = new List<Country>();
                while (await reader.ReadAsync())
                {
                    Country country = new Country();
                    country.Id = reader.GetInt64(0);
                    country.Name = reader.GetString(1);
                    countries.Add(country);
                }
                return countries;
            }
            catch
            {
                return new List<Country>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<Country> ReadAsync(Int64 id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM countries WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                return new Country()
                {
                    Id = reader.GetInt64(0),
                    Name = reader.GetString(1)
                };
            }
            catch
            {
                return new Country();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateAsync(Int64 id, Country entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"UPDATE countries SET name = @Name WHERE id = {id}";
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
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
    }
}
