using Microsoft.AspNetCore.Mvc;
using System;

namespace PostgreSQLProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _dbContext;

        public UserController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("Count")]
        public IActionResult AddUsers(int count)
        {
            for(int i = 0; i < count; i++)
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
    }
}
