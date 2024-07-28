using Domain;
using Persistence;

namespace BusinessLogic.Services;

public class ReportService
{
    private readonly IRepository<Booking> _bookingRepository;
    
    public ReportService(IRepository<Booking> bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }
    
    public IList<ReservationReport> GenerateReportFile()
    {
        IList<ReservationReport> reports = new List<ReservationReport>();
        var bookings = _bookingRepository.FindAll();
        foreach (var booking in bookings)
        {
            reports.Add(CreateReport(booking));
        }
        
        return reports;
    }
    
    private ReservationReport CreateReport(Booking booking)
    {
        return new ReservationReport
        {
            DepositInfo = GetDepositInfo(booking),
            ReservationInfo = GetReservationInfo(booking),
            PaymentStatus = GetPaymentStatus(booking)
        };
    }
    
    private string GetDepositInfo(Booking booking)
    {
        return $"Name: {booking._deposit.Name}";
    }
    
    private string GetReservationInfo(Booking booking)
    {
        return $"Id: {booking.Id}, From: {booking.From}, To: {booking.To}, Status: {booking.Status}, Price: {booking.Price}";
    }
    
    private string GetPaymentStatus(Booking booking)
    {
        return $"Status: {booking.payment.Status}";
    }
}