using DirectoryMS.Application.Dtos;

namespace DirectoryMS.Application.Interfaces;

public interface IUserUseCase
{
    Task<IReadOnlyCollection<UserResponseDto>> GetUsersAsync(CancellationToken cancellationToken = default);
    Task<UserResponseDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default);
    Task<UserResponseDto> UpdateUserAsync(Guid id, UpdateUserRequestDto request, CancellationToken cancellationToken = default);
    Task<UserResponseDto> ApproveFumigatorAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserResponseDto> DisableFumigatorAsync(Guid userId, CancellationToken cancellationToken = default);
}
