using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entity
{
    public partial class OutstandingEmployeesSchool
    {
        public string EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string JobName { get; set; }
        public int SchoolCode { get; set; }
        public string SchoolName { get; set; }
        public string SuperiorId { get; set; }
        public string? SupervisorName { get; set; }
        public int? CurrentYear { get; set; }
        public int WeightedMeasureGrade { get; set; }
        public string GeneralEvaluation { get; set; }
        public string? EvaluationDocument1 { get; set; }
        public string? EvaluationDocument2 { get; set; }
        public string? EvaluationDocument3 { get; set; }
        public int? EvaluationStatusCode { get; set; }
        public int? OutstandingEmployeeRating { get; set; }
        public string? UniqueInitiative { get; set; }
        public string? ReasonSelectedRating { get; set; }
        public string? ParticipateRatingDecision { get; set; }
        public string? SuperiorName { get; set; }
    }
}
