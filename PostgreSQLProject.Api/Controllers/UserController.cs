using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PostgreSQLProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _dbContext;
        private static List<User> users = new List<User>();

        public UserController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            NpgsqlConnection con = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=root1234;Database=myDB");
            con.Open();
            string query = "SELECT * FROM public.\"Users\"";
            using (var transaction = await con.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            using (var command = new NpgsqlCommand(query, con, transaction))
            {
                try
                {
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                Id = Int32.Parse(reader[0].ToString()),
                                Name = (string)reader[1].ToString(),
                            };
                            users.Add(user);
                            Console.WriteLine(user.Id + " " + user.Name);
                        }
                    }

                    await transaction.CommitAsync();
                    Console.WriteLine($"{Thread.CurrentThread.Name} commit");
                }
                catch (NpgsqlException)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"{Thread.CurrentThread.Name} rollback");
                }
                finally
                {
                    con.Close();
                }
            }
            return Ok(users);
        }

        [HttpPost("{count}")]
        public IActionResult AddUsers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                User user = new User()
                {
                    Name = $"A{i}",
                    Number = "+37477665589",
                    Date = DateTime.Now
                };
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id)
        {
            //_dbContext.Users.Update(user);
            //_dbContext.SaveChanges();
            await UpdateData(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> TestIsolationLevel(int id, int id1)
        {
            List<User> user = new List<User>();
            Thread thread = new Thread(() => UpdateData(id));
            Thread thread1 = new Thread(async () => user = await ReadData());

            thread.Start();
            thread1.Start();
            return Ok(user);
        }

        [NonAction]
        public async Task<List<User>> ReadData()
        {
            //using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted);

            //try
            //{
            //    users = _dbContext.Users.ToList();
            //    transaction.Commit();
            //}
            //catch (Exception)
            //{
            //    transaction.Rollback();
            //}
            NpgsqlConnection con = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=root1234;Database=myDB");
            con.Open();
            string query = "SELECT * FROM public.\"Users\"";
            using (var transaction = await con.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            using (var command = new NpgsqlCommand(query, con, transaction))
            {
                try
                {
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            User user = new User
                            {
                                Id = Int32.Parse(reader[0].ToString()),
                                Name = (string)reader[1].ToString(),
                            };
                            users.Add(user);
                            Console.WriteLine(user.Id + " " + user.Name);
                        }
                    }

                    await transaction.CommitAsync();
                    Console.WriteLine($"{Thread.CurrentThread.Name} commit");
                }
                catch (NpgsqlException)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"{Thread.CurrentThread.Name} rollback");
                }
                finally
                {
                    con.Close();
                }
                return users;
            }
        }

        //[NonAction]
        //public User ReadUser(NpgsqlDataReader reader)
        //{
        //    int? id = reader["Id"] as int?;
        //    string name = reader["Name"] as string;
        //    string number = reader["Number"] as string;
        //    DateTime date = (DateTime)reader["Date"];

        //    User user = new User
        //    {
        //        Id = (int)id,
        //        Name = name,
        //        Number = number,
        //        Date = date
        //    };

        //    return user;
        //}


        [NonAction]
        public async Task UpdateData(int id)
        {
            NpgsqlConnection con = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=root1234;Database=myDB");
            con.Open();
            string query = $"UPDATE public.\"Users\" SET \"Name\" = 'A50' WHERE \"Id\" = {id}";
            using (var transaction = await con.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            using (var command = new NpgsqlCommand(query, con, transaction))
            {
                try
                {
                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                    Console.WriteLine($"{Thread.CurrentThread.Name}Update commit");
                }
                catch (NpgsqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    await transaction.RollbackAsync();
                    Console.WriteLine($"{Thread.CurrentThread.Name}Update rollback");
                }
                finally
                {
                    con.Close();
                }
            }
        }

        [NonAction]
        public async Task UpdateData1(int id)
        {
            NpgsqlConnection con = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=root1234;Database=myDB");
            con.Open();
            string query = $"UPDATE public.\"Users\" SET \"Name\" = 'A51' WHERE \"Id\" = {id}";
            using (var transaction = await con.BeginTransactionAsync(IsolationLevel.Serializable))
            using (var command = new NpgsqlCommand(query, con, transaction))
            {
                try
                {
                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                    Console.WriteLine($"{Thread.CurrentThread.Name}Update commit");
                }
                catch (NpgsqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    await transaction.RollbackAsync();
                    Console.WriteLine($"{Thread.CurrentThread.Name}Update rollback");
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
