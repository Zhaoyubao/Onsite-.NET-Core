using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using QuotingRedux.Models;

namespace QuotingRedux.Factory
{
    public class QuoteFactory : IFactory<Quote>
    {
        private readonly IOptions<MySqlOptions> mysqlConfig;
        public QuoteFactory(IOptions<MySqlOptions> conf) {
            mysqlConfig = conf;
        }
        internal IDbConnection Connection => new MySqlConnection(mysqlConfig.Value.ConnectionString);

        public void Add(Quote quote, int userID)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  $"INSERT INTO quotes (content, likes, user_id, created_at, updated_at) VALUES (@content, 0, {userID}, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, quote);
            }
        }
        public IEnumerable<Quote> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "SELECT * FROM quotes JOIN users ON quotes.user_id = users.id ORDER BY likes DESC";
                dbConnection.Open();
                return dbConnection.Query<Quote, User, Quote>(query, (quote, user) => { quote.Author = user; return quote; });
            }
        }
        public Quote FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Quote>("SELECT * FROM quotes WHERE id = @Id", new { Id = id }).FirstOrDefault();
            }
        }
        public void UpdateByID(int id, int likes)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "UPDATE quotes SET likes = @Likes WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Execute(query, new { Likes = likes, Id = id });
            }
        }
        public void DelByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "DELETE FROM quotes WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Execute(query, new { Id = id });
            }
        }
    }
}