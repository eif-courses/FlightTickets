using FlightTickets.Data;
using FlightTickets.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FlightTickets.Features.Tickets;
using FastEndpoints;


public class CreateTicket: Endpoint<Ticket>
{
    private readonly DatabaseContext _context;

    public CreateTicket(DatabaseContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
       Post("/tickets/create");
       AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
       Roles("Admin");
    }

    public override async Task HandleAsync(Ticket req, CancellationToken ct)
    {
        await _context.Tickets.AddAsync(req, ct);
        await _context.SaveChangesAsync(ct);

        await SendOkAsync(ct);

    }
}