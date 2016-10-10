using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Managers
{
    public class PersonManager : IPersonManager
    {
        public IDbRepository db;

        public PersonManager(IDbRepository repo)
        {
            db = repo;
        }

        //public IQueryable<Person> GetPersons()
        //{
        //    db.Get<Person>().Include().Where().
        //}

        public IQueryable<Participant> GetAllParticipants()
        {
            return db.Get<Participant>();
        }

        public IQueryable<HrPerson> GetAllHrPersons()
        {
            return db.Get<HrPerson>();
        }

        public bool AddHrPerson(HrPerson hrPerson)
        {
            var allHrPersons = db.Get<HrPerson>().ToList();

            if (allHrPersons.Any(n => n.GetHrFullName() == hrPerson.GetHrFullName()))
                return false;

            db.Add<HrPerson>(hrPerson);

            return true;
        }

        public bool DeletePaticipantById(int id)
        {
            var paticipantFromDb = db.Get<Participant>().SingleOrDefault(n => n.Id == id);
            if (paticipantFromDb == null)
                return false;

            db.Delete(paticipantFromDb);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        public Participant GetParticipantById(int id)
        {
            return db.Get<Participant>().SingleOrDefault(n => n.Id == id);
        }

        public HrPerson GetHRPersonById(int HrId)
        {
            //var query =
            //(
            //    from HR in db.Get<>()
            //    join @event in _db.Events
            //        on eventUser.EventId equals @event.Id
            //    where eventUser.ProfileId == profileId
            //    select Name
            return db.Get<HrPerson>().SingleOrDefault(n => n.Id == HrId);
        }

        public bool SaveCommentsForParticipant(int personId, string comments)
        {
            Participant participant = GetAllParticipants().SingleOrDefault(n => n.Id == personId);
            if (participant == null)
                return false;

            participant.Comments = comments;
            try
            {
                db.SaveChanges();

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
                db.SaveChanges();

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
            var sessionParticipantFromDb = db.Get<SessionParticipant>()
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

            db.Add<SessionParticipant>(result);
            db.SaveChanges();

            return true;
        }

        public bool RemoveParticipantFromSession(int participantId, int sessionId)
        {
            // Test participantId for null
            if (!CheckIfParticipantExists(participantId)) return false;
            // Test sessionId for null
            if (!CheckIfSessionExists(sessionId)) return false;
            // Make sure SessionParticipant exists!
            var sessionParticipantFromDb = db.Get<SessionParticipant>()
                .SingleOrDefault(n => n.ParticipantId == participantId && n.SessionId == sessionId);

            if (sessionParticipantFromDb == null)
                return false;


            db.Delete<SessionParticipant>(sessionParticipantFromDb);
            db.SaveChanges();

            return true;
        }

        private bool CheckIfSessionExists(int sessionId)
        {
            var sessionFromDb = db.Get<Session>().SingleOrDefault(n => n.Id == sessionId);
            if (sessionFromDb == null)
                return false;
            return true;
        }

        private bool CheckIfParticipantExists(int participantId)
        {
            var participantFromDb = db.Get<Participant>().SingleOrDefault(n => n.Id == participantId);
            if (participantFromDb == null)
                return false;
            return true;
        }
    }
}
