using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace UnitTests.PersistenceTests
{
    [TestClass]
    public class ReviewRepositoryTest
    {
        private ServiceProvider _serviceProvider;
        private ReviewRepository _reviewRepository;
        private User aUser;
        private User aUser2;

        [TestInitialize]
        public void SetUp()
        {
            var services = new ServiceCollection();
            services.AddDbContext<SqlContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
            services.AddScoped<ReviewRepository>();
            _serviceProvider = services.BuildServiceProvider();
            _reviewRepository = _serviceProvider.GetRequiredService<ReviewRepository>();
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
        }

        private Review CreateReview(int rating, string comment, User user, Booking booking)
        {
            return new Review { Rating = rating, Comment = comment, User = user, Booking = booking };
        }

        [TestMethod]
        public void AddReview()
        {
            var aReview = CreateReview(5, "Great!", aUser, new Booking());
            var addedReview = _reviewRepository.Add(aReview);
            Assert.IsNotNull(addedReview);
            Assert.AreEqual(aReview, addedReview);
        }

        [TestMethod]
        public void FindReview()
        {
            var aReview = CreateReview(5, "Great!", aUser, new Booking());
            _reviewRepository.Add(aReview);

            var foundReview = _reviewRepository.Find(d => d.Id == aReview.Id);

            Assert.IsNotNull(foundReview);
            Assert.AreEqual(aReview, foundReview);
        }

        [TestMethod]
        public void FindAllReviews()
        {
            var aReview = CreateReview(5, "Great!", aUser, new Booking());
            var secondReview = CreateReview(1, "Bad!", aUser2, new Booking());

            _reviewRepository.Add(aReview);
            _reviewRepository.Add(secondReview);

            var foundReviews = _reviewRepository.FindAll();

            Assert.AreEqual(2, foundReviews.Count);
            Assert.IsTrue(foundReviews.Contains(aReview));
            Assert.IsTrue(foundReviews.Contains(secondReview));
        }

        [TestMethod]
        public void UpdateReview()
        {
            var aReview = CreateReview(5, "Great!", aUser, new Booking());
            _reviewRepository.Add(aReview);

            var updatedReview = CreateReview(1, "Bad!", aUser, new Booking());
            updatedReview.Id = aReview.Id;

            _reviewRepository.Update(updatedReview);

            var foundReview = _reviewRepository.Find(d => d.Id == aReview.Id);

            Assert.IsNotNull(foundReview);
            Assert.AreEqual(aReview.Id, foundReview.Id);
        }

        [TestMethod]
        public void DeleteReview()
        {
            var aReview = CreateReview(5, "Great!", aUser, new Booking());
            _reviewRepository.Add(aReview);

            _reviewRepository.Delete(aReview.Id);

            var foundReview = _reviewRepository.Find(d => d.Id == aReview.Id);

            Assert.IsNull(foundReview);
        }

        [TestMethod]
        public void DeleteReviewById()
        {
            var aReview = CreateReview(5, "Great!", aUser, new Booking());
            _reviewRepository.Add(aReview);

            _reviewRepository.Delete(aReview.Id);

            var foundReview = _reviewRepository.Find(d => d.Id == aReview.Id);

            Assert.IsNull(foundReview);
        }
        
        [TestCleanup]
        public void CleanUp()
        {
            var context = _serviceProvider.GetService<SqlContext>();
            
            context.Reviews.RemoveRange(context.Reviews);
            
            context.SaveChanges();
            
            _serviceProvider.Dispose();
        }
    }
}
    
    
    