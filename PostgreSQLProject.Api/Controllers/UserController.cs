using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace PostgreSQLProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _dbContext;
        private List<User> users;

        public UserController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_dbContext.Users);
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

        [HttpGet("{a}")]
        public IActionResult TestIsolationLevel(int a)
        {
            Thread thread = new Thread(ReadData);
            Thread thread1 = new Thread(UpdateData);

            thread.Start();
            thread1.Start();

            return Ok(users);
        }

        [NonAction]
        public void ReadData()
        {
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                users = _dbContext.Users.ToList();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }

        [NonAction]
        public void UpdateData()
        {
            using var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                _dbContext.Users.Update(new User() { Id = 1, Name = "A5000", Number = "342", Date = DateTime.Now });
                _dbContext.SaveChanges();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
    }
}
