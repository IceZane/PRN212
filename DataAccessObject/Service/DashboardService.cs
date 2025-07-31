using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject.Service
{
    public class DashboardService
    {
        private readonly DrugUsePreventionSupportSystemContext _context;

        public DashboardService()
        {
            _context = new DrugUsePreventionSupportSystemContext();
        }

        public List<DashboardReport> GetReports()
        {
            return _context.DashboardReports.OrderByDescending(r => r.GeneratedAt).ToList();
        }
    }
}
