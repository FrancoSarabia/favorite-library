
    using FavoriteLibrary.Dtos;
    using FavoriteLibrary.Services;
    using Microsoft.AspNetCore.Mvc;

    namespace FavoriteLibrary.Controllers
    {
        [ApiController]
        [Route("api/users")]
        public class UserController : ControllerBase
        {
            private readonly IUserService _userService;

            public UserController(IUserService userService)
            {
                _userService = userService;
            }

            // POST: api/users
            [HttpPost]
            public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                try
                {
                    var user = await _userService.AddUserAsync(dto);

                    return CreatedAtAction(
                        nameof(GetUserById),
                        new { id = user.Id },
                        user
                    );
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpGet("{id:guid}")]
            public async Task<IActionResult> GetUserById(Guid id)
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
        }
    }
