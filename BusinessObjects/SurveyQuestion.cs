using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class SurveyQuestion
{
    public int QuestionId { get; set; }

    public int? SurveyId { get; set; }

    public string? QuestionText { get; set; }

    public virtual Survey? Survey { get; set; }

    public virtual ICollection<SurveyAnswer> SurveyAnswers { get; set; } = new List<SurveyAnswer>();
}
