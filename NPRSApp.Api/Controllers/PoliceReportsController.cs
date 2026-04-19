using Microsoft.AspNetCore.Mvc;
using NPRSApp.Api.Models;

namespace NPRSApp.Api.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class PoliceReportsController : ControllerBase
    {
        // Temporary in-memory storage for demo/course purposes
        private static readonly List<PoliceReport> Reports = [];

        [HttpGet]
        public IActionResult GetAllReports()
        {
            return Ok(Reports);
        }

        [HttpGet("{id}")]
        public IActionResult GetReportById(int id)
        {
            var report = Reports.FirstOrDefault(r => r.Id == id);
            if (report == null)
                return NotFound();

            return Ok(report);
        }

        [HttpPost]
        public IActionResult CreateReport([FromBody] PoliceReport report)
        {
            report.Id = Reports.Count + 1;
            report.CreatedOn = DateTime.UtcNow;
            report.Status = "Submitted";

            Reports.Add(report);
            return Ok(report);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReport(int id, [FromBody] PoliceReport updatedReport)
        {
            var report = Reports.FirstOrDefault(r => r.Id == id);
            if (report == null)
                return NotFound();

            report.ReporterName = updatedReport.ReporterName;
            report.IncidentType = updatedReport.IncidentType;
            report.IncidentLocation = updatedReport.IncidentLocation;
            report.Description = updatedReport.Description;
            report.ConsultationDate = updatedReport.ConsultationDate;
            report.ConsultationTime = updatedReport.ConsultationTime;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReport(int id)
        {
            var report = Reports.FirstOrDefault(r => r.Id == id);
            if (report == null)
                return NotFound();

            Reports.Remove(report);
            return NoContent();
        }
    }
}