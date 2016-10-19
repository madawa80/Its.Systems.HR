using System;
using System.Linq;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Its.Systems.HR.Test
{
    [TestClass]
    public class PersonTests : BaseTest
    {
        private readonly IPersonManager _personManager;

        public PersonTests() : base()
        {
            _personManager = Container().Resolve<IPersonManager>();
        }
         
        [TestMethod]
        public void GetAllParticipants_ShouldReturnExpected()
        {
            var allParticipants = _personManager.GetAllParticipants().ToList();

            var expectedCount = 8;

            Assert.AreEqual(expectedCount, allParticipants.Count);
        }

        [TestMethod]
        public void GetAllParticipantsForSessionJavaOne2015_ShouldReturnCountOf4()
        {
            var result = _personManager.GetAllParticipantsForSession(1).Count();

            Assert.AreEqual(4, result);
        }

        // UMU API SYNC WITH OUR PARTICIPANT-TABLE IN DB
        [TestMethod]
        public void SyncWithITS_ShouldReturnExpected()
        {
            _personManager.AddDeleteItsPersonsToDb();

            //Assert.AreEqual(-1, _personManager.GetAllParticipants().Count());
        }

    }
}
