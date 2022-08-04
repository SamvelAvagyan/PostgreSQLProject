using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace PostgreSQLProject
{
    internal class Program
    {
        private static List<User> users;
        private static UserDbContext dbContext = new UserDbContext();

        static void Main(string[] args)
        {
            InsertUsers(1000);
            //UpdateUser(1, new User("A0", "+37498264752", DateTime.Now));
            //List<User> users = ReadData();
            //Thread thread = new Thread(FirstQuery);
            //Thread thread1 = new Thread(SecondQuery);
            //thread.Start();
            //thread1.Start();
        }

        static void FirstQuery()
        {
            users = ReadData();
            for (int i = 0; i < users.Count; i++)
            {
                Console.Write(users[i].Name + ", ");
            }
        }

        static void SecondQuery()
        {
            UpdateUser(12, new User("A30", "+37456251836", DateTime.Now));
        }

        static void InsertUsers(int count)
        {
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                User user = new User($"A{i}", $"+374{rnd.Next(90000000, 99999999)}", DateTime.Now);
                dbContext.Users.Add(user);
            }
        }

        static List<User> ReadData()
        {
            return dbContext.Users.ToList();
        }

        static User ReadUser(NpgsqlDataReader reader)
        {
            int? id = reader["Id"] as int?;
            string name = reader["Name"] as string;
            string number = reader["Number"] as string;
            DateTime date = (DateTime)reader["Date"];

            User user = new User
            {
                Id = (int)id,
                Name = name,
                Number = number,
                Date = date
            };

            return user;
        }

        static void UpdateUser(int id, User user)
        {
            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();
                string query = $"update public.Users set Name = @name, Number = @number, Date = @date where Id = @id";
                using (NpgsqlCommand command = new NpgsqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("name", user.Name);
                    command.Parameters.AddWithValue("number", user.Number);
                    command.Parameters.AddWithValue("date", user.Date);
                    command.ExecuteNonQuery();
                }
            }
        }

        static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=root1234;Database=myDB");
        }
    }
}
