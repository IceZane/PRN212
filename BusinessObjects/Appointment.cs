using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int? UserId { get; set; }

    public int? ConsultantId { get; set; }

    public DateOnly? AppointmentDate { get; set; }

    public TimeOnly? TimeSlot { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Consultant? Consultant { get; set; }

    public virtual User? User { get; set; }
}
