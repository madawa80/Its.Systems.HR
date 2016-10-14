using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.UmuApi;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Managers
{
    public class PersonManager : IPersonManager
    {
        private readonly IDbRepository _db;

        public PersonManager(IDbRepository repo)
        {
            _db = repo;
        }

        // TEMPLATE:
        //public IQueryable<Person> GetPersons()
        //{
        //    db.Get<Person>().Include().Where().
        //}

        public IQueryable<Participant> GetAllParticipants()
        {
            return _db.Get<Participant>();
        }

        public IQueryable<HrPerson> GetAllHrPersons()
        {
            return _db.Get<HrPerson>();
        }

        public bool AddHrPerson(HrPerson hrPerson)
        {
            var allHrPersons = _db.Get<HrPerson>().ToList();

            if (allHrPersons.Any(n => n.GetHrFullName() == hrPerson.GetHrFullName()))
                return false;

            _db.Add<HrPerson>(hrPerson);

            return true;
        }

        public bool DeletePaticipantById(int id)
        {
            var paticipantFromDb = _db.Get<Participant>().SingleOrDefault(n => n.Id == id);
            if (paticipantFromDb == null)
                return false;

            _db.Delete(paticipantFromDb);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        public Participant GetParticipantById(int id)
        {
            return _db.Get<Participant>().SingleOrDefault(n => n.Id == id);
        }
        public Participant GetParticipantByCas(string cas)
        {
            return _db.Get<Participant>().SingleOrDefault(n => n.CasId == cas);
        }

        public HrPerson GetHrPersonById(int HrId)
        {
            //var query =
            //(
            //    from HR in db.Get<>()
            //    join @event in _db.Events
            //        on eventUser.EventId equals @event.Id
            //    where eventUser.ProfileId == profileId
            //    select Name
            return _db.Get<HrPerson>().SingleOrDefault(n => n.Id == HrId);
        }

        public bool SaveCommentsForParticipant(int personId, string comments)
        {
            Participant participant = GetAllParticipants().SingleOrDefault(n => n.Id == personId);
            if (participant == null)
                return false;

            participant.Comments = comments;
            try
            {
                _db.SaveChanges();

            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        public bool SaveWishesForParticipant(int personId, string wishes)
        {
            Participant participant = GetAllParticipants().SingleOrDefault(n => n.Id == personId);
            if (participant == null)
                return false;

            participant.Wishes = wishes;
            try
            {
                _db.SaveChanges();

            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        public bool AddParticipantToSession(int participantId, int sessionId)
        {
            // Test participantId for null
            if (!CheckIfParticipantExists(participantId)) return false;
            // Test sessionId for null
            if (!CheckIfSessionExists(sessionId)) return false;
            // Make sure no SessionParticipant exists
            var sessionParticipantFromDb = _db.Get<SessionParticipant>()
                .SingleOrDefault(n => n.ParticipantId == participantId && n.SessionId == sessionId);

            if (sessionParticipantFromDb != null)
                return false;

            // Create a new SessionParticipant
            var result = new SessionParticipant()
            {
                ParticipantId = participantId,
                SessionId = sessionId,
                Rating = 0
            };

            _db.Add<SessionParticipant>(result);
            _db.SaveChanges();

            return true;
        }

        public bool RemoveParticipantFromSession(int participantId, int sessionId)
        {
            // Test participantId for null
            if (!CheckIfParticipantExists(participantId)) return false;
            // Test sessionId for null
            if (!CheckIfSessionExists(sessionId)) return false;
            // Make sure SessionParticipant exists!
            var sessionParticipantFromDb = _db.Get<SessionParticipant>()
                .SingleOrDefault(n => n.ParticipantId == participantId && n.SessionId == sessionId);

            if (sessionParticipantFromDb == null)
                return false;


            _db.Delete<SessionParticipant>(sessionParticipantFromDb);
            _db.SaveChanges();

            return true;
        }

        public IQueryable<Participant> GetAllParticipantsForSession(int id)
        {
            return _db.Get<SessionParticipant>().Where(n => n.SessionId == id).Select(n => n.Participant);
        }

        public bool UpdateReviewForSessionParticipant(int sessionId, int participantIdTEMP, int rating)
        {
            var sessionParticipant = _db.Get<SessionParticipant>()
                .SingleOrDefault(n => n.SessionId == sessionId && n.ParticipantId == participantIdTEMP);

            if (sessionParticipant == null)
                return false;

            if (rating >= 1 && rating <= 5)
            {
                sessionParticipant.Rating = rating;
                _db.SaveChanges();
            }
            else
                return false;

            return true;
        }

        public bool AddItsPersonsToDb()
        {
            var umuApi = new Actions();
            var result = umuApi.GetPersonFromUmuApi();

            foreach (var person in result.Persons)
            {
                var personToAdd = new Participant()
                {
                    CasId = person.CasId,
                    FirstName = person.FirstName,
                    LastName = person.LastName,

                };
                _db.Add<Participant>(personToAdd);
            }

            return true;
        }

        // PRIVATE METHODS BELOW
        private bool CheckIfSessionExists(int sessionId)
        {
            var sessionFromDb = _db.Get<Session>().SingleOrDefault(n => n.Id == sessionId);
            if (sessionFromDb == null)
                return false;
            return true;
        }

        private bool CheckIfParticipantExists(int participantId)
        {
            var participantFromDb = _db.Get<Participant>().SingleOrDefault(n => n.Id == participantId);
            if (participantFromDb == null)
                return false;
            return true;
        }

    }
}
