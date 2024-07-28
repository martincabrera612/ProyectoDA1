using BusinessLogic.Controllers;
using BusinessLogic.Validators;
using Domain;
using Domain.Enums;
using UnitTests.BusinessLogicTests;

namespace UnitTests.DomainTests;

[TestClass]
public class BookingTest
{
     private Booking _booking = null!;
     
     [TestInitialize]
     public void init()
     {
          _booking = new Booking();
     }
          
     [TestMethod]
     [ExpectedException(typeof(InvalidOperationException))]
     public void ValidateFromBeforeToTest()
     {
          _booking.From = new DateTime(2024, 04, 06);
          _booking.To = new DateTime(2024, 04, 03);
          BookingValidator.ValidateDate(_booking);
     }

     [TestMethod]
     [ExpectedException(typeof(InvalidOperationException))]
     public void ApprovalDefaultsFalseTest()
     {
          _booking.Status = Status.Approved;
          BookingValidator.ValidateApprovalDefault(_booking);
     }

     [TestMethod]
     [ExpectedException(typeof(InvalidOperationException))]
     public void UserNotNullTest()
     {
          _booking._user = null;
          BookingValidator.ValidateUserNotNull(_booking);
     }
     
     [TestMethod]
     public void UserNotNullTestWhenUserNotNull()
     {
          User user = new User();
          _booking._user = user;
          BookingValidator.ValidateUserNotNull(_booking);
     }

     [TestMethod]
     [ExpectedException(typeof(InvalidOperationException))]
     public void DepositNotNullTest()
     {
          _booking._deposit = null;
          BookingValidator.ValidateDepositNotNull(_booking);
     }
     
     [TestMethod]
     public void DepositNotNullTestWhenDepositNotNull()
     {
          Deposit aDeposit = new Deposit();
          _booking._deposit = aDeposit;
          BookingValidator.ValidateDepositNotNull(_booking);
     }
     [TestMethod]
     public void PaymentNotNull()
     {
         Payment payment = new Payment(200);
         Assert.IsNotNull(payment);
     }
     
     [TestMethod]
     public void GetPriceTest()
     {
          const double expectedPrice = 150.5;
          _booking.Price = expectedPrice;
          var actualPrice = _booking.Price;
          Assert.AreEqual(expectedPrice,actualPrice);
     }

     [TestMethod]
     public void PaymentStatusTransitionTest()
     {
          double amount = 500;
          Payment payment = new Payment(amount);
          PaymentController paymentController = new PaymentController();
          Assert.AreEqual(PaymentStatus.Reserved, payment.Status);
          paymentController.CapturePayment(payment);
          Assert.AreEqual(PaymentStatus.Captured, payment.Status);
     }

     [TestMethod]
     public void SetPaymentTest()
     {
          var newPayment = new Payment(250);
          _booking.payment = newPayment;
          Assert.AreEqual(newPayment,_booking.payment);
     }

     [TestMethod]
     public void GetPaymentTest()
     {
          var newPayment = new Payment(250);
          _booking.payment = newPayment;
          Assert.IsNotNull(_booking.payment);
     }
}