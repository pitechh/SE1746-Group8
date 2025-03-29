using System;
using System.Collections.Generic;

namespace FLearning.EnrollmentService.Models;

public partial class CourseProgress
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    public decimal? ProgressPercentage { get; set; }

    public DateTime? LastAccessed { get; set; }

    public virtual Student Student { get; set; } = null!;
}
