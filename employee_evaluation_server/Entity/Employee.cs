using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entity
{
    public partial class Employee
    {
        public string EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int JobCode { get; set; }
        public string JobName { get; set; }
        public int SchoolCode { get; set; }
        public string SchoolName { get; set; }
        public string SuperiorId { get; set; }
        public bool IsSchoolManager { get; set; }
        public bool IsSuperior { get; set; }
        public bool IsGeneralManager { get; set; }
        public string? SupervisorName { get; set; }
        public int CurrentYear { get; set; }
    }
}
