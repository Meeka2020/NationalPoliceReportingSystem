using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace NPRSApp.Maui.Model
{
    public class PoliceReport
    {
        public int Id { get; set; }

        public string ReportNo { get; set; } = string.Empty;
        public string ReporterName { get; set; } = string.Empty;
        public string ReporterPhone { get; set; } = string.Empty;
        public string ReporterEmail { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;

        public string IncidentType { get; set; } = string.Empty;
        public DateTime IncidentDate { get; set; } = DateTime.Today;
        public string IncidentLocation { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Make these nullable if they are optional in your form/flow.
        public DateTime? ConsultationDate { get; set; }  // no default -> null
        public TimeSpan? ConsultationTime { get; set; }  // no default -> null

        public string Status { get; set; } = "Submitted";
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }


}
