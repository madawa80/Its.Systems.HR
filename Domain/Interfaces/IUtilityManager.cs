using System.Collections.Generic;
using System.Linq;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Interfaces
{
    public interface IUtilityManager
    {
        /// <summary>
        /// Get all the locations in the db.
        /// </summary>
        /// <returns>IQueryable of Location.</returns>
        IQueryable<Location> GetAllLocations();

        /// <summary>
        /// Gets the Id for a location if it exists, otherwise creates it and 
        /// returns the Id of the newly created location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        int? GetIdForLocationOrCreateIfNotExists(string location);

        /// <summary>
        /// Gets all the tags.
        /// </summary>
        /// <returns>IQueryable of Tag.</returns>
        IQueryable<Tag> GetAllTags();

        /// <summary>
        /// Filter the list of tags for tags not already in the database,
        /// and adds those.
        /// </summary>
        /// <param name="tagsToAdd"></param>
        void AddNewTagsToDb(List<Tag> tagsToAdd);

        /// <summary>
        /// Gets the average rating for a session.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The avg. rating rounded to 1 decimal.</returns>
        double GetRatingForSessionById(int id);

        /// <summary>
        /// Gets all the tags currently assigned to a session.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>IQueryable of Tag, null if not found.</returns>
        IQueryable<Tag> GetAllTagsForSessionById(int sessionId);

        /// <summary>
        /// Gets a single Tag.
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns>A Tag or null if not found.</returns>
        Tag GetTag(int tagId);

        //int GetNoOfRatingsForSessionById(int sessionId);
    }
}
