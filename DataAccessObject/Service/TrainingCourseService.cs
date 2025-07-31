using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject.Service
{

    public class TrainingCourseService
    {
        private readonly DrugUsePreventionSupportSystemContext _context;

        public TrainingCourseService()
        {
            _context = new DrugUsePreventionSupportSystemContext();
        }

        public List<TrainingCourse> GetAllCourses()
        {
            return _context.TrainingCourses
                           .OrderByDescending(c => c.CreatedAt)
                           .ToList();
        }

        public TrainingCourse GetCourseById(int id)
        {
            return _context.TrainingCourses.FirstOrDefault(c => c.CourseId == id);
        }

        public void AddCourse(TrainingCourse course)
        {
            _context.TrainingCourses.Add(course);
            _context.SaveChanges();
        }

        public void UpdateCourse(TrainingCourse course)
        {
            _context.TrainingCourses.Update(course);
            _context.SaveChanges();
        }

        public void DeleteCourse(TrainingCourse course)
        {
            _context.TrainingCourses.Remove(course);
            _context.SaveChanges();
        }
    }
}
