using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Infrastructure.UmuApi;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Managers
{
    public class PersonManager : IPersonManager
    {
        //private Catalogue.Interface.Service.Client.CatalogueRestClient client;
        private readonly IDbRepository _db;

        public PersonManager(IDbRepository repo)
        {
            _db = repo;
            //client.
        }

        public IQueryable<Participant> GetAllParticipants()
        {
            return _db.Get<Participant>().Where(n => n.IsDeleted == false);
        }

        public IQueryable<Participant> GetAllParticipantsIncludingDeleted()
        {
            return _db.Get<Participant>();
        }

        public IQueryable<Participant> GetAllHrPersons()
        {
            return _db.Get<Participant>().Where(n => n.IsHrPerson && n.IsDeleted == false);
        }

        public bool MakeParticipantHrPerson(Participant hrPerson)
        {
            var participantToHrPerson = _db.Get<Participant>().SingleOrDefault(n => n.Id == hrPerson.Id);

            if (participantToHrPerson == null)
                return false;

            participantToHrPerson.IsHrPerson = true;
            _db.SaveChanges();

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

        public Participant GetHrPersonById(int hrPersonId)
        {
            return _db.Get<Participant>().SingleOrDefault(n => n.Id == hrPersonId && n.IsHrPerson);
        }

        public bool SaveCommentsForParticipant(int personId, string comments)
        {
            Participant participant = GetAllParticipants().SingleOrDefault(n => n.Id == personId);
            if (participant == null)
                return false;

            participant.Comments = comments;
            _db.SaveChanges();

            return true;
        }

        public bool SaveWishesForParticipant(int personId, string wishes)
        {
            Participant participant = GetAllParticipants().SingleOrDefault(n => n.Id == personId);
            if (participant == null)
                return false;

            participant.Wishes = wishes;
            _db.SaveChanges();

            return true;
        }

        public bool AddParticipantToSession(int participantId, int sessionId)
        {
            if (!CheckIfParticipantExists(participantId) || !CheckIfSessionExists(sessionId) ||
                CheckIfSessionParticipantAlreadyExists(participantId, sessionId))
            {
                return false;
            }

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
            if (!CheckIfParticipantExists(participantId)) return false;

            if (!CheckIfSessionExists(sessionId)) return false;

            var sessionParticipantFromDb = _db.Get<SessionParticipant>()
                .SingleOrDefault(n => n.ParticipantId == participantId && n.SessionId == sessionId);

            if (sessionParticipantFromDb == null)
                return false;


            _db.Delete<SessionParticipant>(sessionParticipantFromDb);
            _db.SaveChanges();

            return true;
        }

        public bool AddExpressionOfInterest(int sessionId, int participantId)
        {
            var theSession = _db.Get<Session>().SingleOrDefault(n => n.Id == sessionId);

            if (theSession == null || !theSession.IsOpenForExpressionOfInterest || theSession.StartDate < DateTime.Now)
                return false;

            var result = new SessionParticipant()
            {
                ParticipantId = participantId,
                SessionId = sessionId,
            };

            _db.Add<SessionParticipant>(result);
            _db.SaveChanges();

            return true;
        }

        public bool RemoveExpressionOfInterest(int sessionId, int participantId)
        {
            var theSession = _db.Get<Session>().SingleOrDefault(n => n.Id == sessionId);

            if (theSession == null || theSession.StartDate < DateTime.Now) // !theSession.IsOpenForExpressionOfInterest ||
                return false;

            var sessionParticipation = GetASessionParticipant(sessionId, participantId);

            _db.Delete<SessionParticipant>(sessionParticipation);

            return true;
        }

        public IQueryable<Participant> GetAllParticipantsForSession(int id)
        {
            return _db.Get<SessionParticipant>().Where(n => n.SessionId == id).Select(n => n.Participant).Where(n => n.IsDeleted == false);
        }

        public bool UpdateReviewForSessionParticipant(int sessionId, int participantId, int rating, string comments)
        {
            SessionParticipant sessionParticipant = GetASessionParticipant(sessionId, participantId);

            if (sessionParticipant == null)
                return false;

            sessionParticipant.Rating = rating;
            sessionParticipant.Comments = comments;
            _db.SaveChanges();

            return true;
        }

        public SessionParticipant GetASessionParticipant(int sessionId, int participantId)
        {
            return GetAllSessionParticipants()
                .SingleOrDefault(n => n.SessionId == sessionId && n.ParticipantId == participantId);
        }

        public IQueryable<SessionParticipant> GetAllSessionParticipationsForSessionById(int sessionId)
        {
            return GetAllSessionParticipants()
                .Where(n => n.SessionId == sessionId);
        }

        public IQueryable<SessionParticipant> GetAllSessionParticipants()
        {
            return _db.Get<SessionParticipant>().Where(n => n.Participant.IsDeleted == false);
        }

        public List<Participant> InactivateItsPersons()
        {
            // Mark the people missing as IsActive = false

            var umuApi = new Actions();
            var result = new List<Participant>();
            var personsFromUmuApi = umuApi.GetPersonFromUmuApi();
            var paticipants = GetAllParticipants().ToList();
            
            foreach (var person in paticipants)
            {
                if (!personsFromUmuApi.Any(n => n.CasId == person.CasId))
                {
                    person.IsActive = false;
                    _db.Context().Entry(person).State = EntityState.Modified;
                    _db.SaveChanges();
                  
                    result.Add(person);
                }
            }

            return result;
        }

        public void UpdateEmail()
        {
            // Mark the people missing as IsActive = false

            var umuApi = new Actions();
            var result = new List<Participant>();
            var personsFromUmuApi = umuApi.GetPersonFromUmuApi().ToList();
            var paticipants = GetAllParticipants().ToList();

            foreach (var person in paticipants)
            {
                if (personsFromUmuApi.Any(n => n.CasId == person.CasId))
                {
                    person.Email = personsFromUmuApi.SingleOrDefault(n => n.CasId == person.CasId).Guise[0].EPost;
                    _db.Context().Entry(person).State = EntityState.Modified;
                    _db.SaveChanges();
                }

            }
        }

        public List<Participant> AddItsPersons()
        {
            // Add possible new people to our database

            var umuApi = new Actions();
            var result = new List<Participant>();
            var personsFromUmuApi = umuApi.GetPersonFromUmuApi();

            foreach (var person in personsFromUmuApi)
            {
                if (!GetAllParticipants().Any(n => n.CasId == person.CasId))
                {
                    var personToAdd = new Participant()
                    {
                        CasId = person.CasId,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        IsActive = true,
                        Email = person.Guise[0].EPost
                    };
                    result.Add(personToAdd);
                    _db.Add<Participant>(personToAdd);
                }
            }

            return result;
        }


        // PRIVATE METHODS BELOW
        private bool CheckIfSessionExists(int sessionId)
        {
            var sessionFromDb = _db.Get<Session>().SingleOrDefault(n => n.Id == sessionId);

            return sessionFromDb != null;
        }

        private bool CheckIfParticipantExists(int participantId)
        {
            var participantFromDb = _db.Get<Participant>().SingleOrDefault(n => n.Id == participantId);

            return participantFromDb != null;
        }

        private bool CheckIfSessionParticipantAlreadyExists(int participantId, int sessionId)
        {
            var sessionParticipantFromDb =
                _db.Get<SessionParticipant>()
                .SingleOrDefault(n => n.ParticipantId == participantId && n.SessionId == sessionId);

            return sessionParticipantFromDb != null;
        }


        public bool ChangeParticipantHrStatus(int id, bool status)
        {
      
            var paticipant = GetParticipantById(id);
            if (paticipant == null)
                return false;
          
                    paticipant.IsHrPerson = status;
                    _db.Context().Entry(paticipant).State = EntityState.Modified;
                    _db.SaveChanges();
                  return true;
        }

        public bool ChangeParticipantDeletedStatus(int id, bool status)
        {

            var paticipant = GetParticipantById(id);
            if (paticipant == null)
                return false;

            paticipant.IsDeleted = status;
            _db.Context().Entry(paticipant).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

    }
}
