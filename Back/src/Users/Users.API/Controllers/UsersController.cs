using Microsoft.AspNetCore.Mvc;
using Users.ApplicationServices.Users;
using Users.Core.Users;
using Users.Users.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersAppService _usersAppServices;

        public UsersController(IUsersAppService usersAppServices)
        {
            _usersAppServices = usersAppServices;
        }
        
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IResult> Get()
        {
            IQueryable<UserDto> users = await _usersAppServices.GetAllUsersAsync();
            return Results.Ok(users);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IResult> Get(string id)
        {
            UserDto user = await _usersAppServices.FindUserByIdAsync(id);
            if (user == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task PostAsync([FromBody] CreateUserDto userDto)
        {
            await _usersAppServices.CreateUserAsync(userDto);
        }

        /*
        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
