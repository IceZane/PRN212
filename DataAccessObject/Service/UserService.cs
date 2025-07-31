using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject.Service
{
    public class UserService
    {
        private readonly DrugUsePreventionSupportSystemContext _context;

        public UserService()
        {
            _context = new DrugUsePreventionSupportSystemContext();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.Include(u => u.Role).ToList();
        }
        public async Task<List<User>> GetAllUsersWithDetailsAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.UserSurveyResults).ThenInclude(r => r.Survey)
                .Include(u => u.UserCourses).ThenInclude(uc => uc.Course)
                .Include(u => u.UserProgramParticipations).ThenInclude(p => p.Program)
                .ToListAsync();
        }
    }
}
