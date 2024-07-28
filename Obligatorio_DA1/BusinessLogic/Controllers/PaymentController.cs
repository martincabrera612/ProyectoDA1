using Domain;
using Domain.Enums;
using Persistence;

namespace BusinessLogic.Controllers;

public class PaymentController
{
    private readonly IRepository<Payment> _paymentRepository;
    public void CapturePayment(Payment payment)
    {
        if (payment == null)
        {
            throw new ArgumentNullException(nameof(payment.Id));
        }
        else
        {
            payment.Status = PaymentStatus.Captured;
        }
    }
}