using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class CommunityProgram
{
    public int ProgramId { get; set; }

    public string? ProgramTitle { get; set; }

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<ProgramFeedbackSurvey> ProgramFeedbackSurveys { get; set; } = new List<ProgramFeedbackSurvey>();

    public virtual ICollection<UserProgramParticipation> UserProgramParticipations { get; set; } = new List<UserProgramParticipation>();
}
