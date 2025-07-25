using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Survey
{
    public int SurveyId { get; set; }

    public string? SurveyName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<SurveyQuestion> SurveyQuestions { get; set; } = new List<SurveyQuestion>();

    public virtual ICollection<UserSurveyResult> UserSurveyResults { get; set; } = new List<UserSurveyResult>();
}
