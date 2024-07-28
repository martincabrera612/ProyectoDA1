using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DepositRepository : IRepository<Deposit>
    {
        private SqlContext _database;

        public DepositRepository(SqlContext database)
        {
            _database = database;
        }

        public Deposit Add(Deposit aDeposit)
        {
            _database.Deposits.Add(aDeposit);
            _database.SaveChanges();
            return aDeposit;
        }

        public Deposit? Find(Func<Deposit, bool> filter)
        {
            return _database.Deposits.FirstOrDefault(filter);
        }

        public IList<Deposit> FindAll()
        {
            return _database.Deposits.Include(d => d.Availabilities).Include(d => d.Bookings).ToList();
        }

        public Deposit? Update(Deposit updatedEntity)
        {
            var deposit = _database.Deposits.FirstOrDefault(d => d.Id == updatedEntity.Id);
            if (deposit == null) return null;
            deposit.Area = updatedEntity.Area;
            deposit.Size = updatedEntity.Size;
            deposit.AirConditioning = updatedEntity.AirConditioning;
            deposit.Promotions = updatedEntity.Promotions;

            _database.SaveChanges();
            return deposit;
        }

        public void Delete(string id)
        {
            var deposit = _database.Deposits.FirstOrDefault(d => d.Id == id);
            if (deposit != null)
            {
                _database.Deposits.Remove(deposit);
                _database.SaveChanges();
            }
            else
            {
                throw new Exception("Deposit not found for deletion");
            }
        }
    }
}