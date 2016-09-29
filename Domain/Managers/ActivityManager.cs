using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Managers
{
    public class ActivityManager : IActivityManager
    {
        public IDbRepository db;

        public ActivityManager(IDbRepository repo)
        {
            db = repo;
        }

        public IQueryable<Activity> GetAllActivities()
        {
            return db.Get<Activity>();
        }

        public IQueryable<Activity> GetAllActivitiesWithSessions()
        {
            return db.Get<Activity>().Include(n => n.Sessions); //.Where(s => s.Activity == n)
        }

        public IQueryable<Session> GetAllSessionsForActivity(int id)
        {
            return db.Get<Session>().Where(n => n.ActivityId == id);
        }

        public IQueryable<Participant> GetAllParticipantsForSession(int id)
        {
            return db.Get<SessionParticipant>().Where(n => n.SessionId == id).Select(n => n.Participant);
        }

        public IQueryable<Session> GetAllSessionsForParticipantById(int id)
        {
            return db.Get<SessionParticipant>().Where(n => n.ParticipantId == id).Select(n => n.Session);
        }



        public Activity GetActivityById(int id)
        {
            return db.Get<Activity>().SingleOrDefault(n => n.Id == id);
        }

        public bool AddActivity(Activity activityToAdd)
        {
            var allActivities = db.Get<Activity>().ToList();

            if (allActivities.Any(n => n.Name == activityToAdd.Name))
                return false;

            db.Add<Activity>(activityToAdd);

            return true;
        }

        public bool EditActivity(Activity activityToEdit)
        {
            //var allActivities = db.Get<Activity>().ToList();

            //if (allActivities.Any(n => n.Name == activityToEdit.Name))
            //    return false;

            //TODO: Add error handling!?
            db.Context().Entry(activityToEdit).State = EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public bool DeleteActivityById(int id)
        {
            var activityFromDb = db.Get<Activity>().SingleOrDefault(n => n.Id == id);
            if (activityFromDb == null)
                return false;

            db.Delete(activityFromDb);
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



        public void AddSession(Session session)
        {
            db.Add<Session>(session);
        }

        public Session GetSessionById(int id)
        {
            return db.Get<Session>().SingleOrDefault(n => n.Id == id);
        }


        public IQueryable<Location> GetAllLocations()
        {
            return db.Get<Location>();
        }

        public IQueryable<Session> GetAllSessions()
        {
            return db.Get<Session>();
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
        public bool SaveCommentsForSession(int id, string comments)
        {
            Session session = GetAllSessions().SingleOrDefault(n => n.Id == id);
            if (session == null)
                return false;

            session.Comments = comments;
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


        public bool SaveEvaluationForSession(int id, string evaluation)
        {
            Session session = GetAllSessions().SingleOrDefault(n => n.Id == id);
            if (session == null)
                return false;

            session.Evaluation = evaluation;
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

        public int AddLocation(string location)
        {
            var result = new Location()
            {
                Name = location
            };

            db.Add<Location>(result);
            db.SaveChanges();

            return result.Id;
        }

        public Session GetSessionByIdWithIncludes(int sessionId)
        {
            return db.Get<Session>()
                .Include(n => n.Location)
                .Include(n => n.HrPerson)
                .SingleOrDefault(n => n.Id == sessionId);
        }

        public IQueryable<Session> GetAllSessionsWithIncludes()
        {
            return db.Get<Session>()
                .Include(n => n.Location)
                .Include(n => n.HrPerson);
            //.Include(n => n.SessionParticipants)
        }
    }
}