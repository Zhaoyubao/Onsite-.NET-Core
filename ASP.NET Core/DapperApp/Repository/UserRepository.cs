using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using DapperApp.Models;
using Microsoft.Extensions.Options;

namespace DapperApp.Repository {
    public class UserRepository : IRepository<User> {
        private readonly IOptions<MySqlOptions> mysqlConfig;
        public UserRepository(IOptions<MySqlOptions> conf) {
            mysqlConfig = conf;
        }
        internal IDbConnection Connection {
            get {
                return new MySqlConnection(mysqlConfig.Value.ConnectionString);
            }
        }
        public void Add(User item){
            using (IDbConnection dbConnection = Connection) {
                string query = "INSERT INTO users (first_name, last_name, created_at, updated_at) VALUES (@first_name, @last_name, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, item);
            }
        }
        public IEnumerable<User> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM users");
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
    }
}