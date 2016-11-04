using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class RatingStatisticsViewModel
    {
        public Session Session { get; set; }
        public double Rating { get; set; }
        public int NoOfRatings { get; set; }
    }
}