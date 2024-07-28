using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace UnitTests;

[TestClass]
public class PaymentRepositoryTest
{
    private ServiceProvider _serviceProvider;
    private PaymentRepository _paymentRepository;


    [TestInitialize]
    public void Initialize()
    {
        var services = new ServiceCollection();
        services.AddDbContext<SqlContext>(options =>
            options.UseInMemoryDatabase("TestDb"));
        services.AddScoped<PaymentRepository>();
        _serviceProvider = services.BuildServiceProvider();
        _paymentRepository = _serviceProvider.GetRequiredService<PaymentRepository>();
    }

    [TestMethod]
    public void AddPaymentRepositoryTest()
    {
        double amount = 350;
        Payment payment = new Payment(amount);
        var addedPayment = _paymentRepository.Add(payment);
        Assert.IsNotNull(addedPayment);
        Assert.AreEqual(payment, addedPayment);
    }
    
    [TestMethod]
    public void FindPaymentRepositoryTest()
    {
        double amount = 350;
        Payment payment = new Payment(amount);
        _paymentRepository.Add(payment);
        var foundPayment = _paymentRepository.Find(p => p.Id == payment.Id);
        Assert.IsNotNull(foundPayment);
        Assert.AreEqual(payment.Id, foundPayment.Id);
    }
    
    [TestMethod]
    public void FindAllPaymentRepositoryTest()
    {
        double amount = 350;
        Payment payment1 = new Payment(amount);
        Payment payment2 = new Payment(amount+200);
        _paymentRepository.Add(payment1);
        _paymentRepository.Add(payment2);
        var foundPayments = _paymentRepository.FindAll();
        Assert.AreEqual(2, foundPayments.Count);
        Assert.IsTrue(foundPayments.Contains(payment1));
        Assert.IsTrue(foundPayments.Contains(payment2));
    }

    [TestMethod]
    public void UpdatePaymentRepositoryTest()
    {
        double amount = 350;
        Payment payment = new Payment(amount);
        _paymentRepository.Add(payment);
        payment.Status = PaymentStatus.Captured;
        var updatedPayment = _paymentRepository.Update(payment);
        Assert.IsNotNull(updatedPayment);
        Assert.AreEqual(payment.Id, updatedPayment.Id);
        Assert.AreEqual(PaymentStatus.Captured, updatedPayment.Status);
    }

    [TestMethod]
    public void DeletePaymentRepositoryTest()
    {
        double amount = 350;
        Payment payment = new Payment(amount);
        _paymentRepository.Add(payment);
        _paymentRepository.Delete(payment.Id);
        var foundPayment = _paymentRepository.Find(p => p.Id == payment.Id);
        Assert.IsNull(foundPayment);
    }
    
    [TestMethod]
    [ExpectedException(typeof(Exception), "Payment not found for deletion")]
    public void DeletePaymentNotFoundRepositoryTest()
    {
        const string nonExistingId = "0aaab648-464f-4d2a-b18b-c064929ce244";
        _paymentRepository.Delete(nonExistingId);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        var context = _serviceProvider.GetService<SqlContext>();
            
        context.Payments.RemoveRange(context.Payments);
            
        context.SaveChanges();
            
        _serviceProvider.Dispose();
    }
}