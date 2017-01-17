using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using quotingDojo.Models;

namespace quotingDojo.Factory
{
    public class QuoteFactory : IFactory<Quotes>
    {
        private string connectionString;
        public QuoteFactory()
        {
            connectionString = "server=localhost;userid=root;password=root;port=8889;database=quotesdb;SslMode=None";
        }
        internal IDbConnection Connection => new MySqlConnection(connectionString);
        
        public void Add(Quotes item)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  "INSERT INTO quotes (content, author, likes, created_at, updated_at) VALUES (@Content, @Author, 0, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, item);
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
        public IEnumerable<Quotes> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Quotes>("SELECT id, content, likes, DATE_FORMAT(created_at, '%l:%i%p %M %e %Y') AS DATE FROM quotes ORDER BY likes DESC");
            }
        }
        public Quotes FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Quotes>("SELECT * FROM quotes WHERE id = @Id", new { Id = id }).FirstOrDefault();
            }
        }
        public void UpdateByID(int id, int likes)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "UPDATE qutoes SET likes = @Likes WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Execute(query, new { Likes = likes, Id = id });
            }
        }
    }
}