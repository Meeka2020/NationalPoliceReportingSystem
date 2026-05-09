using System;
using System.Collections.Generic;
using System.Text;



namespace NPRSApp.Api.Models
{
    public class PoliceReport
    {
        public int Id { get; set; }
        public string ReportNo { get; set; } = string.Empty;
        public string ReporterName { get; set; } = string.Empty;
        public string ReporterPhone { get; set; } = string.Empty;
        public string ReporterEmail { get; set; } = string.Empty;
        public string IncidentType { get; set; } = string.Empty;
        public DateTime IncidentDate { get; set; }
        public string IncidentLocation { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? ConsultationDate { get; set; }
        public TimeSpan? ConsultationTime { get; set; }
        public string Status { get; set; } = "Submitted";
        public DateTime CreatedOn { get; set; }
    }
}
