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

        double GetRatingForSessionById(int id);
        IQueryable<Tag> GetAllTagsForSessionById(int sessionId);
    }
}
