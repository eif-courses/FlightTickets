using Microsoft.AspNetCore.Identity;

namespace FlightTickets.Data;

public class MyUserIdentity : IdentityUser
{
    public string? MyBestFriend { get; set; }
}