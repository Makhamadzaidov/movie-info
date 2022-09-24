using MovieInfo.Data.Interfaces;
using MovieInto.Domain.Configurations;
using MovieInto.Domain.Constants;
using MovieInto.Domain.Entities;
using Npgsql;

namespace MovieInfo.Data.Repositories
{
    public class ViewerRepository : IViewerRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);

        public async Task<bool> CreateAsync(Viewer entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO viewers(name) VALUES(@Name)";

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
                string query = $"DELETE FROM viewers WHERE id = {id}";
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

        public async Task<IEnumerable<Viewer>> ReadAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM viewers ORDER BY id ASC OFFSET {@params.SkipCount} LIMIT {@params.PageSize}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();

                ICollection<Viewer> viewers = new List<Viewer>();
                while (await reader.ReadAsync())
                {
                    Viewer viewer = new Viewer();
                    viewer.Id = reader.GetInt64(0);
                    viewer.Name = reader.GetString(1);
                    viewers.Add(viewer);
                }
                return viewers;
            }
            catch
            {
                return new List<Viewer>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<Viewer> ReadAsync(Int64 id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM viewers WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                return new Viewer()
                {
                    Id = reader.GetInt64(0),
                    Name = reader.GetString(1)
                };
            }
            catch
            {
                return new Viewer();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateAsync(Int64 id, Viewer entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "UPDATE viewers SET name = @Name " +
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
