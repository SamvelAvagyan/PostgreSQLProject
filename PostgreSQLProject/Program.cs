﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace PostgreSQLProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //InsertUsers(1000);
            List<User> users = ReadData();
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

        static List<User> ReadData()
        {
            List<User> users = new List<User>();
            using (NpgsqlConnection con = GetConnection())
            {
                string query = "select * from Users";
                con.Open();
                NpgsqlCommand command = new NpgsqlCommand(query, con);
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    User user = ReadUser(reader);
                    users.Add(user);
                }
            }

            return users; 
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

        static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=root1234;Database=myDB");
        }
    }
}
