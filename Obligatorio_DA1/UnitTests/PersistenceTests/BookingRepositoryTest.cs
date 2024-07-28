using BusinessLogic.Services;
using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using UnitTests.PersistenceTests.Context;

namespace UnitTests.PersistenceTests
{
    [TestClass]
    public class BookingRepositoryTest
    {
        private ServiceProvider _serviceProvider;
        private BookingRepository _bookingRepository;
        private List<Promotion> _promotions;
        private User aUser;
        private User aUser2;
        private Promotion promotion;
        private Deposit deposit;
        private PricingService _pricingService;
        private SqlContextFactory _contextFactory;

        [TestInitialize]
        public void Init()
        {
            var services = new ServiceCollection();
            services.AddDbContext<SqlContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
            services.AddScoped<BookingRepository>();
            _serviceProvider = services.BuildServiceProvider();
            _bookingRepository = _serviceProvider.GetRequiredService<BookingRepository>();

            _contextFactory = new SqlContextFactory();
            var context = _contextFactory.CreateMemoryContext();

            _pricingService = new PricingService();
            _promotions = new List<Promotion>();
            aUser = new User
            {
                Email = "example@gmail.com",
                IsAdmin = true,
                Name = "Jhon",
                Password = "Passw@rd1",
                Surname = "Doe"
            };
            aUser2 = new User
            {
                Email = "example2@gmail.com",
                IsAdmin = true,
                Name = "Nick",
                Password = "Passw@rd1",
                Surname = "Doe"
            };
            promotion = new Promotion
            {
                From = new DateTime(2024, 3, 10),
                To = new DateTime(2024, 4, 25),
                Percentage = 70,
                Label = "Spring Promotion"
            };
            _promotions.Add(promotion);
            deposit = new Deposit
            {
                Name = "Deposit",
                AirConditioning = true,
                Area = Area.A,
                Promotions = _promotions,
                Size = Size.Medium
            };
        }

        [TestMethod]
        public void AddBookingRepositoryTest()
        {
            _pricingService = new PricingService
                { _deposit = deposit, From = new DateTime(2024, 3, 11), To = new DateTime(2024, 4, 27) };
            var aBooking = new Booking
            {
                _deposit = deposit, _user = aUser, Status = Status.Pending, From = new DateTime(2024, 3, 11),
                To = new DateTime(2024, 4, 27),
                Price = _pricingService.CalculatePrice()
            };
            var addedBooking = _bookingRepository.Add(aBooking);
            Assert.IsNotNull(addedBooking);
            Assert.AreEqual(aBooking.Id, addedBooking.Id);
        }

        [TestMethod]
        public void FindBookingRepositoryTest()
        {
            _pricingService = new PricingService
            {
                _deposit = deposit, From = new DateTime(2024, 3, 11),
                To = new DateTime(2024, 4, 27)
            };
            var aBooking = new Booking
            {
                _deposit = deposit, _user = aUser, Status = Status.Pending, From = new DateTime(2024, 3, 11),
                To = new DateTime(2024, 4, 27),
                Price = _pricingService.CalculatePrice()
            };
            _bookingRepository.Add(aBooking);
            var foundBooking = _bookingRepository.Find(b => b.Id == aBooking.Id);
            Assert.IsNotNull(foundBooking);
            Assert.AreEqual(aBooking.Id, foundBooking.Id);
        }

        [TestMethod]
        public void FindAllBookingRepositoryTest()
        {
            _pricingService = new PricingService
            {
                _deposit = deposit,
                From = new DateTime(2024, 3, 11),
                To = new DateTime(2024, 4, 27)
            };
            var booking1 = new Booking
            {
                _deposit = deposit,
                _user = aUser,
                Status = Status.Pending,
                From = new DateTime(2024, 3, 11),
                To = new DateTime(2024, 4, 27),
                Price = _pricingService.CalculatePrice()
            };
            _pricingService = new PricingService
            {
                _deposit = deposit,
                From = new DateTime(2024, 2, 7),
                To = new DateTime(2024, 6, 2)
            };
            var booking2 = new Booking
            {
                _deposit = deposit, _user = aUser2,
                Status = Status.Approved,
                From = new DateTime(2024, 2, 7),
                To = new DateTime(2024, 6, 2),
                Price = _pricingService.CalculatePrice()
            };
            _bookingRepository.Add(booking1);
            _bookingRepository.Add(booking2);
            var foundBookings = _bookingRepository.FindAll();

            Assert.AreEqual(2, foundBookings.Count);
            Assert.IsTrue(foundBookings.Contains(booking1));
            Assert.IsTrue(foundBookings.Contains(booking2));
        }

        [TestMethod]
        public void UpdateBookingRepositoryTest()
        {
            _pricingService = new PricingService
            {
                _deposit = deposit, From = new DateTime(2024, 3, 11),
                To = new DateTime(2024, 4, 27)
            };
            var aBooking = new Booking
            {
                _deposit = deposit, _user = aUser, Status = Status.Pending, From = new DateTime(2024, 3, 11),
                To = new DateTime(2024, 4, 27),
                Price = _pricingService.CalculatePrice()
            };
            _bookingRepository.Add(aBooking);
            aBooking.Status = Status.Approved;
            var updatedBooking = _bookingRepository.Update(aBooking);
            Assert.AreEqual(aBooking.Id, updatedBooking.Id);
            Assert.AreEqual(aBooking.Status, updatedBooking.Status);
        }

        [TestMethod]
        public void DeleteBookingRepositoryTest()
        {
            _pricingService = new PricingService
            {
                _deposit = deposit, From = new DateTime(2024, 3, 11),
                To = new DateTime(2024, 4, 27)
            };
            var aBooking = new Booking
            {
                _deposit = deposit, _user = aUser, Status = Status.Rejected, From = new DateTime(2024, 3, 11),
                To = new DateTime(2024, 4, 27),
                Price = _pricingService.CalculatePrice()
            };
            _bookingRepository.Add(aBooking);
            _bookingRepository.Delete(aBooking.Id);
            var foundBooking = _bookingRepository.Find(b => b.Id == aBooking.Id);
            Assert.IsNull(foundBooking);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Booking not found for deletion")]
        public void DeleteBookingNotFoundRepositoryTest()
        {
            const string nonExisitingId = "00000000000";
            _bookingRepository.Delete(nonExisitingId);
        }

        [TestCleanup]
        public void CleanUp()
        {
            var context = _serviceProvider.GetService<SqlContext>();
            
            context.Bookings.RemoveRange(context.Bookings);
            
            context.SaveChanges();
            
            _serviceProvider.Dispose();
        }
    }
}
