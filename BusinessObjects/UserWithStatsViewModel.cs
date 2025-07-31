using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class UserWithStatsViewModel
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string? RoleName { get; set; }

        public int TotalSurveys { get; set; }
        public int TotalCourses { get; set; }
        public int TotalPrograms { get; set; }
    }
}
