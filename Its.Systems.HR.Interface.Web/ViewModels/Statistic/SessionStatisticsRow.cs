using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class SessionStatisticsRow
    {
        public Session Session { get; set; }
        public int NumberOfParticipants { get; set; }
    }
}