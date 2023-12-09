using Data;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todo.Models;
using Repositories;
using Template;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "admin,manager")]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserRepository _repository;
    private readonly LogFormater _log;

    public UsersController(ILogger<UsersController> logger, IConfiguration configuration, TRepository<User, ApplicationDbContext> repository)
    {
        _configuration = configuration;
        _repository = (UserRepository)repository;
        _log = new LogFormater(logger);
    }

    public class UserDTO
    {
        public string account { get; set; } = "";
        public string password { get; set; } = "";

        public string role { get; set; } = "";
    }

    [HttpGet]
    public IQueryable<User> Get()
    {
        return _repository.GetAll();
    }

    // GET: api/[controller]/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User?>> Get(string id)
    {
        return await _repository.Get(id).FirstOrDefaultAsync();
    }

    [HttpPost]
    public async Task<ActionResult> Post(User obj)
    {
        if (string.IsNullOrEmpty(obj.Id))
        {
            return BadRequest("Id is empty!");
        }
        var addedUser = await _repository.Add(obj);
        return CreatedAtAction("Get", new { id = obj.Id }, addedUser);
    }

    // PUT: api/[controller]/id
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(string id, User obj)
    {
        if (id != obj.Id)
        {
            return NotFound();
        }
        await _repository.Update(obj);
        return Ok(obj);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(string id, User obj)
    {
        obj.Id = null;
        var found = _repository.Get(id).FirstOrDefault();
        if (found != null)
        {
            var pros = obj.GetType().GetProperties();
            foreach (var prop in pros)
            {
                if (prop.CanRead)
                {
                    var val = prop.GetValue(obj);
                    if (val != null)
                    {
                        prop.SetValue(found, val);
                    }
                }
            }
            await _repository.Update(found);
            return Ok(found);
        }
        return NotFound("Invalid id!");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var found = await _repository.Delete(id);
        if (found != null)
        {
            return Ok(found);
        }
        return NotFound("Invalid id!");
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public ActionResult LoginWithUserPassword(UserDTO user)
    {
        try
        {
            string token = _repository.Login(user.account, user.password);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            _log.Error(ex.Message, ex);
            return NotFound(ex.Message);
        }
    }

}
