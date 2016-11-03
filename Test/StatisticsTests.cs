using System;
using System.Collections.Generic;
using System.Linq;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Its.Systems.HR.Test
{
    [TestClass]
    public class StatisticsTests : BaseTest
    {
        private readonly IUtilityManager _utilityManager;
        private readonly IPersonManager _personManager;
        private readonly ISessionManager _sessionManager;

        public StatisticsTests() : base()
        {
            _utilityManager = Container().Resolve<IUtilityManager>();
            _personManager = Container().Resolve<IPersonManager>();
            _sessionManager = Container().Resolve<ISessionManager>();
        }


        [TestMethod]
        public void TopRatedSession_ShouldBeJavaOne2015()
        {
            var allSessionsWithReviews = _sessionManager.GetAllSessionsWithReviews().ToList();

            var ratingStatistics = new List<RatingStatistics>();
            foreach (var session in allSessionsWithReviews)
            {
                ratingStatistics.Add(new RatingStatistics()
                {
                    Session = session,
                    Rating = _utilityManager.GetRatingForSessionById(session.Id)
                });
            }

            var topRatedSession = ratingStatistics.OrderByDescending(n => n.Rating).Take(1).SingleOrDefault();

            var expected = _sessionManager.GetSessionById(1);

            Assert.AreEqual(expected, topRatedSession.Session);
        }
    }

    public class RatingStatistics
    {
        public Session Session { get; set; }
        public double Rating { get; set; }
    }
}
