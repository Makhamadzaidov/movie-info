using MovieInfo.Data.Interfaces;
using MovieInto.Domain.Configurations;
using MovieInto.Domain.Constants;
using MovieInto.Domain.Entities;
using Npgsql;

namespace MovieInfo.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);

        public async Task<bool> CreateAsync(User entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"INSERT INTO users(first_name, last_name, is_male, " +
                               $"birth_date, email, phone_number, login, password)" +
                               $"VALUES(@FirstName, @LastName, @Gender, @BirthDate, @Email, @PhoneNumber, @Login, @Password)";

                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("FirstName", entity.FirstName),
                        new("LastName", entity.LastName),
                        new("Gender", entity.Gender),
                        new("BirthDate", entity.BirthDate),
                        new("Email", entity.Email),
                        new("PhoneNumber", entity.PhoneNumber),
                        new("Login", entity.Login),
                        new("Password", entity.Password),
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
                string qury = $"DELETE FROM users WHERE id = {id}";
                var command = new NpgsqlCommand(qury, _connection);
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

        public async Task<IEnumerable<User>> ReadAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM users ORDER BY id ASC OFFSET {@params.SkipCount} LIMIT {@params.PageSize}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();

                ICollection<User> users = new List<User>();
                while (await reader.ReadAsync())
                {
                    User user = new User();
                    user.Id = reader.GetInt64(0);
                    user.FirstName = reader.GetString(1);
                    user.LastName = reader.GetString(2);
                    user.Gender = reader.GetBoolean(3);
                    user.BirthDate = DateOnly.Parse($"{reader.GetDate(4)}");
                    user.Email = reader.GetString(5);
                    user.PhoneNumber = reader.GetString(6);
                    user.Login = reader.GetString(7);
                    user.Password = reader.GetString(8);
                    user.CreatedDate = reader.GetDateTime(9);

                    if (!reader.IsDBNull(10))
                        user.UpdatedDate = reader.GetDateTime(10);
                    users.Add(user);
                }
                return users;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<User>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<User> ReadAsync(Int64 id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM users WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                User user = new User();
                user.Id = reader.GetInt64(0);
                user.FirstName = reader.GetString(1);
                user.LastName = reader.GetString(2);
                user.Gender = reader.GetBoolean(3);
                user.BirthDate = DateOnly.Parse($"{reader.GetDate(4)}");
                user.Email = reader.GetString(5);
                user.PhoneNumber = reader.GetString(6);
                user.Login = reader.GetString(7);
                user.Password = reader.GetString(8);
                user.CreatedDate = reader.GetDateTime(9);

                if (!reader.IsDBNull(10))
                    user.UpdatedDate = reader.GetDateTime(10);

                return user;
            }
            catch 
            {
                return new User();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateAsync(Int64 id, User entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "UPDATE users " +
                               "SET first_name = @FirstName," +
                               "last_name = @LastName," +
                               "is_male = @Gender," +
                               "birth_date = @BirthDate," +
                               "email = @Email," +
                               "phone_number = @PhoneNumber," +
                               "login = @Login," +
                               "password = @Password," +
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
                        new("Email", entity.Email),
                        new("PhoneNumber", entity.PhoneNumber),
                        new("Login", entity.Login),
                        new("Password", entity.Password),
                        new("UpdatedDate", entity.UpdatedDate = DateTime.Now)
                    }
                };
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
    }
}
