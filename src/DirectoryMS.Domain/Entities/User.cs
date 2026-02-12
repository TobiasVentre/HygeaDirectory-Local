using DirectoryMS.Domain.Enums;

namespace DirectoryMS.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public UserRole Role { get; private set; }
    public FumigatorApprovalStatus FumigatorApprovalStatus { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    public bool CanOfferServices =>
        Role == UserRole.Fumigator && FumigatorApprovalStatus == FumigatorApprovalStatus.Approved;

    public User(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        UserRole role,
        DateTime createdAtUtc)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Role = role;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
        FumigatorApprovalStatus = role == UserRole.Fumigator
            ? FumigatorApprovalStatus.Pending
            : FumigatorApprovalStatus.NotApplicable;
    }

    public void UpdateProfile(string firstName, string lastName, string email, string phoneNumber, UserRole role)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;

        if (Role != role)
        {
            Role = role;
            FumigatorApprovalStatus = role switch
            {
                UserRole.Fumigator => FumigatorApprovalStatus.Pending,
                _ => FumigatorApprovalStatus.NotApplicable
            };
        }

        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ApproveFumigator()
    {
        if (Role != UserRole.Fumigator)
        {
            throw new InvalidOperationException("Only users with Fumigator role can be approved.");
        }

        FumigatorApprovalStatus = FumigatorApprovalStatus.Approved;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void DisableFumigator()
    {
        if (Role != UserRole.Fumigator)
        {
            throw new InvalidOperationException("Only users with Fumigator role can be disabled.");
        }

        FumigatorApprovalStatus = FumigatorApprovalStatus.Disabled;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
