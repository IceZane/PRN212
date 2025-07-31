using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject
{
    public class UserCourseDAO
    {
        private readonly DrugUsePreventionSupportSystemContext _context;
        public UserCourseDAO()
        {
            _context = new DrugUsePreventionSupportSystemContext();
        }
        public List<UserCourse> GetUserCoursesByUserId(User user)
        {
            var userId = user?.UserId;
            return _context.UserCourses
                .Include(uc => uc.Course)
                .Include(uc => uc.User)
                .Where(uc => uc.UserId == userId)
                .ToList();
        }
    }
}
