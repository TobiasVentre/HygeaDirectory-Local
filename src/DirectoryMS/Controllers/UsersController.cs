using DirectoryMS.Application.Dtos;
using DirectoryMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryMS.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly IUserUseCase _userUseCase;

    public UsersController(IUserUseCase userUseCase)
    {
        _userUseCase = userUseCase;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<UserResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _userUseCase.GetUsersAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userUseCase.GetUserByIdAsync(id, cancellationToken);
        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request, CancellationToken cancellationToken)
    {
        var createdUser = await _userUseCase.CreateUserAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _userUseCase.UpdateUserAsync(id, request, cancellationToken);
        return Ok(user);
    }
}
