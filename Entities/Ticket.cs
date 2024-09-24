namespace FlightTickets.Entities;

public class Ticket
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int TicketNumber { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ArrivalDate { get; set; }
}