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
    public class UtilitiesTests : BaseTest
    {
        private readonly IUtilityManager _utilityManager;
        private readonly IPersonManager _personManager;
        private readonly ISessionManager _sessionManager;

        public UtilitiesTests() : base()
        {
            _utilityManager = Container().Resolve<IUtilityManager>();
            _personManager = Container().Resolve<IPersonManager>();
            _sessionManager = Container().Resolve<ISessionManager>();
        }

        // RATINGS
        [TestMethod]
        public void GetRatingForSession_ShouldReturnExpected()
        {
            var expected = 3.7;

            var result = _utilityManager.GetRatingForSessionById(1);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetRatingForSessionWithNoRatings_ShouldReturn0()
        {
            var expected = 0;

            var result = _utilityManager.GetRatingForSessionById(5);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AddNewRating_ShouldUpdateTotalRating()
        {
            var sessionId = 1;
            var participantId = 5;

            var newSessionParticipant = _personManager.AddParticipantToSession(participantId, sessionId);
            var newReview = _personManager.UpdateReviewForSessionParticipant(sessionId, participantId, 1, "New");

            var expected = 3;

            var result = _utilityManager.GetRatingForSessionById(1);

            var session = _sessionManager.GetSessionByIdWithIncludes(1);
            var currentSessionParticipation = session.SessionParticipants.SingleOrDefault(n => n.ParticipantId == participantId && n.SessionId == sessionId);

            Assert.AreEqual(true, newSessionParticipant);
            Assert.AreEqual(true, newReview);
            Assert.AreEqual(expected, result);
            Assert.AreEqual("New", currentSessionParticipation.Comments);
        }

        // LOCATIONS
        [TestMethod]
        public void AddLocationAlreadyExisting_ShouldReturnExpected()
        {
            int? newLocationAlreadyExisting = _utilityManager.GetIdForLocationOrCreateIfNotExists("Umeå");

            var expectedCount = 5;

            var resultCount = _utilityManager.GetAllLocations().Count();

            Assert.AreEqual(expectedCount, resultCount);
            Assert.AreEqual(1, newLocationAlreadyExisting);
        }

        [TestMethod]
        public void AddLocationNotAlreadyExistingAndOneExisting_ShouldReturnExpected()
        {
            int? newLocationNotAlreadyExisting = _utilityManager.GetIdForLocationOrCreateIfNotExists("Tokyo");
            int? newLocationAlreadyExisting = _utilityManager.GetIdForLocationOrCreateIfNotExists("Umeå");

            var expectedCount = 6;

            var resultCount = _utilityManager.GetAllLocations().Count();

            Assert.AreEqual(expectedCount, resultCount);
            Assert.AreEqual("Tokyo", _utilityManager.GetAllLocations().SingleOrDefault(n => n.Name == "Tokyo").Name);
            Assert.AreEqual(1, newLocationAlreadyExisting);
        }

        // TAGS
        [TestMethod]
        public void AddTagsAlreadyExistingAndNotExisting_ShouldReturnExpected()
        {
            List<Tag> tagsToAdd = new List<Tag>()
            {
                new Tag() {Name = "lunch"}, // existing
                new Tag() {Name = "SQL"}, // existing
                new Tag() {Name = "sql"}, // existing
                new Tag() {Name = "newtag"},
                new Tag() {Name = "UNIQUETag"},
                new Tag() {Name = "uniquetag"}, // duplicate
            };

            _utilityManager.AddNewTagsToDb(tagsToAdd);

            // Original count is 6
            var expectedCount = 8;

            var resultCount = _utilityManager.GetAllTags().Count();

            Assert.AreEqual(expectedCount, resultCount);
            Assert.AreEqual("uniquetag", _utilityManager.GetAllTags().SingleOrDefault(n => n.Name == "uniquetag").Name);
            Assert.AreNotEqual(1, _utilityManager.GetAllTags().Count(n => n.Name == "UNIQUETag"));
        }

        [TestMethod]
        public void TagsForSessionId1_ShouldReturnExpected()
        {
            var expectedCount = 3;

            var resultCount = _utilityManager.GetAllTagsForSessionById(1).Count();

            Assert.AreEqual(expectedCount, resultCount);
        }
    }
}
