using MovieInfo.Data.Interfaces;
using MovieInto.Domain.Configurations;
using MovieInto.Domain.Constants;
using MovieInto.Domain.Entities;
using Npgsql;

namespace MovieInfo.Data.Repositories
{
    #pragma warning disable
    public class ActorRepository : IActorRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);

        public async Task<bool> CreateAsync(Actor entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"INSERT INTO actors (first_name, last_name, is_male, birth_date, hobby) " +
                    $"VALUES(@FirstName, @LastName, @Gender, @BirthDate, @Hobby)";
                var command = new NpgsqlCommand(query, _connection) 
                {
                    Parameters = 
                    {
                        new("FirstName", entity.FirstName),
                        new("LastName", entity.LastName),
                        new("Gender", entity.Gender),
                        new("BirthDate", entity.BirthDate),
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
                string qury = $"DELETE FROM actors WHERE id = {id}";
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

        public async Task<IEnumerable<Actor>> ReadAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM actors ORDER BY id ASC OFFSET {@params.SkipCount} LIMIT {@params.PageSize}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                ICollection<Actor> actors = new List<Actor>();
                while (await reader.ReadAsync())
                {
                    Actor actor = new Actor();
                    actor.Id = reader.GetInt64(0);
                    actor.FirstName = reader.GetString(1);
                    actor.LastName = reader.GetString(2);
                    actor.Gender = reader.GetBoolean(3);
                    actor.BirthDate = DateOnly.Parse($"{reader.GetDate(4)}");
                    actor.Hobby = reader.GetString(5);
                    actor.CreatedDate = reader.GetDateTime(6);
                    
                    if (!reader.IsDBNull(7)) 
                        actor.UpdatedDate = reader.GetDateTime(7);
                    actors.Add(actor);
                }

                return actors;
            }
            catch
            {
                return new List<Actor>();
            }
            finally 
            { 
                await _connection.CloseAsync();
            }
        }

        public async Task<Actor> ReadAsync(Int64 id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM actors WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                Actor actor = new Actor();
                actor.Id = reader.GetInt64(0);
                actor.FirstName = reader.GetString(1);
                actor.LastName = reader.GetString(2);
                actor.Gender = reader.GetBoolean(3);
                actor.BirthDate = DateOnly.Parse($"{reader.GetDate(4)}");
                actor.Hobby = reader.GetString(5);
                actor.CreatedDate = reader.GetDateTime(6);

                if (!reader.IsDBNull(7))
                    actor.UpdatedDate = reader.GetDateTime(7);
                return actor;
            }
            catch
            {
                return new Actor();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateAsync(Int64 id, Actor entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "UPDATE actors " +
                               "SET first_name = @FirstName," +
                               "last_name = @LastName," +
                               "is_male = @Gender," +
                               "birth_date = @BirthDate," +
                               "hobby = @Hobby," +
                               $"updated_date = @UpdatedDate " +
                               $"WHERE id = {id}";

                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("FirstName", entity.FirstName),
                        new("LastName", entity.LastName),
                        new("Gender", entity.Gender),
                        new("BirthDate", entity.BirthDate),
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