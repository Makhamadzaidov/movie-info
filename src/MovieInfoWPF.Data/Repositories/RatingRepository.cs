using MovieInfo.Data.Interfaces;
using MovieInto.Domain.Configurations;
using MovieInto.Domain.Constants;
using MovieInto.Domain.Entities;
using Npgsql;

namespace MovieInfo.Data.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);
        public async Task<bool> CreateAsync(Rating entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO ratings(movie_id, viewer_id, comment, viewer_ball) " +
                               "VALUES(@MovieId, @ViewerId, @Comment, @ViewerBall)";

                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("MovieId", entity.MovieId),
                        new("ViewerId", entity.ViewerId),
                        new("Comment", entity.Comment),
                        new("ViewerBall", entity.ViewerBall),
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
                string query = $"DELETE FROM ratings WHERE id = {id}";
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

        public async Task<IEnumerable<Rating>> ReadAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM ratings ORDER BY id ASC OFFSET {@params.SkipCount} LIMIT {@params.PageSize}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();

                ICollection<Rating> ratings = new List<Rating>();
                while (await reader.ReadAsync())
                {
                    Rating rating = new Rating();
                    rating.Id = reader.GetInt64(0);
                    rating.MovieId = reader.GetInt64(1);
                    rating.ViewerId = reader.GetInt64(2);
                    rating.Comment = reader.GetString(3);
                    rating.ViewerBall = reader.GetFloat(4);
                    rating.CreatedDate = reader.GetDateTime(5);
                    ratings.Add(rating);
                }
                return ratings;
            }
            catch
            {
                return new List<Rating>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<Rating> ReadAsync(Int64 id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM ratings WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                Rating rating = new Rating();
                rating.Id = reader.GetInt64(0);
                rating.MovieId = reader.GetInt64(1);
                rating.ViewerId = reader.GetInt64(2);
                rating.Comment = reader.GetString(3);
                rating.ViewerBall = reader.GetFloat(4);
                rating.CreatedDate = reader.GetDateTime(5);

                return rating;

            }
            catch
            {
                return new Rating();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateAsync(Int64 id, Rating entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "UPDATE ratings " +
                               "SET movie_id = @MovieId," +
                               "viewer_id = @ViewerId," +
                               "comment = @Comment," +
                               "viewer_ball = @ViewerBall " +
                               $"WHERE id = {id}";

                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("MovieId", entity.MovieId),
                        new("ViewerId", entity.ViewerId),
                        new("Comment", entity.Comment),
                        new("ViewerBall", entity.ViewerBall)
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
