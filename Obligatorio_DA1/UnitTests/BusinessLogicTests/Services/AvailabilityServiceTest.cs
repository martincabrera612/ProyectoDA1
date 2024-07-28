using BusinessLogic;
using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using Domain;
using Domain.Enums;
using Moq;
using Persistence;

namespace UnitTests.BusinessLogicTests.Services;

[TestClass]
public class AvailabilityServiceTest
{
    private Mock<IRepository<Deposit>> mockDepositRepository;
    private Mock<SessionLogic> mockSessionLogic;
    private AvailabilityService _availabilityService;
    private Deposit aDeposit;
    private Availability anAvailability;
    private Booking aBooking;

    [TestInitialize]
    public void SetUp()
    {
        aDeposit = new Deposit();
        anAvailability = new Availability
        {
            From = DateTime.Now,
            To = DateTime.Now.AddDays(10)
        };

        aBooking = new Booking
        {
            _deposit = aDeposit,
            From = DateTime.Now,
            To = DateTime.Now.AddDays(6),
            Status = Status.Approved
        };
        mockDepositRepository = new Mock<IRepository<Deposit>>();
        mockSessionLogic = new Mock<SessionLogic>();

        _availabilityService = new AvailabilityService(mockDepositRepository.Object, mockSessionLogic.Object);
    }

    [TestMethod]
    public void IsAvailable_WhenDepositHasAvailabilityAndNoOverlap_ReturnsTrue()
    {
        aDeposit.Availabilities.Add(anAvailability);
        mockDepositRepository.Setup(x => x.FindAll()).Returns(new List<Deposit> { aDeposit });

        var result = _availabilityService.IsAvailable(aDeposit, DateTime.Now, DateTime.Now.AddDays(1));

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsAvailable_WhenDepositHasAvailabilityAndOverlap_ReturnsFalse()
    {
        aDeposit.Availabilities.Add(anAvailability);
        aDeposit.Bookings.Add(aBooking);
        mockDepositRepository.Setup(x => x.FindAll()).Returns(new List<Deposit> { aDeposit });

        var result = _availabilityService.IsAvailable(aDeposit, DateTime.Now, DateTime.Now.AddDays(1));

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void GetAvailableDeposits_WhenDepositIsAvailable_ReturnsDeposit()
    {
        aDeposit.Availabilities.Add(anAvailability);
        mockDepositRepository.Setup(x => x.FindAll()).Returns(new List<Deposit> { aDeposit });

        var result = _availabilityService.GetAvailableDeposits(DateTime.Now, DateTime.Now.AddDays(1));

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void AddAvailability_WhenAvailabilityDoesNotExist_AddsAvailability()
    {
        mockDepositRepository.Setup(x => x.Find(It.IsAny<Func<Deposit, bool>>())).Returns(aDeposit);
        mockSessionLogic.Setup(x => x.HasAdminPrivileges());
        mockDepositRepository.Setup(x => x.Update(aDeposit));

        _availabilityService.AddAvailability(aDeposit.Id, DateTime.Now, DateTime.Now.AddDays(1));

        Assert.AreEqual(1, aDeposit.Availabilities.Count);
    }

    [TestMethod]
    public void AddAvailability_WhenAvailabilityExists_ThrowsBusinessLogicException()
    {
        aDeposit.Availabilities.Add(anAvailability);
        mockDepositRepository.Setup(x => x.Find(It.IsAny<Func<Deposit, bool>>())).Returns(aDeposit);

        Assert.ThrowsException<BusinessLogicException>(() =>
        {
            _availabilityService.AddAvailability(aDeposit.Id, DateTime.Now, DateTime.Now.AddDays(1));
        });
    }
}