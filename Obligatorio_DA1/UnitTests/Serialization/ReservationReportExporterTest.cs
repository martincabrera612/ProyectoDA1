using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Domain;
using Serialization;

namespace UnitTests.SerializationTests
{
    [TestClass]
    public class ReservationReportExporterTests
    {
        private ReservationReportExporter _exporter;
        private List<ReservationReport> _reports;
        private string _txtFilePath;
        private string _csvFilePath;

        [TestInitialize]
        public void SetUp()
        {
            _exporter = new ReservationReportExporter();
            _reports = new List<ReservationReport>
            {
                new ReservationReport { DepositInfo = "Dep1", ReservationInfo = "Res1", PaymentStatus = "Paid" },
                new ReservationReport { DepositInfo = "Dep2", ReservationInfo = "Res2", PaymentStatus = "Unpaid" }
            };
            _txtFilePath = Path.Combine(Path.GetTempPath(), "report.txt");
            _csvFilePath = Path.Combine(Path.GetTempPath(), "report.csv");
        }

        [TestCleanup]
        public void TearDown()
        {
            if (File.Exists(_txtFilePath))
                File.Delete(_txtFilePath);

            if (File.Exists(_csvFilePath))
                File.Delete(_csvFilePath);
        }

        [TestMethod]
        public void ExportToTxt_CreatesFileWithExpectedContent()
        {
            _exporter.ExportToTxt(_reports, _txtFilePath);

            Assert.IsTrue(File.Exists(_txtFilePath));

            var lines = File.ReadAllLines(_txtFilePath);
            Assert.AreEqual(2, lines.Length);
            Assert.AreEqual("DEPOSITO: Dep1\tRESERVA: Res1\tPAGO: Paid", lines[0]);
            Assert.AreEqual("DEPOSITO: Dep2\tRESERVA: Res2\tPAGO: Unpaid", lines[1]);
        }

        [TestMethod]
        public void ExportToCsv_CreatesFileWithExpectedContent()
        {
            _exporter.ExportToCsv(_reports, _csvFilePath);

            Assert.IsTrue(File.Exists(_csvFilePath));

            var lines = File.ReadAllLines(_csvFilePath);
            Assert.AreEqual(3, lines.Length);
            Assert.AreEqual("DEPOSITO,RESERVA,,,,,PAGO", lines[0]);
            Assert.AreEqual("Dep1,Res1,Paid", lines[1]);
            Assert.AreEqual("Dep2,Res2,Unpaid", lines[2]);
        }
    }
}
