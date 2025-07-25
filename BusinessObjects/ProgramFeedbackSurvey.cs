using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class ProgramFeedbackSurvey
{
    public int FeedbackId { get; set; }

    public int? UserId { get; set; }

    public int? ProgramId { get; set; }

    public int? PreSurveyScore { get; set; }

    public int? PostSurveyScore { get; set; }

    public string? Feedback { get; set; }

    public virtual CommunityProgram? Program { get; set; }

    public virtual User? User { get; set; }
}
