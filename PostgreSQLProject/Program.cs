using Npgsql;
using System;
using System.Data;

namespace PostgreSQLProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InsertUsers(1000);
        }

        static void InsertUsers(int count)
        {
            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();
                for (int i = 0; i < count; i++)
                {
                    string query = $"insert into public.Users(FirstName, LastName, Email) values('A{i}', 'A{i}yan', 'A{i}@gmail.com')";
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
