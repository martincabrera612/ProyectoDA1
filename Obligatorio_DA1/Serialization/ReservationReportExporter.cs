using System.Runtime.Serialization;
using Domain;

namespace Serialization
{
    public class ReservationReportExporter
    {
        public void ExportToTxt(IList<ReservationReport> reports, string filePath)
        {
            try
            {
                string? directoryPath = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var report in reports)
                    {
                        writer.WriteLine($"DEPOSITO: {report.DepositInfo}\tRESERVA: {report.ReservationInfo}\tPAGO: {report.PaymentStatus}");
                    }
                }

                Console.WriteLine($"TXT file successfully written to {Path.GetFullPath(filePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while exporting to TXT: {ex.Message}");
                throw new SerializationException("An error occurred while exporting to TXT", ex);
            }
        }

        public void ExportToCsv(IList<ReservationReport> reports, string filePath)
        {
            try
            {
                string? directoryPath = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("DEPOSITO,RESERVA,,,,,PAGO");
                    foreach (var report in reports)
                    {
                        writer.WriteLine($"{report.DepositInfo},{report.ReservationInfo},{report.PaymentStatus}");
                    }
                }

                Console.WriteLine($"CSV file successfully written to {Path.GetFullPath(filePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while exporting to CSV: {ex.Message}");
                throw new SerializationException("An error occurred while exporting to CSV", ex);
            }
        }
    }
}
