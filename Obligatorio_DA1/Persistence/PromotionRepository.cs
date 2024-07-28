using Domain;

namespace Persistence
{
    public class PromotionRepository : IRepository<Promotion>
    {
        private SqlContext _database;

        public PromotionRepository(SqlContext database)
        {
            _database = database;
        }

        public Promotion Add(Promotion aPromotion)
        {
            _database.Promotions.Add(aPromotion);
            _database.SaveChanges();
            return aPromotion;
        }

        public Promotion? Find(Func<Promotion, bool> filter)
        {
            return _database.Promotions.FirstOrDefault(filter);
        }

        public IList<Promotion> FindAll()
        {
            return _database.Promotions.ToList();
        }

        public Promotion? Update(Promotion updatedEntity)
        {
            var promotion = _database.Promotions.FirstOrDefault(p => p.Id == updatedEntity.Id);
            if (promotion == null) return null;
            promotion.Label = updatedEntity.Label;
            promotion.Percentage = updatedEntity.Percentage;
            promotion.From = updatedEntity.From;
            promotion.To = updatedEntity.To;

            _database.SaveChanges();
            return promotion;
        }

        public void Delete(string id)
        {
            var toDelete = _database.Promotions.FirstOrDefault(p => p.Id == id);

            if (toDelete != null)
            {
                _database.Promotions.Remove(toDelete);
                _database.SaveChanges();
            }
            else
            {
                throw new Exception("Promotion not found for deletion");
            }
        }
    }
}