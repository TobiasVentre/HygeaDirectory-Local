using DirectoryMS.Domain.Enums;

namespace DirectoryMS.Application.Dtos;

public sealed record UpdateUserRequestDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    UserRole Role);
