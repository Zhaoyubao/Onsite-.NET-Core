using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using Wall.Models;

namespace Wall.Factory
{
    public class MessageFactory : IFactory<Message>
    {
        private readonly IOptions<MySqlOptions> mysqlConfig;
        public MessageFactory(IOptions<MySqlOptions> conf) {
            mysqlConfig = conf;
        }
        internal IDbConnection Connection => new MySqlConnection(mysqlConfig.Value.ConnectionString);

        public void Add(Message msg, int userID)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  $"INSERT INTO messages (content, user_id, created_at, updated_at) VALUES (@content, {userID}, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, msg);
            }
        }
        public IEnumerable<Message> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "SELECT * FROM messages JOIN users ON messages.user_id = users.id ORDER BY messages.created_at DESC";
                dbConnection.Open();
                return dbConnection.Query<Message, User, Message>(query, (msg, user) => { msg.Author = user; return msg; });
            }
        }
        public Message FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Message>("SELECT * FROM messages WHERE id = @Id", new { Id = id }).FirstOrDefault();
            }
        }
        public void DelByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "DELETE FROM messages WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Execute(query, new { Id = id });
            }
        }
        public void DelComsByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "DELETE FROM comments WHERE message_id = @Id";
                dbConnection.Open();
                dbConnection.Execute(query, new { Id = id });
            }
        }
    }
}