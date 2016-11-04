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
        private readonly IUtilityManager _utilityManager;

        private const int Totalsessionsineffortdb = 4;

        public SessionTests() : base()
        {
            _activityManager = Container().Resolve<IActivityManager>();
            _sessionManager = Container().Resolve<ISessionManager>();
            _personManager = Container().Resolve<IPersonManager>();
            _utilityManager = Container().Resolve<IUtilityManager>();
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
        public void ListSessionsForJavaOne_ShouldReturnCountOf2()
        {
            var result = _sessionManager.GetAllSessionsForActivity(2).Count();

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void AddANewSessionToJavaOne_ShouldBeAddedToDb()
        {
            var result = new Session()
            {
                Name = "JavaOne 2017",
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(4),
                HrPersonId = 1,
                LocationId = 5,
                ActivityId = 2,
                SessionParticipants = null
            };

            _sessionManager.AddSession(result);

            Assert.AreEqual("JavaOne 2017", _sessionManager.GetSessionById(result.Id).Name);
        }

        [TestMethod]
        public void CreateASession_ShouldCreateSessionInDb()
        {
            var sessionToAdd = new Session()
            {
                ActivityId = 1,
                StartDate = null,
                EndDate = null,
                HrPersonId = null,
                LocationId = null,
                Name = "NewSession"
            };

            _sessionManager.AddSession(sessionToAdd);

            Assert.IsNotNull(_sessionManager.GetAllSessions().SingleOrDefault(n => n.Name == "NewSession"));
        }

        [TestMethod]
        public void EditASession_ShouldReturnExpected()
        {
            var sessionToEdit = _sessionManager.GetSessionById(1);

            sessionToEdit.Name = "EDITEDSESSIONNAME";
            _sessionManager.EditSession(sessionToEdit);

            Assert.AreEqual("EDITEDSESSIONNAME", _sessionManager.GetSessionById(1).Name);
        }

        [TestMethod]
        public void UseCase1_AddingActivityWithEverything_ShouldReturnExpected()
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

        [TestMethod]
        public void UseCase2_RemoveAllActivities_ShouldDeleteAllSessionParticipants()
        {
            // 1. DELETE ALL ACTIVITIES
            _activityManager.DeleteActivityById(1);
            _activityManager.DeleteActivityById(2);
            _activityManager.DeleteActivityById(3);
            _activityManager.DeleteActivityById(4);
            _activityManager.DeleteActivityById(5);

            // 2. GET THE COUNT FOR SESSIONPARTICIPANTS
            var resultCount = _personManager.GetAllSessionParticipants().Count();

            var expectedCount = 0;

            Assert.AreEqual(expectedCount, resultCount);
        }

        [TestMethod]
        public void UseCase3_RemoveAllSessions_ShouldDeleteAllSessionParticipants()
        {
            // 1. DELETE ALL SESSIONS
            _sessionManager.DeleteSessionById(1);
            _sessionManager.DeleteSessionById(2);
            _sessionManager.DeleteSessionById(3);
            _sessionManager.DeleteSessionById(4);
            
            // 2. GET THE COUNT FOR SESSIONPARTICIPANTS
            var resultCount = _personManager.GetAllSessionParticipants().Count();

            var expectedCount = 0;

            Assert.AreEqual(expectedCount, resultCount);
        }

        [TestMethod]
        public void UseCase4_RemoveSessionJavaOne2015_ShouldDeleteAllSessionTagsForJavaOne2015()
        {
            // 1. DELETE ALL SESSIONS
            _sessionManager.DeleteSessionById(1);

            // 2. GET THE COUNT FOR SESSIONTAGS
            var resultCount = _utilityManager.GetAllTagsForSessionById(1).Count();

            var expectedCount = 0;

            Assert.AreEqual(expectedCount, resultCount);
        }
    }
}
