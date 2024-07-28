using Domain;

namespace UnitTests.DomainTests;

[TestClass]
public class ReviewTest
{
    private Review _review = null!;
    
    [TestInitialize]
    public void Init()
    {
        _review = new Review();
    }
    
    [TestMethod]
    public void ValidateRatingTest()
    {
        _review.Rating = 5;
        Assert.IsNotNull(_review.Rating);
    }
    
    [TestMethod]
    public void ValidateCommentTest()
    {
        _review.Comment = "This is a comment";
        Assert.IsNotNull(_review.Comment);
    }
    
    [TestMethod]
    public void ValidateUserNotNullTest()
    {
        _review.User = new User();
        Assert.IsNotNull(_review.User);
    }
    
    [TestMethod]
    public void ValidateBookingNotNullTest()
    {
        _review.Booking = new Booking();
        Assert.IsNotNull(_review.Booking);
    }
}