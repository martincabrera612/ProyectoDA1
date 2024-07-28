using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace UnitTests.PersistenceTests
{
    [TestClass]
    public class DepositRepositoryTests
    {
        private ServiceProvider _serviceProvider;
        private DepositRepository _depositRepository;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<SqlContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
            services.AddScoped<DepositRepository>();
            _serviceProvider = services.BuildServiceProvider();
            _depositRepository = _serviceProvider.GetRequiredService<DepositRepository>();
        }

        private Deposit CreateDeposit(Size size, Area area, bool airConditioning)
        {
            return new Deposit {Name = "Deposit", Size = size, Area = area, AirConditioning = airConditioning };
        }

        [TestMethod]
        public void TestAddDepositRepository()
        {
            var aDeposit = CreateDeposit(Size.Medium, Area.A, false);
            var addedDeposit = _depositRepository.Add(aDeposit);
            Assert.IsNotNull(addedDeposit);
            Assert.AreEqual(aDeposit, addedDeposit);
        }

        [TestMethod]
        public void TestFindDepositRepository()
        {
            var aDeposit = CreateDeposit(Size.Medium, Area.A, false);
            _depositRepository.Add(aDeposit);

            var foundDeposit = _depositRepository.Find(d => d.Id == aDeposit.Id);

            Assert.IsNotNull(foundDeposit);
            Assert.AreEqual(aDeposit, foundDeposit);
        }
        
        [TestMethod]
        public void TestFindAllDepositsRepository()
        {
            var aDeposit = CreateDeposit( Size.Medium, Area.A, false);
            var secondDeposit = CreateDeposit(Size.Big, Area.B, true);
    
            _depositRepository.Add(aDeposit);
            _depositRepository.Add(secondDeposit);

            var foundDeposits = _depositRepository.FindAll();

            Assert.AreEqual(2, foundDeposits.Count);
            Assert.IsTrue(foundDeposits.Contains(aDeposit));
            Assert.IsTrue(foundDeposits.Contains(secondDeposit));
        }

        [TestMethod]
        public void TestUpdateDepositRepository()
        {
            var aDeposit = CreateDeposit(Size.Medium, Area.A, false);
            _depositRepository.Add(aDeposit);
            
            aDeposit.Area = Area.B;
            var updatedDeposit = _depositRepository.Update(aDeposit);

            Assert.IsNotNull(updatedDeposit);
            Assert.AreEqual(aDeposit, updatedDeposit);
            
            var nonExistentDeposit = CreateDeposit(Size.Small, Area.C, true);
            var result = _depositRepository.Update(nonExistentDeposit);
            
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestDeleteDepositRepository()
        {
            var aDeposit = CreateDeposit(Size.Medium, Area.A, false);
            _depositRepository.Add(aDeposit);

            _depositRepository.Delete(aDeposit.Id);

            var foundDeposit = _depositRepository.Find(d => d.Id == aDeposit.Id);
            Assert.IsNull(foundDeposit);
        }
        
        [TestCleanup]
        public void CleanUp()
        {
            var context = _serviceProvider.GetService<SqlContext>();
            
            context.Deposits.RemoveRange(context.Deposits);
            
            context.SaveChanges();
            
            _serviceProvider.Dispose();
        }
    }
}