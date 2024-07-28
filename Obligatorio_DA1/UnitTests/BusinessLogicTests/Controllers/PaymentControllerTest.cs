using BusinessLogic.Controllers;
using Domain;
using Domain.Enums;

namespace UnitTests.BusinessLogicTests;

[TestClass]
public class PaymentControllerTest
{
    private PaymentController _paymentController;

    [TestInitialize]
    public void Initialize()
    {
        _paymentController = new PaymentController();
        
    }

    [TestMethod]
    public void PaymentStatusChangesToCapturedTest()
    {
        Payment payment = new Payment(200);
        _paymentController.CapturePayment(payment);
        Assert.AreEqual(PaymentStatus.Captured, payment.Status);
    }
    
}