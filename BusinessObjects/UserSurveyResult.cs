using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class UserSurveyResult
{
    public int ResultId { get; set; }

    public int? UserId { get; set; }

    public int? SurveyId { get; set; }

    public int? TotalScore { get; set; }

    public string? Recommendation { get; set; }

    public DateTime? TakenAt { get; set; }

    public virtual Survey? Survey { get; set; }

    public virtual User? User { get; set; }
}
