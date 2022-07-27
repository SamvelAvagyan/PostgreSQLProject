using Npgsql;
using System;

namespace PostgreSQLProject
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }

        static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=root1234;Database=myDB;");
        }
    }
}
