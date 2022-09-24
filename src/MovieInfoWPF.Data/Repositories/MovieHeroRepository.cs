using MovieInfo.Data.Interfaces;
using MovieInto.Domain.Configurations;
using MovieInto.Domain.Constants;
using MovieInto.Domain.Entities;
using Npgsql;

namespace MovieInfo.Data.Repositories
{
    public class MovieHeroRepository : IMovieHeroRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DatabaseConstants.CONNECTION_STRING);
        public async Task<bool> CreateAsync(MovieHero entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO movie_hero(actors_id, movie_id, hero_name) VALUES(@ActorId, @MovieId, @HeroName)";
                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("ActorId", entity.ActorId),
                        new("MovieId", entity.MovieId),
                        new("HeroName", entity.HeroName),
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
                string query = $"DELETE FROM movie_hero WHERE id = {id}";
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

        public async Task<IEnumerable<MovieHero>> ReadAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM movie_hero ORDER BY id ASC OFFSET {@params.SkipCount} LIMIT {@params.PageSize}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();

                ICollection<MovieHero> movieHeroes = new List<MovieHero>();
                while (await reader.ReadAsync())
                {
                    MovieHero movieHero = new MovieHero();
                    movieHero.Id = reader.GetInt64(0);
                    movieHero.ActorId = reader.GetInt64(1);
                    movieHero.MovieId = reader.GetInt64(2);
                    movieHero.HeroName = reader.GetString(3);
                    movieHeroes.Add(movieHero);
                }
                return movieHeroes;
            }
            catch
            {
                return new List<MovieHero>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<MovieHero> ReadAsync(Int64 id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM movie_hero WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                return new MovieHero()
                {
                    Id = reader.GetInt64(0),
                    ActorId = reader.GetInt64(1),
                    MovieId = reader.GetInt64(2),
                    HeroName = reader.GetString(3),
                };
            }
            catch
            {
                return new MovieHero();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> UpdateAsync(Int64 id, MovieHero entity)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"UPDATE movie_hero SET actors_id = @ActorId, movie_id = @MovieId, " +
                               $"hero_name = @HeroName WHERE id = {id}";
                var command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("ActorId", entity.ActorId),
                        new("MovieId", entity.MovieId),
                        new("HeroName", entity.HeroName),
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
