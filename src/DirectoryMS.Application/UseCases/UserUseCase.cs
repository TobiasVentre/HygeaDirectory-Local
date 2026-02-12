using DirectoryMS.Application.Common;
using DirectoryMS.Application.Dtos;
using DirectoryMS.Application.Interfaces;
using DirectoryMS.Domain.Entities;
using DirectoryMS.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace DirectoryMS.Application.UseCases;

public class UserUseCase : IUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserUseCase> _logger;

    public UserUseCase(IUserRepository userRepository, ILogger<UserUseCase> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<UserResponseDto>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(MapToResponse).ToArray();
    }

    public async Task<UserResponseDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetExistingUserAsync(id, cancellationToken);
        return MapToResponse(user);
    }

    public async Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request.FirstName, request.LastName, request.Email, request.PhoneNumber);

        var user = new User(
            Guid.NewGuid(),
            request.FirstName.Trim(),
            request.LastName.Trim(),
            request.Email.Trim().ToLowerInvariant(),
            request.PhoneNumber.Trim(),
            request.Role,
            DateTime.UtcNow);

        await _userRepository.AddAsync(user, cancellationToken);

        _logger.LogInformation(
            "User created {@UserId} role {Role} at {TimestampUtc}",
            user.Id,
            user.Role,
            user.CreatedAtUtc);

        return MapToResponse(user);
    }

    public async Task<UserResponseDto> UpdateUserAsync(Guid id, UpdateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request.FirstName, request.LastName, request.Email, request.PhoneNumber);

        var user = await GetExistingUserAsync(id, cancellationToken);
        user.UpdateProfile(
            request.FirstName.Trim(),
            request.LastName.Trim(),
            request.Email.Trim().ToLowerInvariant(),
            request.PhoneNumber.Trim(),
            request.Role);

        await _userRepository.UpdateAsync(user, cancellationToken);

        _logger.LogInformation(
            "User updated {@UserId} role {Role} at {TimestampUtc}",
            user.Id,
            user.Role,
            user.UpdatedAtUtc);

        return MapToResponse(user);
    }

    public async Task<UserResponseDto> ApproveFumigatorAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await GetExistingUserAsync(userId, cancellationToken);

        user.ApproveFumigator();
        await _userRepository.UpdateAsync(user, cancellationToken);

        _logger.LogInformation(
            "Fumigator approved {@UserId} status {Status} canOffer {CanOfferServices}",
            user.Id,
            user.FumigatorApprovalStatus,
            user.CanOfferServices);

        return MapToResponse(user);
    }

    public async Task<UserResponseDto> DisableFumigatorAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await GetExistingUserAsync(userId, cancellationToken);

        user.DisableFumigator();
        await _userRepository.UpdateAsync(user, cancellationToken);

        _logger.LogInformation(
            "Fumigator disabled {@UserId} status {Status} canOffer {CanOfferServices}",
            user.Id,
            user.FumigatorApprovalStatus,
            user.CanOfferServices);

        return MapToResponse(user);
    }

    private async Task<User> GetExistingUserAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        return user ?? throw new NotFoundException($"User with id '{id}' was not found.");
    }

    private static void ValidateRequest(string firstName, string lastName, string email, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ValidationException("FirstName is required.");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ValidationException("LastName is required.");
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new ValidationException("A valid Email is required.");
        }

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new ValidationException("PhoneNumber is required.");
        }
    }

    private static UserResponseDto MapToResponse(User user) =>
        new(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Role,
            user.FumigatorApprovalStatus,
            user.CanOfferServices,
            user.CreatedAtUtc,
            user.UpdatedAtUtc);
}
