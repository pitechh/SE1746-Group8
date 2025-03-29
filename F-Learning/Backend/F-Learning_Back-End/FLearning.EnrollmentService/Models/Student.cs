using System;
using System.Collections.Generic;

namespace FLearning.EnrollmentService.Models;

public partial class Student
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime? EnrolledAt { get; set; }

    public int? TotalCoursesEnrolled { get; set; }

    public int? CompletedCourses { get; set; }

    public virtual ICollection<CourseProgress> CourseProgresses { get; set; } = new List<CourseProgress>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
