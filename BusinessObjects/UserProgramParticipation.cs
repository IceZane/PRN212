using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class UserProgramParticipation
{
    public int ParticipationId { get; set; }

    public int? UserId { get; set; }

    public int? ProgramId { get; set; }

    public DateTime? JoinedAt { get; set; }

    public virtual CommunityProgram? Program { get; set; }

    public virtual User? User { get; set; }
}
