﻿using PortalTeme.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortalTeme.Data.Models {
    public class Course {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public AcademicYear Year { get; set; }

        [Required]
        public User Professor { get; set; }

    }

    public class AcademicYear {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

    }

    public class Assignment {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime LastUpdated { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }

    public class CourseAssignment {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Course Course { get; set; }

        [Required]
        public User Professor { get; set; }

        public List<User> Assistants { get; set; }

        public List<User> Students { get; set; }

        public List<Assignment> Assignments { get; set; }
    }

    public enum AssignmentEntryState {
        Submitted,
        Reviewed,
        Graded
    }

    public class AssignmentEntry {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public CourseAssignment CourseAssignment { get; set; }

        [Required]
        public User Student { get; set; }

        [Required]
        public AssignmentEntryState State { get; set; }

        public int Grading { get; set; }
    }

    public class AssignmentEntryVersion {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public AssignmentEntry AssignmentEntry { get; set; }

        public List<AssignmentEntryFile> Files { get; set; }

        public DateTime DateAdded { get; set; }
    }

    // This may be temporary
    public enum AssignmentEntryFileType {
        SourceCode,
        Project,
        Essay
    }

    public class AssignmentEntryFile {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public AssignmentEntryFileType FileType { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class AssignmentExtensionRequest {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Assignment Assignment { get; set; }

        [Required]
        public User Student { get; set; }

        [Required]
        public string Reason { get; set; }

        public bool Approved { get; set; }
    }
}
