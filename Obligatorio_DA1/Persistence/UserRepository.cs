using Domain;

namespace Persistence
{
    public class UserRepository : IRepository<User>
    {
        private SqlContext _database;

        public UserRepository(SqlContext database)
        {
            _database = database;
        }

        public User Add(User aUser)
        {
            _database.Users.Add(aUser);
            _database.SaveChanges();
            return aUser;
        }

        public User? Find(Func<User, bool> filter)
        {
            return _database.Users.FirstOrDefault(filter);
        }

        public IList<User> FindAll()
        {
            return _database.Users.ToList();
        }

        public User? Update(User updatedEntity)
        {
            var user = _database.Users.FirstOrDefault(u => u.Id == updatedEntity.Id);
            if (user == null) return null;
            user.Name = updatedEntity.Name;
            user.Surname = updatedEntity.Surname;
            user.Email = updatedEntity.Email;
            user.Password = updatedEntity.Password;
            user.IsAdmin = updatedEntity.IsAdmin;

            _database.SaveChanges();
            return user;
        }

        public void Delete(string id)
        {
            var user = _database.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _database.Users.Remove(user);
                _database.SaveChanges();
            }
            else
            {
                throw new Exception("User not found for deletion");
            }
        }
    }
}