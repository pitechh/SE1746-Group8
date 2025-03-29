using System;
using System.Collections.Generic;

namespace FLearning.EnrollmentService.Models;

public partial class Enrollment
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public int CourseId { get; set; }

    public DateTime? EnrolledDate { get; set; }

    public string? Status { get; set; }

    public virtual Student Student { get; set; } = null!;
}
