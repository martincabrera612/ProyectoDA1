using BusinessLogic.Services;
using Domain;
using Domain.Enums;
using Moq;
using Persistence;

namespace UnitTests.BusinessLogicTests.Services;

[TestClass]
public class StatsServiceTest
{
    private Mock<IRepository<Booking>> _bookingsRepositoryMock;
    private StatsService _statsService;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _bookingsRepositoryMock = new Mock<IRepository<Booking>>();
        _statsService = new StatsService(_bookingsRepositoryMock.Object);
    }
    
    [TestMethod]
    public void CalculateEarningsInTimeframe_ShouldReturnCorrectValue()
    {
        var from = new DateTime(2022, 1, 1);
        var to = new DateTime(2022, 1, 31);
        var area = Area.A;
        var bookings = new List<Booking>
        {
            new Booking { Price = 100, From = new DateTime(2022, 1, 1), To = new DateTime(2022, 1, 2), Status = Status.Approved, _deposit = new Deposit { Area = Area.A } },
            new Booking { Price = 200, From = new DateTime(2022, 1, 15), To = new DateTime(2022, 1, 16), Status = Status.Approved, _deposit = new Deposit { Area = Area.A } },
            new Booking { Price = 300, From = new DateTime(2022, 2, 1), To = new DateTime(2022, 2, 2), Status = Status.Approved, _deposit = new Deposit { Area = Area.B } }
        };
        _bookingsRepositoryMock.Setup(r => r.FindAll()).Returns(bookings);
        
        var result = _statsService.CalculateEarningsInTimeframe(from, to, area);
        
        Assert.AreEqual(300, result);
    }
    
    [TestMethod]
    public void GetDataForDate_ShouldReturnCorrectValue()
    {
        const int year = 2022;
        const int month = 1;
        var bookings = new List<Booking>
        {
            new Booking { From = new DateTime(2022, 1, 1) },
            new Booking { From = new DateTime(2022, 1, 15) },
            new Booking { From = new DateTime(2022, 2, 1) }
        };
        _bookingsRepositoryMock.Setup(r => r.FindAll()).Returns(bookings);
        
        var result = _statsService.GetDataForDate(year, month);
        
        Assert.AreEqual(2, result);
    }
    
    [TestMethod]
    public void GetDataForDate_ShouldReturnZero_WhenNoBookings()
    {
        var year = 2022;
        var month = 1;
        _bookingsRepositoryMock.Setup(r => r.FindAll()).Returns(new List<Booking>());
        
        var result = _statsService.GetDataForDate(year, month);
        
        Assert.AreEqual(0, result);
    }
    
}