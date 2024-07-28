using Domain;

namespace BusinessLogic.Validators;

public class ReviewValidator
{
    public static bool Validate(Review aReview)
    {
        ValidateRating(aReview.Rating);
        ValidateComment(aReview.Comment);
        ValidateBookingUser(aReview.Booking, aReview.User);
        ValidateBooking(aReview.Booking);
        return true;
    }
    
    public static void ValidateRating(int rating)
    {
         if (rating < 1 || rating > 5)
         {
             throw new InvalidOperationException("Rating must be between 1 and 5");
         }
    }
    
    public static void ValidateComment(string comment)
    {
        if (comment.Length > 500)
        {
            throw new InvalidOperationException("Comment has to be less than 500 characters");
        }
    }
    
    public static void ValidateBookingUser(Booking booking, User aUser){
        if (booking._user != aUser)
        {
            throw new InvalidOperationException("Booking does not belong to user");
        }
    }
    
    public static void ValidateBooking(Booking booking)
    {
        if (booking.To > DateTime.Now)
        {
            throw new InvalidOperationException("Booking is not finished");
        }
    }
}