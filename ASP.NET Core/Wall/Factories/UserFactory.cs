using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using Wall.Models;

namespace Wall.Factory
{
    public class UserFactory : IFactory<User>
    {
        private readonly IOptions<MySqlOptions> mysqlConfig;
        public UserFactory(IOptions<MySqlOptions> conf) {
            mysqlConfig = conf;
        }
        internal IDbConnection Connection => new MySqlConnection(mysqlConfig.Value.ConnectionString);

        public void Add(User NewUser)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  "INSERT INTO users (firstname, lastname, email, password, created_at, updated_at) VALUES (@firstname, @lastname, @email, @password, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, NewUser);
            }
        }
        
        public User FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM users WHERE id = @Id", new { Id = id }).FirstOrDefault();
            }
        }

        public User FindByEmail(string email)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM users WHERE email = @Email", new { Email = email }).FirstOrDefault();
            }
        }
    }
}