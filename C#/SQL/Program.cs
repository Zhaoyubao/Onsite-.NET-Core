using System;
using static System.Console;
using System.Collections.Generic;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace ConsoleWithDb
{
    public class DbConnector
    {
        public static List<Dictionary<string, object>> ExecuteQuery(string queryString)
        {
            string server = "localhost";
            string db = "consoleDB";
            string port = "8889";
            string user = "root";
            string pass = "root";
            using(var connection = new MySqlConnection(
                $"Server={server};Database={db};Port={port};UserID={user};Password={pass};"))
            {
                connection.Open();
                using(var command = new MySqlCommand(queryString, connection))
                {
                    var result = new List<Dictionary<string, object>>();
                    using(DbDataReader rdr = command.ExecuteReader())
                    {
                        while(rdr.Read())
                        {
                            var dict = new Dictionary<string, object>();
                            for( int i = 0; i < rdr.FieldCount; i++ ) {
                                dict.Add(rdr.GetName(i), rdr.GetValue(i));
                            }
                            result.Add(dict);
                        }
                    }
                    return result;
                }
            }
        }
    }
    class Program
    {
        static void Main()
        {
            ShowAll();
            string input = "a";
            while(input != "Q")
            {
                WriteLine("Choose an operation:");
                WriteLine("1. Insert");
                WriteLine("2. Update");
                WriteLine("3. Delete");
                Write("Your Choice(press q to exit): ");
                input = ReadKey().Key.ToString();
                WriteLine();
                switch (input)
                {
                    case "D1": 
                        Insert();
                        break;
                    case "D2":
                        Update();
                        break;
                    case "D3":
                        Delete();
                        break;
                }
            }
        }
            static void Insert()
            {
                Write("First Name: ");
                string fname = ReadLine();
                Write("Last Name: ");
                string lname = ReadLine();
                while(true)
                {
                    Write("Favorite Number: ");
                    try {
                        int num = int.Parse(ReadLine());
                        DbConnector.ExecuteQuery($"INSERT INTO users (first_name, last_name, favorite_number, created_at, update_at) VALUES ('{fname}', '{lname}', {num}, NOW(), NOW())");
                        ShowAll();
                        break;
                    }
                    catch (FormatException) {
                        WriteLine("Invalid Number!");
                        continue;
                    }
                }
            }
            static void Update()
            {
                WriteLine("Which one do you want to update?");
                while(true)
                {
                    Write("Input ID: ");
                    try {
                        int id = int.Parse(ReadLine());
                        if (GetUser(id)) 
                        {
                            Write("Update First Name: ");
                            string fname = ReadLine();
                            Write("Update Last Name: ");
                            string lname = ReadLine();
                            while(true)
                            {
                                Write("Update Favorite Number: ");
                                try {
                                    int num = int.Parse(ReadLine());
                                    DbConnector.ExecuteQuery($"UPDATE users SET first_name=\"{fname}\", last_name=\"{lname}\", favorite_number={num},  update_at=NOW() WHERE id={id}");
                                    ShowAll();
                                    break;
                                }
                                catch (FormatException) {
                                    WriteLine("Invalid Number!");
                                    continue;
                                }
                            }
                            break;
                        }
                        WriteLine("Wrong ID!");
                    }
                    catch (FormatException) {
                        WriteLine("Invalid ID!");
                        continue;
                    }
                }
            }
            static void Delete()
            {
                WriteLine("Which one do you want to delete?");
                while(true)
                {
                    Write("Input ID: ");
                    try {
                        int id = int.Parse(ReadLine());
                        if (GetUser(id)) 
                        {
                            DbConnector.ExecuteQuery($"DELETE FROM users WHERE id={id}");
                            ShowAll();
                            break;
                        }
                        WriteLine("Wrong ID!");
                    }
                    catch (FormatException) {
                        WriteLine("Invalid ID!");
                        continue;
                    }
                }
            }
            static void ShowAll()
            {
                var users = DbConnector.ExecuteQuery("SELECT id, first_name, last_name, favorite_number FROM users");
                foreach (var user in users)
                {   
                    foreach (var kvp in user)
                        WriteLine($"{kvp.Key,-15}: {kvp.Value}");
                    WriteLine("-".PadLeft(30, '-')); 
                }
            }
            static bool GetUser(int id)
            {
                var users = DbConnector.ExecuteQuery($"SELECT * FROM users WHERE id={id}");
                if (users.Count == 0)
                    return false;
                else 
                    return true;
            }
    }
}
