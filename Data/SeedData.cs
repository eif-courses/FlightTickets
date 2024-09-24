using Microsoft.AspNetCore.Identity;

namespace FlightTickets.Data;

public static class SeedData
{
    public static async Task Initialize(
        UserManager<MyUserIdentity> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedUsers(userManager);
        
    }


    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Admin", "User" };

        foreach (string roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private static async Task SeedUsers(UserManager<MyUserIdentity> userManager)
    {
        string userName = "admin@viko.lt";
        string password = "Kolegija1@";

        if (await userManager.FindByNameAsync(userName) == null)
        {
            var user = new MyUserIdentity
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true,
                MyBestFriend = "EIF"
            };
            
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
        

        

    }
    
    
}