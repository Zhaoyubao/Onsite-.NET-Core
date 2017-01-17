using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using Notes.Models;

namespace Notes.Factory
{
    public class NoteFactory : IFactory<Note>
    {
        private readonly IOptions<MySqlOptions> mysqlConfig;
        public NoteFactory(IOptions<MySqlOptions> conf) {
            mysqlConfig = conf;
        }
        internal IDbConnection Connection => new MySqlConnection(mysqlConfig.Value.ConnectionString);

        public void Add(Note note)
        {
            using (IDbConnection dbConnection = Connection) 
            {
                string query =  "INSERT INTO notes (title, description, created_at, updated_at) VALUES (@title, '', NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, note);
            }
        }
        
        public IEnumerable<Note> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Note>("SELECT * FROM notes ORDER BY id DESC");
            }
        }

        public Note FindById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Note>("SELECT * FROM notes WHERE id=@Id", new { Id = id }).FirstOrDefault();
            }
        }
        public void UpdateByID(int id, Note note)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"UPDATE notes SET title = @Title, description = @Description, created_at=NOW(), updated_at=NOW() WHERE id = {id}";
                dbConnection.Open();
                dbConnection.Execute(query, note);
            }
        }

        public void DelByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "DELETE FROM notes WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Execute(query, new { Id = id });
            }
        }
    }
}