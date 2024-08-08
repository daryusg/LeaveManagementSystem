
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.Periods
{
    public class PeriodsService(ApplicationDbContext _context) : IPeriodsService
    {
        public async Task<Period> GetCurrentPeriodAsync()
        {
            //get the current period based on the year
            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            return period;
        }
    }
}
