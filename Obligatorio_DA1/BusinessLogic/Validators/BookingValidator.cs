using BusinessLogic.Services;
using Domain;
using Domain.Enums;

namespace BusinessLogic.Validators;

public static class BookingValidator
{
    public static void IsValid(Booking aBooking)
    {
        ValidateDate(aBooking);
        ValidateApprovalDefault(aBooking);
        ValidateUserNotNull(aBooking);
        ValidateDepositNotNull(aBooking);

    }

    public static void ValidateDate(Booking _booking)
    {
        if (_booking.From > _booking.To)
        {
            throw new InvalidOperationException("The 'From' date cannot be later than the 'To' date");
        }

    }

    public static void ValidateApprovalDefault(Booking _booking)
    {
        if (_booking.Status == Status.Approved)
        {
            throw new InvalidOperationException("The Approved default state must be false");
        }
    }

    public static void ValidateUserNotNull(Booking _booking)
    {
        if (_booking._user == null)
        {
            throw new InvalidOperationException("The User cannot be Null");
        }
    }

    public static void ValidateDepositNotNull(Booking _booking)
    {
        if (_booking._deposit == null)
        {
            throw new InvalidOperationException("The Deposit cannot be Null");
        }
    }
}