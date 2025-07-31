using BusinessObjects;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObject
{
    public class TrainingCourseDAO
    {
        private readonly DrugUsePreventionSupportSystemContext _context;

        public TrainingCourseDAO()
        {
            _context = new DrugUsePreventionSupportSystemContext();
        }

        public List<TrainingCourse> GetAllCourses()
        {
            return _context.TrainingCourses.OrderByDescending(c => c.CreatedAt).ToList();
        }

        public List<TrainingCourse> SearchCourses(string keyword)
        {
            return _context.TrainingCourses
                .Where(c => c.Title.Contains(keyword) || c.Description.Contains(keyword))
                .ToList();
        }

        public List<TrainingCourse> FilterByAudience(string audience)
        {
            return _context.TrainingCourses
                .Where(c => c.TargetAudience == audience)
                .ToList();
        }
    }
}
