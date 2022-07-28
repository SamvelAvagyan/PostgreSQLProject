using Npgsql;
using System;
using System.Data;

namespace PostgreSQLProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //InsertUsers(1000);

        }

        static void InsertUsers(int count)
        {
            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();
                Random rnd = new Random();
                for (int i = 0; i < count; i++)
                {
                    User user = new User($"A{i}", $"+374{rnd.Next(90000000, 99999999)}", DateTime.Now);
                    AddUser(user);
                }
            }
        }

        static void AddUser(User user)
        {
            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();
                string query = $"insert into public.Users(Name, Number, Date) values('{user.Name}', '{user.Number}', '{user.Date}')";
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                command.ExecuteNonQuery();
            }
        }

        static void ReadData()
        {

        }

        static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=root1234;Database=myDB");
        }
    }
}
