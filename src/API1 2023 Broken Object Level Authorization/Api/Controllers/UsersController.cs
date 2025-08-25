using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static readonly List<User> Users = new()
    {
        new User(1, "Alice", "alice@example.com", "https://via.placeholder.com/150"),
        new User(2, "Bob", "bob@example.com", "https://via.placeholder.com/150")
    };

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        if (user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}

public record User(int Id, string Name, string Email, string ImageUrl);
