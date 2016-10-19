using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Its.Systems.HR.Test
{
    [TestClass]
    public class ActivityTests : BaseTest
    {
        private readonly IActivityManager _activityManager;
        private readonly ISessionManager _sessionManager;

        public ActivityTests() : base()
        {
            _activityManager = Container().Resolve<IActivityManager>();
            _sessionManager = Container().Resolve<ISessionManager>();
        }


        [TestMethod]
        public void GetActivityById2_ShouldReturnJavaOne()
        {
            var result = _activityManager.GetActivityById(2);

            Assert.AreEqual("JavaOne", result.Name);
        }

        [TestMethod]
        public void GetActivityById999_ShouldReturnNull()
        {
            var result = _activityManager.GetActivityById(999);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void AddNewActivity_ShouldMakeAllActivitesCountTo6()
        {
            var newActivity = new Activity()
            {
                Name = "NewActivity",
            };

            var result = _activityManager.AddActivity(newActivity);

            Assert.AreEqual(6, _activityManager.GetAllActivities().Count());
        }

        [TestMethod]
        public void AddNewActivityThatAlreadyExists_ShouldReturnFalse()
        {
            var newActivity = new Activity()
            {
                Name = "JavaOne",
            };

            var result = _activityManager.AddActivity(newActivity);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void EditJavaOneNameToEditedName_ShouldGetJavaOneNameInDbUpdated()
        {
            var result = _activityManager.EditActivity(2, "EDITEDNAME");

            Assert.AreEqual("EDITEDNAME", _activityManager.GetActivityById(2).Name);
        }

        [TestMethod]
        public void EditJavaOneToAirHack_ShouldNotUpdateInDb()
        {
            var result = _activityManager.EditActivity(2, "AirHack");

            Assert.AreEqual("JavaOne", _activityManager.GetActivityById(2).Name);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void DeleteJavaOne_ShouldReturnAllActivitiesCountOf4()
        {
            var result = _activityManager.DeleteActivityById(2);

            Assert.AreEqual(4, _activityManager.GetAllActivities().Count());
        }

        [TestMethod]
        public void DeleteJavaOneTwice_ShouldReturnFalseAndAllActivitesCountOf4()
        {
            _activityManager.DeleteActivityById(2);
            var result = _activityManager.DeleteActivityById(2);

            Assert.AreEqual(false, result);
            Assert.AreEqual(4, _activityManager.GetAllActivities().Count());
        }

        [TestMethod]
        public void ListAllActivities_ShouldReturnCountOf5()
        {
            var result = _activityManager.GetAllActivities().Count();

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void ListSessionsForJavaOne_ShouldReturnCountOf2()
        {
            var result = _sessionManager.GetAllSessionsForActivity(2).Count();

            Assert.AreEqual(2, result);
        }

        // ADD SESSIONS
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

        //[TestMethod]
        //public void AdminCanAddActivity()
        //{
        //    db = Container().Resolve<IDbRepository>();
        //    var p = db.Get<HrPerson>().SingleOrDefault(n => n.Id == 1);

        //    //p.AddActivity("DataHack");


        //    Assert.AreEqual(1, db.Get<Activity>().SingleOrDefault(n => n.Name == "DataHack").Id);
        //}
    }
}
