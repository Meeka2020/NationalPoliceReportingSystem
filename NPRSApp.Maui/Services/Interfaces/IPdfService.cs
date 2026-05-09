using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPRSApp.Maui.Services.Interfaces
{
    public interface IPdfService
    {
        Task<string> GenerateReportPdfAsync(object reportData);
        Task<string> GenerateReceiptPdfAsync(object receiptData);
    }
}