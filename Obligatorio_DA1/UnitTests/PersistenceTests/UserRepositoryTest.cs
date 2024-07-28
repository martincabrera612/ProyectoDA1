using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace UnitTests.PersistenceTests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private ServiceProvider _serviceProvider;
        private UserRepository _userRepository;
        private User aUser;
        private User aUser2;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<SqlContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
            services.AddScoped<UserRepository>();
            _serviceProvider = services.BuildServiceProvider();
            _userRepository = _serviceProvider.GetRequiredService<UserRepository>();
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

        [TestMethod]
        public void TestAddUserRepository()
        {
            var addedUser = _userRepository.Add(aUser);
            Assert.IsNotNull(addedUser);
            Assert.AreEqual(aUser.Id, addedUser.Id);
        }

        [TestMethod]
        public void TestFindUserRepository()
        {
            _userRepository.Add(aUser);

            var foundUser = _userRepository.Find(u => u.Id == aUser.Id);

            Assert.IsNotNull(foundUser);
            Assert.AreEqual(aUser.Id, foundUser.Id);
            Assert.AreEqual(aUser.Name, foundUser.Name);
        }

        [TestMethod]
        public void TestFindAllUsersRepository()
        {
            _userRepository.Add(aUser);
            _userRepository.Add(aUser2);

            var foundUsers = _userRepository.FindAll();

            Assert.AreEqual(2, foundUsers.Count);
            Assert.IsTrue(foundUsers.Contains(aUser));
            Assert.IsTrue(foundUsers.Contains(aUser2));
        }

        [TestMethod]
        public void TestUpdateUserRepository()
        {
            _userRepository.Add(aUser);

            aUser.Name = "Jane Doe";
            var updatedUser = _userRepository.Update(aUser);

            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(aUser.Id, updatedUser.Id);
            Assert.AreEqual(aUser.Name, updatedUser.Name);
        }

        [TestMethod]
        public void TestDeleteUserRepository()
        {
            _userRepository.Add(aUser);

            _userRepository.Delete(aUser.Id);

            var foundUser = _userRepository.Find(u => u.Id == aUser.Id);
            Assert.IsNull(foundUser);
        }

        [TestCleanup]
        public void CleanUp()
        {
            var context = _serviceProvider.GetService<SqlContext>();
            
            context.Users.RemoveRange(context.Users);
            
            context.SaveChanges();
            
            _serviceProvider.Dispose();
        }
    }
}