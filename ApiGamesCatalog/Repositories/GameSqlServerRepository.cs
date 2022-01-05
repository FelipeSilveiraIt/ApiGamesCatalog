using ApiGamesCatalog.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGamesCatalog.Repositories
{
    public class GameSqlServerRepository : IGameRepository
    {
        private readonly SqlConnection sqlConnection;

        public GameSqlServerRepository(IConfiguration configuration) //gets configs from appsettings.json
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Default")); //must add attribute ConnectionStrings in appsettings.json
        }

        public async Task<List<Game>> Get(int page, int amount)
        {
            var games = new List<Game>();

            var comamnd = $"select * from Games order by id offset {((page - 1) * amount)} rows fetch next {amount} rows only";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comamnd, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                games.Add(new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Publisher = (string)sqlDataReader["Publisher"],
                    Price = (double)sqlDataReader["Price"]
                });
            }

            await sqlConnection.CloseAsync();

            return games;
        }

        public async Task<Game> Get(Guid id)
        {
            Game game = null;

            var comamnd = $"select * from Games where Id = '{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comamnd, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                game = new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Publisher = (string)sqlDataReader["Publisher"],
                    Price = (double)sqlDataReader["Price"]
                };
            }

            await sqlConnection.CloseAsync();

            return game;
        }

        public async Task<List<Game>> Get(string name, string publisher)
        {
            var games = new List<Game>();

            var command = $"select * from Games where Name = '{name}' and Publisher = '{publisher}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                games.Add(new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Publisher = (string)sqlDataReader["Publisher"],
                    Price = (double)sqlDataReader["Price"]
                });
            }

            await sqlConnection.CloseAsync();

            return games;
        }

        public async Task Post(Game game)
        {
            var command = $"insert Games (Id, Name, Publisher, Price) values ('{game.Id}', '{game.Name}', '{game.Publisher}', {game.Price.ToString().Replace(",", ".")})";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Put(Game game)
        {
            var command = $"update Games set Name = '{game.Name}', Publisher = '{game.Publisher}', Price = {game.Price.ToString().Replace(",", ".")} where Id = '{game.Id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Delete(Guid id)
        {
            var command = $"delete from Games where Id = '{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }
    }
}
