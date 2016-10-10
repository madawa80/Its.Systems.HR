using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Interfaces
{
    public interface IUtilityManager
    {
        /// <summary>
        /// Adds an location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>The id of the added location.</returns>
        int AddLocation(string location);

        /// <summary>
        /// Gets all the tags.
        /// </summary>
        /// <returns>IQueryable of Tag.</returns>
        IQueryable<Tag> GetAllTags();

        /// <summary>
        /// Adds the list of tags passed.
        /// </summary>
        /// <param name="tags"></param>
        void AddTags(List<Tag> tags);
    }
}
