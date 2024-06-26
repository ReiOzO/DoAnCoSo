﻿namespace BELibrary.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DetailRecord")]
    public partial class DetailRecord
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string DiseaseName { get; set; }

        public string Note { get; set; }

        public string Result { get; set; }

        public bool Status { get; set; }

        public Guid? DoctorId { get; set; }

        public Guid? FacultyId { get; set; }

        [Required]
        public int Process { get; set; }

        public Guid RecordId { get; set; }
        public bool IsMainRecord { get; set; }

        public virtual Record Record { get; set; }

        public virtual Faculty Faculty { get; set; }
    }
}