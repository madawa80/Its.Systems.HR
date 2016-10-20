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
    public class SessionTests : BaseTest
    {
        private readonly IActivityManager _activityManager;
        private readonly ISessionManager _sessionManager;
        private readonly IPersonManager _personManager;

        private const int Totalsessionsineffortdb = 4;

        public SessionTests() : base()
        {
            _activityManager = Container().Resolve<IActivityManager>();
            _sessionManager = Container().Resolve<ISessionManager>();
            _personManager = Container().Resolve<IPersonManager>();
        }

        [TestMethod]
        public void GetAllSessions_ShouldReturnExpectedCount()
        {
            var result = _sessionManager.GetAllSessions();
            var resultCount = result.Count();

            var expected = Totalsessionsineffortdb;

            Assert.AreEqual(expected, resultCount);
        }

        [TestMethod]
        public void CreateASession_ShouldReturnCountPlusOne()
        {
            var sessionToAdd = new Session()
            {
                ActivityId = 1,
                StartDate = null,
                EndDate = null,
                HrPersonId = null,
                LocationId = null,
            };

            _sessionManager.AddSession(sessionToAdd);

            var resultCount = _sessionManager.GetAllSessions().Count();

            var expected = Totalsessionsineffortdb + 1;

            Assert.AreEqual(expected, resultCount);
        }

        [TestMethod]
        public void UseCase1_ShouldReturnExpected()
        {
            // 1. SKAPA AKTIVITET
            var activityToAdd = new Activity()
            {
                Name = "NewActivity",
            };

            _activityManager.AddActivity(activityToAdd);    //returns bool

            // 2. SKAPA SESSION
            var sessionToAdd = new Session()
            {
                ActivityId = activityToAdd.Id,
                StartDate = null,
                EndDate = null,
                HrPersonId = null,
                LocationId = null,
            };

            _sessionManager.AddSession(sessionToAdd);   //returns void!!

            // 3. LÄGG TILL DELTAGARE
            _personManager.AddParticipantToSession(1, sessionToAdd.Id);
            _personManager.AddParticipantToSession(2, sessionToAdd.Id);
            _personManager.AddParticipantToSession(3, sessionToAdd.Id);

            // 4. SPARA KOMMENTAR OCH UTVÄRDERING
            _sessionManager.SaveCommentsForSession(sessionToAdd.Id, "New Comment");
            _sessionManager.SaveEvaluationForSession(sessionToAdd.Id, "New Eval");

            // 5. TA BORT DELTAGARE
            _personManager.RemoveParticipantFromSession(3, sessionToAdd.Id);

            // TEST RESULTS
            var expectedParticipantsCount = 2;

            Assert.AreEqual(expectedParticipantsCount, _personManager.GetAllParticipantsForSession(sessionToAdd.Id).Count());
            Assert.AreEqual("New Comment", _sessionManager.GetSessionById(sessionToAdd.Id).Comments);
            Assert.AreEqual("New Eval", _sessionManager.GetSessionById(sessionToAdd.Id).Evaluation);
        }


    }
}
