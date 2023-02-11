using Hangfire.Dashboard;

namespace CMS.Scheduler.Filters
{
    public class NoAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext dashboardContext)
        {
            return true;
        }
    }
}
