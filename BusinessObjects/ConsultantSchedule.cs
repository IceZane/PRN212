using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class ConsultantSchedule
{
    public int ScheduleId { get; set; }

    public int? ConsultantId { get; set; }

    public DateOnly? AvailableDate { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public virtual Consultant? Consultant { get; set; }
}
