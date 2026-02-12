using DirectoryMS.Domain.Enums;

namespace DirectoryMS.Application.Dtos;

public sealed record UserResponseDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    UserRole Role,
    FumigatorApprovalStatus FumigatorApprovalStatus,
    bool CanOfferServices,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
