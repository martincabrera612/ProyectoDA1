using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class BookingRepository : IRepository<Booking>
    {
        private SqlContext _database;

        public BookingRepository(SqlContext database)
        {
            _database = database;
        }

        public Booking Add(Booking aBooking)
        {
            var existingUser = _database.Users.Find(aBooking._user.Id);
            if (existingUser != null)
            {
                _database.Users.Attach(existingUser);
                aBooking._user = existingUser;
            }

            _database.Bookings.Add(aBooking);
            _database.SaveChanges();
            return aBooking;
        }

        public Booking? Find(Func<Booking, bool> filter)
        {
            return _database.Bookings.FirstOrDefault(filter);
        }

        public IList<Booking> FindAll()
        {
            return _database.Bookings.Include(booking => booking._user).
                Include(booking => booking._deposit).
                Include(booking => booking.payment).
                ToList();
        }

        public Booking? Update(Booking updatedEntity)
        {
            var booking = _database.Bookings.FirstOrDefault(b => b.Id == updatedEntity.Id);
            if (booking == null) return null;
            booking.From = updatedEntity.From;
            booking.To = updatedEntity.To;
            booking.Status = updatedEntity.Status;
            booking._user = updatedEntity._user;
            booking._deposit = updatedEntity._deposit;
            booking.Price = updatedEntity.Price;
            booking.RejectionReason = updatedEntity.RejectionReason;
            booking.payment = updatedEntity.payment;

            _database.SaveChanges();
            return booking;
        }

        public void Delete(string id)
        {
            var booking = _database.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                _database.Bookings.Remove(booking);
                _database.SaveChanges();
            }
            else
            {
                throw new Exception("Booking not found for deletion");
            }
        }
    }
}