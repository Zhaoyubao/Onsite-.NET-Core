using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using Wall.Models;

namespace Wall.Factory
{
    public class CommentFactory : IFactory<Comment>
    {
        private readonly IOptions<MySqlOptions> mysqlConfig;
        public CommentFactory(IOptions<MySqlOptions> conf) {
            mysqlConfig = conf;
        }
        internal IDbConnection Connection => new MySqlConnection(mysqlConfig.Value.ConnectionString);

        public void Add(Comment com, int userID, int msgID)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  $"INSERT INTO comments (content, user_id, message_id, created_at, updated_at) VALUES (@content, {userID}, {msgID}, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, com);
            }
        }
        public IEnumerable<Comment> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "SELECT * FROM comments JOIN users ON comments.user_id = users.id ORDER BY comments.created_at";
                dbConnection.Open();
                return dbConnection.Query<Comment, User, Comment>(query, (com, user) => { com.Author = user; return com; });
            }
        }
        public Comment FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Comment>("SELECT * FROM comments WHERE id = @Id", new { Id = id }).FirstOrDefault();
            }
        }
          public void DelByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "DELETE FROM comments WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Execute(query, new { Id = id });
            }
        }
    }
}