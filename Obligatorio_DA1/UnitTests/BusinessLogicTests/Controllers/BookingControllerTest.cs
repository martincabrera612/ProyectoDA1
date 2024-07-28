using BusinessLogic;
using BusinessLogic.Controllers;
using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using Domain;
using Domain.Enums;
using Moq;
using Persistence;

namespace UnitTests.BusinessLogicTests;


[TestClass]
public class BookingControllerTest
{
    private BookingController _bookingController;
    private Booking aBooking;
    private Mock<IRepository<Booking>> mockBookingRepository;
    private Mock<SessionLogic> mockSessionLogic;
    private Mock<PricingService> mockPricingService;
   

    [TestInitialize]
    public void SetUp()
    {   PricingService _pricingService = new PricingService();
        List<Promotion> _promotions = new List<Promotion>();
        User aUser = new User
        {
            Email = "example@gmail.com", Id = "a7b4e045-e569-4a72-a00d-0730b9c84564", IsAdmin = true, Name = "Jhon",
            Password = "Passw@rd1", Surname = "Doe"
        };
        Promotion promotion = new Promotion
        {
            From = new DateTime(2024, 3, 10), To = new DateTime(2024, 4, 25),
            Id = "a7b4e045-e569-4a72-a00d-0730b9c84357", Percentage = 70
        };
        _promotions.Add(promotion);
        Deposit deposit = new Deposit
        {
            AirConditioning = true, Area = Area.A, Id = "a7b4e045-e569-4a72-a00d-0730b9c84356", Promotions = _promotions,
            Size = Size.Medium
        };
        aBooking = new Booking
        {
            _deposit = new Deposit(),
            _user = new User(),
            Status = Status.Pending,
            From = new DateTime(2024, 3, 10),
            To = new DateTime(2024, 4, 25),
            Id = "a7b4e045-e569-4a72-a00d-0730b9c84564",
            Price = 150.5
        };
        mockBookingRepository = new Mock<IRepository<Booking>>();
        mockSessionLogic = new Mock<SessionLogic>();
        _bookingController = new BookingController(mockBookingRepository.Object, mockSessionLogic.Object);
        
        var currentUser = new User
        {
            Email = "example@gmail.com",
            Id = "a7b4e045-e569-4a72-a00d-0730b9c84564",
            IsAdmin = true,
            Name = "John",
            Password = "Passw@rd1",
            Surname = "Doe"
        };
        mockSessionLogic.Setup(x => x.CurrentUser).Returns(currentUser);
    }
    
    [TestMethod]
    public void CreateBookingTest()
    {
        var result = _bookingController.Create(aBooking);
        Assert.AreEqual("Booking created successfully",result);
    }

    [TestMethod]
    public void UpdateBookingTest()
    {
       
        mockBookingRepository.Setup(repo => repo.Update(aBooking)).Returns(aBooking);
        var response = _bookingController.Update(aBooking);
        Assert.AreEqual(response,"Booking updated successfully");
        mockBookingRepository.Verify(repo=>repo.Update(aBooking),Times.Once);

    }

    [TestMethod]
    public void AdminCanApproveTest()
    {
        mockBookingRepository.Setup(repo => repo.Update(aBooking)).Returns(aBooking);
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(true);
        var response = _bookingController.ApproveBooking(aBooking);
        Assert.AreEqual(response,"Booking approved successfully");
        mockBookingRepository.Verify(repo=>repo.Update(aBooking),Times.Once);
    }

    [TestMethod]
    public void AdminCannotApproveTest()
    {
        mockSessionLogic.Setup(session => session.IsAdmin()).Returns(false);
        var response = _bookingController.ApproveBooking(aBooking);
        Assert.AreEqual(response,"You must be an admin to approve a booking");
    }

    [TestMethod]
    public void DeleteBookingTest()
    {
        const string bookingId = "a7b4e045-e569-4a72-a00d-0730b9c84564";
        mockBookingRepository.Setup(repo => repo.Delete(bookingId));
        var result = _bookingController.Delete(bookingId);
        Assert.AreEqual("Booking deleted successfully",result);
    }

    [TestMethod]
    public void DeleteBookingFailTest()
    {
        const string invalidBookingId = "a7b4e045-e569-4a72-a00d-0730b9c84500";
        mockBookingRepository.Setup(repo => repo.Delete(invalidBookingId)).Throws(new BusinessLogicException());
        var result = _bookingController.Delete(invalidBookingId);
        Assert.AreEqual("Booking not found for deletion", result);
    }
    
}