using System.Linq;
using Infrastructure.UmuApi;
using Its.Systems.HR.Domain.Interfaces;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Its.Systems.HR.Test
{
    [TestClass]
    public class PersonTests : BaseTest
    {
        private readonly IPersonManager _personManager;
        private readonly ISessionManager _sessionManager;

        public PersonTests() : base()
        {
            _personManager = Container().Resolve<IPersonManager>();
            _sessionManager = Container().Resolve<ISessionManager>();
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

        [TestMethod]
        public void GetAllSessionParticipations_ShouldNotContainSessionsDeleted()
        {
            var allSessionParticipations = _personManager.GetAllSessionParticipants();
            var allSessions = _sessionManager.GetAllSessions();

            var count = 0;

            foreach (var sessionParticipant in allSessionParticipations)
            {
                if (!allSessions.Any(n => n.Id == sessionParticipant.SessionId))
                    count++;
            }

            Assert.AreEqual(0, count);
        }

        // UMU API SYNC WITH OUR PARTICIPANT-TABLE IN DB
        [TestMethod]
        public void SyncWithITS_ShouldReturnExpected()
        {
            // Assumes the following CasIds exists in UmuApi ITS Persons:
            // sape0014, maku0029, elnjan96, jaru0002

            var umuApi = new Actions();
            var actualCountFromUmuWebApi = umuApi.GetPersonFromUmuApi().Count;
             
            var addedPersons = _personManager.AddItsPersons();
            var inactivatedPersons = _personManager.InactivateItsPersons();

            Assert.AreEqual(4, inactivatedPersons.Count);
            Assert.AreEqual(actualCountFromUmuWebApi - 4, addedPersons.Count);
            Assert.AreEqual(actualCountFromUmuWebApi, _personManager.GetAllParticipants().Count(n => n.IsActive));
        }

    }
}
