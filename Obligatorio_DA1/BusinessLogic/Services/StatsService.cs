using System.ComponentModel;
using Domain;
using Domain.Enums;
using Persistence;

namespace BusinessLogic.Services;

public class StatsService
{
    private readonly IRepository<Booking> _bookingsRepository;
    
    public StatsService(IRepository<Booking> bookingsRepository)
    {
        _bookingsRepository = bookingsRepository;
    }
    
    public double CalculateEarningsInTimeframe(DateTime from, DateTime to, Area area)
    {
        var bookings = GetBookingsInTimeframe(from, to, area);
        return CalculateTotalEarnings(bookings);
    }

    private IEnumerable<Booking> GetBookingsInTimeframe(DateTime from, DateTime to, Area area)
    {
        return _bookingsRepository.FindAll()
            .Where(b => b.To >= from && b.From <= to && b.Status == Status.Approved && b._deposit.Area == area);
    }

    private double CalculateTotalEarnings(IEnumerable<Booking> bookings)
    {
        return bookings.Sum(b => b.Price);
    }
    
    public int GetDataForDate(int year, int month)
    {
        return _bookingsRepository.FindAll()
            .Count(b => b.From.Year == year && b.From.Month == month);
    }
}