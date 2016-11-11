using System.Collections.Generic;
using System.Linq;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Interfaces
{
    public interface ISessionManager
    {
        /// <summary>
        /// Gets all the sessions for an activity by activityId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IQueryable of Session if found.</returns>
        IQueryable<Session> GetAllSessionsForActivity(int id);

        /// <summary>
        /// Gets all the sessions for a participant by participantId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IQueryable of Session if found.</returns>
        IQueryable<Session> GetAllSessionsForParticipantById(int id);

        /// <summary>
        /// Adds the session passed.
        /// </summary>
        /// <param name="session"></param>
        /// <returns>True if successfull.</returns>
        bool AddSession(Session session);

        /// <summary>
        /// Gets a session by sessionId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Session if found.</returns>
        Session GetSessionById(int id);

        /// <summary>
        /// Gets all the sessions.
        /// </summary>
        /// <returns>IQueryable of Session if found.</returns>
        IQueryable<Session> GetAllSessions();

        /// <summary>
        /// Saves the comments passed for a session, by sessionId.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comments"></param>
        /// <returns>True if successfull.</returns>
        bool SaveCommentsForSession(int id, string comments);

        /// <summary>
        /// Saves the evaluation passed for a session, by sessionId.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="evaluation"></param>
        /// <returns>True if successfull.</returns>
        bool SaveEvaluationForSession(int id, string evaluation);

        /// <summary>
        /// Gets the session by sessionId, includes Activity, Location, HrPerson and Tags.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>The session with Location, HrPerson and Tags.</returns>
        Session GetSessionByIdWithIncludes(int sessionId);

        /// <summary>
        /// Gets all sessions, includes Activity, Location, HrPerson and Tags.
        /// </summary>
        /// <returns>IQueryble of Session with Location, HrPerson and Tags.</returns>
        IQueryable<Session> GetAllSessionsWithIncludes();

        /// <summary>
        /// Gets all the sessions for a specific year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns>IQueryable of Session.</returns>
        IQueryable<Session> GetAllSessionsForYear(int year);

        /// <summary>
        /// Edits the session being passed.
        /// </summary>
        /// <param name="sessionToUpdate"></param>
        /// <returns>True if successfull.</returns>
        bool EditSession(Session sessionToUpdate);

        /// <summary>
        /// Adds a list of tags to a session by sessionId.
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="sessionId"></param>
        void AddSessionTags(List<Tag> tags, int sessionId);

        /// <summary>
        /// Deletes a session by sessionId.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>True if successfull.</returns>
        bool DeleteSessionById(int sessionId);

        /// <summary>
        /// Adds a tag to a session, by sessionId.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="tagName"></param>
        /// <returns>The id of the created tag, or null it already exists.</returns>
        int? AddTagToSession(int sessionId, string tagName);

        /// <summary>
        /// Removes a tag by tagId from a session, by sessionId.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="tagId"></param>
        bool RemoveTagFromSession(int sessionId, int tagId);
        
        IQueryable<Session> GetAllSessionsForTag(int tagId);

        /// <summary>
        /// Gets all the sessions with reviews and includes everything
        /// related to reviews.
        /// </summary>
        /// <returns>IQueryable of Session.</returns>
        IQueryable<Session> GetAllSessionsWithReviews();
    }
}
