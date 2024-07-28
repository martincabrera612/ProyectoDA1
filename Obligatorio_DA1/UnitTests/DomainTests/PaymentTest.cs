using System.Runtime.CompilerServices;
using Domain;
using Domain.Enums;

namespace UnitTests.DomainTests;


[TestClass]
public class PaymentTest
{
    private Payment _payment;
    private double amount;

    [TestInitialize]
    public void Initialize()
    {
        amount = 1000; 
        _payment = new Payment(amount);
    }
    
    [TestMethod]
    public void DefaultPaymentStatusIsReservedTest()
    {
        Payment payment = new Payment(amount);
        Assert.AreEqual(PaymentStatus.Reserved, payment.Status );
    }

    [TestMethod]
    public void PaymentNotNull()
    {
        Assert.IsNotNull(_payment);
    }
    

    [TestMethod]
    public void PaymentConstructorTest()
    {
        double expectedAmount = 500; 
        DateTime beforeCreation = DateTime.Today;
        Payment payment = new Payment(expectedAmount);
        
        Assert.AreEqual(beforeCreation , payment.PaymentDate);
        Assert.AreEqual(expectedAmount, payment.Amount);
    }

    [TestMethod]
    public void PaymentIdNotNullOrWhiteSpace()
    {
        Assert.IsFalse(string.IsNullOrWhiteSpace(_payment.Id));
    }
}   