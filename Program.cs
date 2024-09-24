using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using FlightTickets.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();


builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlite("Data Source=FlightTickets.db");
});

builder.Services.AddIdentity<MyUserIdentity, IdentityRole>()
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthenticationCookie(validFor: TimeSpan.FromDays(30), options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddAuthorization();


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    
    context.Database.Migrate();
    
    var userManager = services.GetRequiredService<UserManager<MyUserIdentity>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    
    await SeedData.Initialize(userManager, roleManager);
    
}


app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen();

app.Run();