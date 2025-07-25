using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class SurveyAnswer
{
    public int AnswerId { get; set; }

    public int? QuestionId { get; set; }

    public string? AnswerText { get; set; }

    public int? Score { get; set; }

    public virtual SurveyQuestion? Question { get; set; }
}
