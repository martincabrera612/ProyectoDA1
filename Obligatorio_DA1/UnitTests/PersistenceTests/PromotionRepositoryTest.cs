using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace UnitTests.PersistenceTests
{
    [TestClass]
    public class PromotionRepositoryTests
    {
        private ServiceProvider _serviceProvider;
        private PromotionRepository _promotionRepository;

        [TestInitialize]
        public void SetUp()
        {
            var services = new ServiceCollection();
            services.AddDbContext<SqlContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
            services.AddScoped<PromotionRepository>();
            _serviceProvider = services.BuildServiceProvider();
            _promotionRepository = _serviceProvider.GetRequiredService<PromotionRepository>();
        }

        [TestMethod]
        public void TestAddPromotionRepository()
        {
            var aPromotion = new Promotion
            {
                Label = "Christmas Sale",
                Percentage = 70,
                From = new DateTime(2024, 12, 10),
                To = new DateTime(2024, 12, 25)
            };
            var addedPromotion = _promotionRepository.Add(aPromotion);
            Assert.IsNotNull(addedPromotion);
            Assert.AreEqual(aPromotion.Id, addedPromotion.Id);
        }

        [TestMethod]
        public void TestFindPromotionRepository()
        {
            var aPromotion = new Promotion
            {
                Label = "Christmas Sale",
                Percentage = 70,
                From = new DateTime(2024, 12, 10),
                To = new DateTime(2024, 12, 25)
            };
            _promotionRepository.Add(aPromotion);

            var foundPromotion = _promotionRepository.Find(p => p.Id == aPromotion.Id);

            Assert.IsNotNull(foundPromotion);
            Assert.AreEqual(aPromotion.Id, foundPromotion.Id);
        }

        [TestMethod]
        public void TestFindAllPromotionsRepository()
        {
            var aPromotion = new Promotion
            {
                Label = "Christmas Sale",
                Percentage = 70,
                From = new DateTime(2024, 12, 10),
                To = new DateTime(2024, 12, 25)
            };
            var secondPromotion = new Promotion
            {
                Label = "Halloween Sale",
                Percentage = 50,
                From = new DateTime(2023, 10, 03),
                To = new DateTime(2024, 11, 24)
            };

            _promotionRepository.Add(aPromotion);
            _promotionRepository.Add(secondPromotion);

            var foundPromotions = _promotionRepository.FindAll();

            Assert.AreEqual(2, foundPromotions.Count);
            Assert.IsTrue(foundPromotions.Contains(aPromotion));
            Assert.IsTrue(foundPromotions.Contains(secondPromotion));
        }

        [TestMethod]
        public void TestUpdatePromotionRepository()
        {
            var aPromotion = new Promotion
            {
                Label = "Christmas Sale",
                Percentage = 70,
                From = new DateTime(2024, 12, 10),
                To = new DateTime(2024, 12, 25)
            };
            _promotionRepository.Add(aPromotion);

            aPromotion.Label = "Halloween Sale";
            var updatedPromotion = _promotionRepository.Update(aPromotion);

            Assert.IsNotNull(updatedPromotion);
            Assert.AreEqual(aPromotion.Id, updatedPromotion.Id);
            Assert.AreEqual(aPromotion.Label, updatedPromotion.Label);

            var nonExistentPromotion = new Promotion
            {
                Label = "Summer Sale",
                Percentage = 50,
                From = new DateTime(2025, 6, 1),
                To = new DateTime(2025, 8, 31)
            };
            var result = _promotionRepository.Update(nonExistentPromotion);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestDeletePromotionRepository()
        {
            var aPromotion = new Promotion
            {
                Label = "Christmas Sale",
                Percentage = 70,
                From = new DateTime(2024, 12, 10),
                To = new DateTime(2024, 12, 25)
            };
            _promotionRepository.Add(aPromotion);

            _promotionRepository.Delete(aPromotion.Id);

            var foundPromotion = _promotionRepository.Find(p => p.Id == aPromotion.Id);
            Assert.IsNull(foundPromotion);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Promotion not found for deletion")]
        public void TestDeletePromotionNotFoundRepository()
        {
            const string nonExistingId = "0aaab648-464f-4d2a-b18b-c064929ce266";

            _promotionRepository.Delete(nonExistingId);
        }

        [TestCleanup]
        public void CleanUp()
        {
            var context = _serviceProvider.GetService<SqlContext>();
            
            context.Promotions.RemoveRange(context.Promotions);
            
            context.SaveChanges();
            
            _serviceProvider.Dispose();
        }
    }
}