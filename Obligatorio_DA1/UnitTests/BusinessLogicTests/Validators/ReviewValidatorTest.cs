using BusinessLogic.Validators;
using Domain;

namespace UnitTests.BusinessLogicTests;

[TestClass]
public class ReviewValidatorTest
{
    private Review _review = null!;
    
    [TestInitialize]
    public void SetUp()
    {
        _review = new Review();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestIsBookingFinishedFail()
    {
           _review.Booking = new Booking
           {
               To = DateTime.Now.AddDays(1)
           };
           
       ReviewValidator.ValidateBooking(_review.Booking);
    }
    
    [TestMethod]
    public void TestIsBookingFinishedSuccess()
    {
        _review.Booking = new Booking
        {
            To = DateTime.Now.AddDays(-1)
        };
        
        ReviewValidator.ValidateBooking(_review.Booking);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestRatingFail()
    {
        _review.Rating = 6;
        ReviewValidator.ValidateRating(_review.Rating);
    }
    
    [TestMethod]
    public void TestRatingSuccess()
    {
        _review.Rating = 5;
        ReviewValidator.ValidateRating(_review.Rating);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestCommentFail()
    {
        _review.Comment = new string('a', 501);
        ReviewValidator.ValidateComment(_review.Comment);
    }
    
    [TestMethod]
    public void TestCommentSuccess()
    {
        _review.Comment = "12345";
        ReviewValidator.ValidateComment(_review.Comment);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestBookingUserFail()
    {
        _review.Booking = new Booking
        {
            _user = new User()
        };
        
        _review.User = new User();
        
        ReviewValidator.ValidateBookingUser(_review.Booking, _review.User);
    }
    
    [TestMethod]
    public void TestBookingUserSuccess()
    {
        _review.Booking = new Booking
        {
            _user = new User()
        };
        
        _review.User = _review.Booking._user;
        
        ReviewValidator.ValidateBookingUser(_review.Booking, _review.User);
    }
}