using BusinessLogic.Exceptions;
using BusinessLogic.Validators;
using Domain;
using Domain.Enums;
using Persistence;

namespace BusinessLogic.Controllers;

public class BookingController
{
    private readonly IRepository<Booking> _bookingRepository;
    private readonly SessionLogic _sessionLogic;
    private const string AddBooking = "Booking created successfully";
    private const string UpdateBooking = "Booking updated successfully";
    private const string ApprovedBooking = "Booking approved successfully";
    private const string RejectedBooking = "Booking rejected successfully";
    private const string DeletedBooking = "Booking deleted successfully";
    
    public BookingController(IRepository<Booking> aBookingRepository, SessionLogic aSessionLogic)
    {
        _bookingRepository = aBookingRepository;
        _sessionLogic = aSessionLogic;
    }
    
    public string Create(Booking aBooking)
    {
        Payment payment = new Payment(aBooking.Price);
        aBooking._user = _sessionLogic.CurrentUser;
        aBooking.payment = payment;
        BookingValidator.IsValid(aBooking);
        _bookingRepository.Add(aBooking);
        return AddBooking;
    }

    public string Update(Booking aBooking)
    {
        _bookingRepository.Update(aBooking);
        return UpdateBooking;
    }

    public string ApproveBooking(Booking aBooking)
    {
        try
        {
            if (!_sessionLogic.IsAdmin())
            {
                throw new BusinessLogicException("You must be an admin to approve a booking");
            }
            aBooking.Status = Status.Approved;
            //aBooking.payment.Status = PaymentStatus.Captured;
            _bookingRepository.Update(aBooking);
            return ApprovedBooking;
        }
        catch (BusinessLogicException exception)
        {
            return exception.Message;
        }
    }

    public string RejectBooking(Booking aBooking)
    {
        try
        {
            if (!_sessionLogic.IsAdmin())
            {
                throw new BusinessLogicException("You must be an admin to reject a booking");
            }
            aBooking.Status = Status.Rejected;
            _bookingRepository.Update(aBooking);
            return RejectedBooking;
        }
        catch (BusinessLogicException exception)
        {
            return exception.Message;
        }
    }
    
    public string Delete(string id)
    {
        try
        {
            _bookingRepository.Delete(id);
            return DeletedBooking;
        }
        catch (BusinessLogicException exception)
        {
            return "Booking not found for deletion";
        }
        
    }
}