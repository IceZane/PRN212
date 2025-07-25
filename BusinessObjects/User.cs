using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class User
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public int? RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

    public virtual ICollection<CommunityProgram> CommunityPrograms { get; set; } = new List<CommunityProgram>();

    public virtual Consultant? Consultant { get; set; }

    public virtual ICollection<DashboardReport> DashboardReports { get; set; } = new List<DashboardReport>();

    public virtual ICollection<ProgramFeedbackSurvey> ProgramFeedbackSurveys { get; set; } = new List<ProgramFeedbackSurvey>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<TrainingCourse> TrainingCourses { get; set; } = new List<TrainingCourse>();

    public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();

    public virtual ICollection<UserProgramParticipation> UserProgramParticipations { get; set; } = new List<UserProgramParticipation>();

    public virtual ICollection<UserSurveyResult> UserSurveyResults { get; set; } = new List<UserSurveyResult>();
}
