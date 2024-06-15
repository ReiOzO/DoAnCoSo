using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BELibrary.Entity
{
    [Table("PatientRecord")]
    public class PatientRecord
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime TestDate { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string ClinicalSymptoms { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; }
        public Guid? DoctorId { get; set; }
        public Guid RecordId { get; set; }
        public Guid PatientId { get; set; }
        public virtual Record Record { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}