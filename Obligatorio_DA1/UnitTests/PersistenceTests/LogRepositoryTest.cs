using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Persistence;
using UnitTests.PersistenceTests.Context;

namespace UnitTests.PersistenceTests
{
    [TestClass]
    public class LogRepositoryTest
    {
        private ServiceProvider _serviceProvider;
        private LogRepository _logRepository;
        private User aUser;
        private User aUser2;

        [TestInitialize]
        public void SetUp()
        {
            var services = new ServiceCollection();
            services.AddDbContext<SqlContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
            services.AddScoped<LogRepository>();
            _serviceProvider = services.BuildServiceProvider();
            _logRepository = _serviceProvider.GetRequiredService<LogRepository>();
            
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

        private Log CreateLog(User user, EventType eventType)
        {
            return new Log {User = user, EventType = eventType };
        }

        [TestMethod]
        public void AddLog()
        {
            var aLog = CreateLog(aUser, EventType.UserLogin);
            var addedLog = _logRepository.Add(aLog);
            Assert.IsNotNull(addedLog);
            Assert.AreEqual(aLog, addedLog);
        }

        [TestMethod]
        public void FindLog()
        {
            var aLog = CreateLog(aUser, EventType.UserLogin);
            _logRepository.Add(aLog);

            var foundLog = _logRepository.Find(d => d.Id == aLog.Id);

            Assert.IsNotNull(foundLog);
            Assert.AreEqual(aLog, foundLog);
        }

        [TestMethod]
        public void FindAllLogs()
        {
            var aLog = CreateLog(aUser, EventType.UserLogin);
            var secondLog = CreateLog(aUser2, EventType.UserLogout);

            _logRepository.Add(aLog);
            _logRepository.Add(secondLog);

            var foundLogs = _logRepository.FindAll();

            Assert.AreEqual(2, foundLogs.Count);
            Assert.IsTrue(foundLogs.Contains(aLog));
            Assert.IsTrue(foundLogs.Contains(secondLog));
        }

        [TestMethod]
        public void UpdateLog()
        {
            var aLog = CreateLog(aUser, EventType.UserLogin);
            _logRepository.Add(aLog);

            var updatedLog = CreateLog(aUser, EventType.UserLogout);

            _logRepository.Update(updatedLog);

            var foundLog = _logRepository.Find(d => d.Id == aLog.Id);

            Assert.IsNotNull(foundLog);
            Assert.AreEqual(aLog.Id, foundLog.Id);
        }

        [TestMethod]
        public void DeleteLog()
        {
            var aLog = CreateLog(aUser, EventType.UserLogin);
            _logRepository.Add(aLog);

            _logRepository.Delete(aLog.Id);

            var foundLog = _logRepository.Find(d => d.Id == aLog.Id);

            Assert.IsNull(foundLog);
        }
        
        [TestCleanup]
        public void CleanUp()
        {
            var context = _serviceProvider.GetService<SqlContext>();
            
            context.Logs.RemoveRange(context.Logs);
            
            context.SaveChanges();
            
            _serviceProvider.Dispose();
        }
    }
}