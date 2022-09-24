using MovieInfo.Data.Interfaces;
using MovieInto.Domain.Configurations;
using MovieInto.Domain.Constants;
using MovieInto.Domain.Entities;
using Npgsql;

namespace MovieInfo.Data.Repositories
{
    #pragma warning disable
    public class DirectorRepository : IDirectorRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);

        public async Task<bool> CreateAsync(Director entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO directors(first_name, last_name, is_male, birth_date, position, hobby) " +
                                "VALUES(@FirstName, @LastName, @Gender, @BirthDate, @Position, @Hobby)";
                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("FirstName", entity.FirstName),
                        new("LastName", entity.LastName),
                        new("Gender", entity.Gender),
                        new("BirthDate", entity.BirthDate),
                        new("Position", entity.Position),
                        new("Hobby", entity.Hobby)
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
                string query = $"DELETE FROM directors WHERE id = {id}";
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

        public async Task<IEnumerable<Director>> ReadAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM directors ORDER BY ID ASC OFFSET {@params.SkipCount} LIMIT {@params.PageSize}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();

                ICollection<Director> directors = new List<Director>();
                while (await reader.ReadAsync())
                {
                    Director director = new Director();
                    director.Id = reader.GetInt64(0);
                    director.FirstName = reader.GetString(1);
                    director.LastName = reader.GetString(2);
                    director.Gender = reader.GetBoolean(3);
                    director.BirthDate = DateOnly.Parse($"{reader.GetDate(4)}");
                    director.Position = reader.GetString(5);
                    director.Hobby = reader.GetString(6);
                    director.CreatedDate = reader.GetDateTime(7);

                    if (!reader.IsDBNull(8))
                        director.UpdatedDate = reader.GetDateTime(8);
                    directors.Add(director);
                }
                return directors;
            }
            catch
            {
                return new List<Director>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<Director> ReadAsync(Int64 id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM directors WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                Director director = new Director();
                director.Id = reader.GetInt64(0);
                director.FirstName = reader.GetString(1);
                director.LastName = reader.GetString(2);
                director.Gender = reader.GetBoolean(3);
                director.BirthDate = DateOnly.Parse($"{reader.GetDate(4)}");
                director.Position = reader.GetString(5);
                director.Hobby = reader.GetString(6);
                director.CreatedDate = reader.GetDateTime(7);

                if (!reader.IsDBNull(8))
                    director.UpdatedDate = reader.GetDateTime(8);
                return director;
            }
            catch
            {
                return new Director();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateAsync(Int64 id, Director entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "UPDATE directors " +
                               "SET first_name = @FirstName," +
                               "last_name = @LastName," +
                               "is_male = @Gender," +
                               "birth_date = @BirthDate," +
                               "position = @Position," +
                               "hobby = @Hobby," +
                               "updated_date = @UpdatedDate " +
                               $"WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("FirstName", entity.FirstName),
                        new("LastName", entity.LastName),
                        new("Gender", entity.Gender),
                        new("BirthDate", entity.BirthDate),
                        new("Position", entity.Position),
                        new("Hobby", entity.Hobby),
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
