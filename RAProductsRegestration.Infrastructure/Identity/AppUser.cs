using Microsoft.AspNetCore.Identity;

namespace RAProductsRegestration.Infrastructure.Identity;

public sealed class AppUser
    : IdentityUser
{
    public Guid? EmployeeId
    {
        get;
        set;
    }

    public string? Picture
    {
        get;
        set;
    }

    public bool IsDisabled
    {
        get;
        set;
    }

    public string? ShortName
    {
        get;
        set;
    }

    public bool ChangedHisOwnPassword
    {
        get;
        set;
    }

    public bool ChangedHisOwnNickName
    {
        get;
        set;
    }

    public DateTimeOffset? LastLoginAtUtc
    {
        get;
        set;
    }
}