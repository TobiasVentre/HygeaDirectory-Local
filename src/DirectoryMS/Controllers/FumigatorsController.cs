using DirectoryMS.Application.Dtos;
using DirectoryMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryMS.Controllers;

[ApiController]
[Route("api/v1/fumigators")]
public class FumigatorsController : ControllerBase
{
    private readonly IUserUseCase _userUseCase;

    public FumigatorsController(IUserUseCase userUseCase)
    {
        _userUseCase = userUseCase;
    }

    [HttpPatch("{userId:guid}/approve")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ApproveFumigator(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userUseCase.ApproveFumigatorAsync(userId, cancellationToken);
        return Ok(user);
    }

    [HttpPatch("{userId:guid}/disable")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DisableFumigator(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userUseCase.DisableFumigatorAsync(userId, cancellationToken);
        return Ok(user);
    }
}
