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
                    string query = $"insert into public.Users(Name, Number, Date) values('A{i}', '+374{rnd.Next(90000000, 99999999)}', '{DateTime.Now}')";
                    NpgsqlCommand command = new NpgsqlCommand(query, con);
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
