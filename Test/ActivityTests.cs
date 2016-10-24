using System;
using System.Linq;
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
        public void GetActivityByInvalidId_ShouldReturnNull()
        {
            var result = _activityManager.GetActivityById(999);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void AddNewActivity_ShouldAddThatToDb()
        {
            var newActivity = new Activity()
            {
                Name = "NewActivity",
            };

            var result = _activityManager.AddActivity(newActivity);

            Assert.AreEqual(true, result);
            Assert.AreEqual("NewActivity", _activityManager.GetAllActivities().SingleOrDefault(n => n.Name == "NewActivity").Name);
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
        public void EditJavaOneNameToEditedName_ShouldUpdateNameInDB()
        {
            var result = _activityManager.EditActivity(2, "EDITEDNAME");

            Assert.AreEqual(true, result);
            Assert.AreEqual("EDITEDNAME", _activityManager.GetActivityById(2).Name);
        }

        [TestMethod]
        public void EditActivityToExistingName_ShouldNotUpdateInDB()
        {
            var result = _activityManager.EditActivity(2, "AirHack");

            Assert.AreEqual(false, result);
            Assert.AreEqual("JavaOne", _activityManager.GetActivityById(2).Name);
        }

        [TestMethod]
        public void DeleteJavaOne_ShouldNotExistInDb()
        {
            var result = _activityManager.DeleteActivityById(2);

            Assert.AreEqual(true, result);
            Assert.AreEqual(null, _activityManager.GetActivityById(2));
        }

        [TestMethod]
        public void DeleteJavaOneTwice_ShouldReturnFalseAndJavaOneDeletedFromDB()
        {
            _activityManager.DeleteActivityById(2);
            var result = _activityManager.DeleteActivityById(2);

            Assert.AreEqual(false, result);
            Assert.AreEqual(null, _activityManager.GetActivityById(2));
        }

        [TestMethod]
        public void ListAllActivities_ShouldReturnCountOf5()
        {
            var result = _activityManager.GetAllActivities().Count();

            Assert.AreEqual(5, result);
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
