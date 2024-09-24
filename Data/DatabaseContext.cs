using FlightTickets.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlightTickets.Data;

public class DatabaseContext: IdentityDbContext<MyUserIdentity>
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Ticket> Tickets { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("Data Source=FlightTickets.db");
    }
}