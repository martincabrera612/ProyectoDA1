using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ReviewRepository : IRepository<Review>
    {
        private SqlContext _database;

        public ReviewRepository(SqlContext database)
        {
            _database = database;
        }
    
        public Review Add(Review aReview)
        {
            var existingUser = _database.Users.Find(aReview.User.Id);
            if (existingUser != null)
            {
                _database.Users.Attach(existingUser);
                aReview.User = existingUser;
            }

            _database.Reviews.Add(aReview);
            _database.SaveChanges();
            return aReview;
        }
    
        public Review? Find(Func<Review, bool> filter)
        {
            return _database.Reviews.FirstOrDefault(filter);
        }
    
        public IList<Review> FindAll()
        {
            return _database.Reviews.Include(review => review.User).ToList();
        }
    
        public Review? Update(Review updatedEntity)
        {
            var review = _database.Reviews.FirstOrDefault(r => r.Id == updatedEntity.Id);
            if (review == null) return null;
            review.Rating = updatedEntity.Rating;
            review.Comment = updatedEntity.Comment;
            review.User = updatedEntity.User;
            review.Booking = updatedEntity.Booking;

            _database.SaveChanges();
            return review;
        }
    
        public void Delete(string id)
        {
            var review = _database.Reviews.FirstOrDefault(r => r.Id == id);
            if (review != null)
            {
                _database.Reviews.Remove(review);
                _database.SaveChanges();
            }
            else
            {
                throw new Exception("Review not found for deletion");
            }
        }
    }
}