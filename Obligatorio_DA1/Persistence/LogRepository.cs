using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class LogRepository : IRepository<Log>
    {
        private SqlContext _database;

        public LogRepository(SqlContext database)
        {
            _database = database;
        }
    
        public Log Add(Log aLog)
        {
            var existingUser = _database.Users.Find(aLog.User.Id);
            if (existingUser != null)
            {
                _database.Users.Attach(existingUser);
                aLog.User = existingUser;
            }

            _database.Logs.Add(aLog);
            _database.SaveChanges();
            return aLog;
        }
    
        public Log? Find(Func<Log, bool> filter)
        {
            return _database.Logs.FirstOrDefault(filter);
        }
    
        public IList<Log> FindAll()
        {
            return _database.Logs.Include(log => log.User).ToList();
        }
    
        public Log? Update(Log updatedEntity)
        {
            var log = _database.Logs.FirstOrDefault(r => r.Id == updatedEntity.Id);
            if (log == null) return null;
            log.EventType = updatedEntity.EventType;
            log.TimeStamp = updatedEntity.TimeStamp;
            log.User = updatedEntity.User;

            _database.SaveChanges();
            return log;
        }
    
        public void Delete(string id)
        {
            var log = _database.Logs.FirstOrDefault(r => r.Id == id);
            if (log != null)
            {
                _database.Logs.Remove(log);
                _database.SaveChanges();
            }
            else
            {
                throw new Exception("Log not found for deletion");
            }
        }
    }
}