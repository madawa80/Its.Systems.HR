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
        private IActivityManager impl;

        public ActivityTests() : base()
        { }


        [TestMethod]
        public void GetActivityJavaOne_ShouldReturnThatActivity()
        {
            impl = Container().Resolve<IActivityManager>();

            var result = impl.GetActivityById(2);

            Assert.AreEqual("JavaOne", result.Name);
        }

        [TestMethod]
        public void AddNewActivity_ShouldMakeAllActivitesCountTo6()
        {
            impl = Container().Resolve<IActivityManager>();

            var newActivity = new Activity()
            {
                Name = "NewActivity",
            };

            var result = impl.AddActivity(newActivity);

            Assert.AreEqual(6, impl.GetAllActivities().Count());
        }

        [TestMethod]
        public void EditJavaOneNameToEditedName_ShouldGetJavaOneNameInDbUpdated()
        {
            impl = Container().Resolve<IActivityManager>();

            var activityFromDb = impl.GetActivityById(2);

            activityFromDb.Name = "EDITEDNAME";

            var result = impl.EditActivity(activityFromDb);

            Assert.AreEqual("EDITEDNAME", impl.GetActivityById(2).Name);
        }

        [TestMethod]
        public void DeleteJavaOne_ShouldReturnAllActivitiesCountOf4()
        {
            impl = Container().Resolve<IActivityManager>();

            var result = impl.DeleteActivity(2);

            Assert.AreEqual(4, impl.GetAllActivities().Count());
        }

        [TestMethod]
        public void DeleteJavaOneTwice_ShouldReturnFalseAndAllActivitesCountOf4()
        {
            impl = Container().Resolve<IActivityManager>();

            impl.DeleteActivity(2);
            var result = impl.DeleteActivity(2);

            Assert.AreEqual(false, result);
            Assert.AreEqual(4, impl.GetAllActivities().Count());
        }



        [TestMethod]
        public void ListAllActivities_ShouldReturnCountOf5()
        {
            impl = Container().Resolve<IActivityManager>();

            var result = impl.GetAllActivities().Count();

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void ListSessionsForJavaOne_ShouldReturnCountOf2()
        {
            impl = Container().Resolve<IActivityManager>();

            var result = impl.GetAllSessionsForActivity(2).Count();

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetAllParticipantsForSessionJavaOne2015_ShouldReturnCountOf3()
        {
            impl = Container().Resolve<IActivityManager>();

            var result = impl.GetAllParticipantsForSession(1).Count();

            Assert.AreEqual(3, result);
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
