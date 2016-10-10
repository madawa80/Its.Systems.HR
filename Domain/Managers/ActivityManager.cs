﻿using System;
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

        //public bool DeletePaticipantById(int id)
        //{
        //    var paticipantFromDb = db.Get<Participant>().SingleOrDefault(n => n.Id == id);
        //    if (paticipantFromDb == null)
        //        return false;

        //    db.Delete(paticipantFromDb);
        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //    return true;
        //}
       
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
                .Include(n => n.SessionTags)
                .SingleOrDefault(n => n.Id == sessionId);
        }

        public IQueryable<Session> GetAllSessionsWithIncludes()
        {
            return db.Get<Session>()
                .Include(n => n.Activity)
                .Include(n => n.Location)
                .Include(n => n.HrPerson);
            //.Include(n => n.SessionParticipants)
        }

        public IQueryable<Session> GetAllSessionsForYear(int Year)
        {
            return db.Get<Session>().Where(n => n.StartDate.Year == Year);
        }

        public bool EditSession(Session sessionToUpdate)
        {
            //var allActivities = db.Get<Activity>().ToList();

            //if (allActivities.Any(n => n.Name == activityToEdit.Name))
            //    return false;

            //TODO: Add error handling!?
            db.Context().Entry(sessionToUpdate).State = EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public IQueryable<Tag> GetAllTags()
        {
            return db.Get<Tag>();
        }

        public void AddTags(List<Tag> tags)
        {
            // TODO: this will result in one roundtrip for every new tag...
            foreach (var tag in tags)
            {
                db.Add<Tag>(tag);
            }
            db.SaveChanges();
        }

        public void AddSessionTags(List<Tag> tags, int sessionId)
        {
            foreach (var tag in tags)
            {
                var tagFromDb = db.Get<Tag>().SingleOrDefault(n => n.Name == tag.Name);
                if (tagFromDb != null)
                    db.Add<SessionTag>(new SessionTag()
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
            var sessionInDb = db.Get<Session>().SingleOrDefault(n => n.Id == sessionId);
            if (sessionInDb == null)
                return false;

            db.Delete(sessionInDb);
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

        public int AddTagToSession(int sessionId, string tagName)
        {
            tagName = tagName.ToLower();

            // 0. Check if the tag is already in the SessionTag table!
            if (db.Get<SessionTag>().Any(n => n.SessionId == sessionId && n.Tag.Name == tagName))
                return -1;

            int tagId;
            // 1. check if the tagName already exists
            var existingTag = db.Get<Tag>().SingleOrDefault(n => n.Name == tagName);
            // 2. If exists, take the tag id, if NOT; create a new tag and take the id
            if (existingTag != null)
                tagId = existingTag.Id;
            else
            {
                var createdTagInDb = db.Add<Tag>(new Tag()
                {
                    Name = tagName
                });

                tagId = createdTagInDb.Id;
            }
            // 3. Create a SessionTag with the result.
            db.Add<SessionTag>(new SessionTag()
            {
                SessionId = sessionId,
                TagId = tagId
            });

            return tagId;
        }

        public bool RemoveTagFromSession(int sessionId, int tagId)
        {
            var sessionTagtoDelete =
                db.Get<SessionTag>().SingleOrDefault(n => n.SessionId == sessionId && n.TagId == tagId);
            if (sessionTagtoDelete == null)
                return false;

            db.Delete(sessionTagtoDelete);
            return true;
        }
    }
}