using BusinessLogic;
using BusinessLogic.Controllers;
using Domain;
using Moq;
using Persistence;

namespace UnitTests.BusinessLogicTests;

[TestClass]
public class ReviewControllerTest
{
    private Mock<IRepository<Review>> mockReviewRepository;
    private Mock<SessionLogic> mockSessionLogic;
    private ReviewController reviewController;
    private Review aReview;
    private User aUser;
    private Booking aBooking;
    
    [TestInitialize]
    public void SetUp()
    {
        mockReviewRepository = new Mock<IRepository<Review>>();
        mockSessionLogic = new Mock<SessionLogic>();
        reviewController = new ReviewController(mockReviewRepository.Object, mockSessionLogic.Object);

        aUser = new User();

        aBooking = new Booking()
        {
            _user = aUser,
            To = DateTime.Now.AddDays(-1)
        };
        
        aReview = new Review()
        {
            Rating = 5,
            Comment = "This is a comment",
            User = aUser,
            Booking = aBooking
        };
    }
    
    [TestMethod]
    public void TestUserCanCreateReview()
    {
        mockSessionLogic.Setup(session => session.CurrentUser).Returns(aUser);
        var result = reviewController.Create(aReview);
        
        Assert.AreEqual("Review created successfully", result);
        Assert.AreEqual(aUser, aReview.User);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestUserCannotCreateReview()
    {
        aReview.Rating = 6;
        mockSessionLogic.Setup(session => session.CurrentUser).Returns(aUser);
        var result = reviewController.Create(aReview);
    }
}