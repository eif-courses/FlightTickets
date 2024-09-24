using FastEndpoints;
using FlightTickets.Data;
using FlightTickets.Entities;
using FlightTickets.Features.Tickets.Dto;
using Microsoft.EntityFrameworkCore;

namespace FlightTickets.Features.Tickets;




internal sealed class GetTickets : EndpointWithoutRequest<List<TicketResponseDto>, TicketResponseMapper>
{
    private readonly DatabaseContext _context;

    public GetTickets(DatabaseContext context)
    {
        _context = context;
    }


    public override void Configure()
    {
        Get("/tickets");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var tickets = await _context.Tickets.ToListAsync(ct);
        
        var response = Map.FromEntity(tickets);
        
        await SendAsync(response, 200, ct);
    }
}

internal sealed class TicketResponseMapper : ResponseMapper<List<TicketResponseDto>, List<Ticket>>
{
    public override List<TicketResponseDto> FromEntity(List<Ticket> e)
    {
        return e.Select(ticket => new TicketResponseDto
        {
            TicketNumber = ticket.TicketNumber
        }).ToList();
    }
}
