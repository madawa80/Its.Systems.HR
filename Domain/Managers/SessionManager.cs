using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Managers
{
    public class SessionManager : ISessionManager
    {
        private readonly IDbRepository _db;

        public SessionManager(IDbRepository repo)
        {
            _db = repo;
        }

        public IQueryable<Session> GetAllSessionsForActivity(int id)
        {
            return _db.Get<Session>().Where(n => n.ActivityId == id);
        }

        public IQueryable<Session> GetAllSessionsForParticipantById(int id)
        {
            return _db.Get<SessionParticipant>().Where(n => n.ParticipantId == id).Select(n => n.Session);
        }

        public void AddSession(Session session)
        {
            _db.Add<Session>(session);
        }

        public Session GetSessionById(int id)
        {
            return _db.Get<Session>().SingleOrDefault(n => n.Id == id);
        }

        public IQueryable<Session> GetAllSessions()
        {
            return _db.Get<Session>();
        }

        public bool SaveCommentsForSession(int id, string comments)
        {
            Session session = GetAllSessions().SingleOrDefault(n => n.Id == id);
            if (session == null)
                return false;

            session.Comments = comments;
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

        public bool SaveEvaluationForSession(int id, string evaluation)
        {
            Session session = GetAllSessions().SingleOrDefault(n => n.Id == id);
            if (session == null)
                return false;

            session.Evaluation = evaluation;
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

        public Session GetSessionByIdWithIncludes(int sessionId)
        {
            return _db.Get<Session>()
                .Include(n => n.Activity)
                .Include(n => n.Location)
                .Include(n => n.HrPerson)
                .Include(n => n.SessionTags)
                .Include(n => n.SessionParticipants)
                .SingleOrDefault(n => n.Id == sessionId);
        }

        public IQueryable<Session> GetAllSessionsWithIncludes()
        {
            return _db.Get<Session>()
                .Include(n => n.Activity)
                .Include(n => n.Location)
                .Include(n => n.HrPerson)
                .Include(n => n.SessionTags);
            //.Include(n => n.SessionParticipants)
        }

        public IQueryable<Session> GetAllSessionsForYear(int Year)
        {
            return _db.Get<Session>().Where(n => n.StartDate.Year == Year);
        }

        public bool EditSession(Session sessionToUpdate)
        {
            //var allActivities = db.Get<Activity>().ToList();

            //if (allActivities.Any(n => n.Name == activityToEdit.Name))
            //    return false;

            //TODO: Add error handling!?
            _db.Context().Entry(sessionToUpdate).State = EntityState.Modified;
            _db.SaveChanges();

            return true;
        }

        public void AddSessionTags(List<Tag> tags, int sessionId)
        {
            foreach (var tag in tags)
            {
                var tagFromDb = _db.Get<Tag>().SingleOrDefault(n => n.Name == tag.Name);
                if (tagFromDb != null)
                    _db.Add<SessionTag>(new SessionTag()
                    {
                        TagId = tagFromDb.Id,
                        SessionId = sessionId
                    });
            }
            // already saves changes in db.Add<T>
            //db.SaveChanges();
        }

        public bool DeleteSessionById(int sessionId)
        {
            var sessionInDb = _db.Get<Session>().SingleOrDefault(n => n.Id == sessionId);
            if (sessionInDb == null)
                return false;

            _db.Delete(sessionInDb);
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

        public int AddTagToSession(int sessionId, string tagName)
        {
            tagName = tagName.ToLower();

            // 0. Check if the tag is already in the SessionTag table!
            if (_db.Get<SessionTag>().Any(n => n.SessionId == sessionId && n.Tag.Name == tagName))
                return -1;

            int tagId;
            // 1. check if the tagName already exists
            var existingTag = _db.Get<Tag>().SingleOrDefault(n => n.Name == tagName);
            // 2. If exists, take the tag id, if NOT; create a new tag and take the id
            if (existingTag != null)
                tagId = existingTag.Id;
            else
            {
                var createdTagInDb = _db.Add<Tag>(new Tag()
                {
                    Name = tagName
                });

                tagId = createdTagInDb.Id;
            }
            // 3. Create a SessionTag with the result.
            _db.Add<SessionTag>(new SessionTag()
            {
                SessionId = sessionId,
                TagId = tagId
            });

            return tagId;
        }
        ///<inheritDoc />
        public bool RemoveTagFromSession(int sessionId, int tagId)
        {
            var sessionTagtoDelete =
                _db.Get<SessionTag>().SingleOrDefault(n => n.SessionId == sessionId && n.TagId == tagId);
            if (sessionTagtoDelete == null)
                return false;

            _db.Delete(sessionTagtoDelete);
            return true;
        }
    }
}