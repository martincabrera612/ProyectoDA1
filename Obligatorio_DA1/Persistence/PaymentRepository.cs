using System.Linq.Dynamic.Core;
using Domain;

namespace Persistence;

public class PaymentRepository : IRepository<Payment>
{
    
    private SqlContext _database;

    public PaymentRepository(SqlContext database)
    {
        _database = database;
    }
    public Payment Add(Payment payment)
    {
        _database.Payments.Add(payment);
        _database.SaveChanges();
        return payment;
    }

    public Payment? Find(Func<Payment, bool> filter)
    {
        return _database.Payments.FirstOrDefault(filter);
    }

    public IList<Payment> FindAll()
    {
        return _database.Payments.ToList();
    }

    public Payment? Update(Payment updatedPayment)
    {
        var payment = _database.Payments.FirstOrDefault(p => p.Id == updatedPayment.Id);
        if (payment == null) return null;
        payment.Status = updatedPayment.Status;
        payment.Amount = updatedPayment.Amount;
        payment.PaymentDate = updatedPayment.PaymentDate;
        _database.SaveChanges();
        return payment;
    }

    public void Delete(string id)
    {
        var PaymentToDelete = _database.Payments.FirstOrDefault(p => p.Id == id);
        if (PaymentToDelete != null)
        {
            _database.Payments.Remove(PaymentToDelete);
            _database.SaveChanges();
        }
        else
        {
            throw new Exception("Payment not found for deletion");
        }
    }
}