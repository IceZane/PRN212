using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject.Service
{
    public class CommunityProgramService
    {
        private readonly DrugUsePreventionSupportSystemContext _context;

        public CommunityProgramService()
        {
            _context = new DrugUsePreventionSupportSystemContext();
        }

        public List<CommunityProgram> GetAllPrograms()
        {
            return _context.CommunityPrograms
                           .OrderByDescending(p => p.StartDate)
                           .ToList();
        }
        public CommunityProgram? GetProgramById(int programId)
        {
            return _context.CommunityPrograms.Find(programId);
        }

        // Add new program
        public bool AddProgram(CommunityProgram program)
        {
            try
            {
                _context.CommunityPrograms.Add(program);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
            
        // Update existing program
        public bool UpdateProgram(CommunityProgram program)
        {
            try
            {
                var existingProgram = _context.CommunityPrograms.Find(program.ProgramId);
                if (existingProgram != null)
                {
                    existingProgram.ProgramTitle = program.ProgramTitle;
                    existingProgram.Description = program.Description;
                    existingProgram.StartDate = program.StartDate;
                    existingProgram.EndDate = program.EndDate;

                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Delete program
        public bool DeleteProgram(int programId)
        {
            try
            {
                var program = _context.CommunityPrograms.Find(programId);
                if (program != null)
                {
                    _context.CommunityPrograms.Remove(program);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Check if program exists
        public bool ProgramExists(int programId)
        {
            return _context.CommunityPrograms.Any(p => p.ProgramId == programId);
        }

        // Get programs by date range
        public List<CommunityProgram> GetProgramsByDateRange(DateOnly? startDate, DateOnly? endDate)
        {
            var query = _context.CommunityPrograms.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(p => p.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.EndDate <= endDate.Value);
            }

            return query.OrderByDescending(p => p.StartDate).ToList();
        }

        // Dispose context
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
