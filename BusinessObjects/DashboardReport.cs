using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class DashboardReport
{
    public int ReportId { get; set; }

    public string? ReportType { get; set; }

    public string? Data { get; set; }

    public int? GeneratedBy { get; set; }

    public DateTime? GeneratedAt { get; set; }

    public virtual User? GeneratedByNavigation { get; set; }
}
