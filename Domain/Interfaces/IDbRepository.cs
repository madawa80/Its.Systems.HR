using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Its.Systems.HR.Domain.Interfaces
{
    public interface IDbRepository
    {
        /// <summary>
        /// Function to retrieve an IQueryable object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <returns>IQueryable object</returns>
        IQueryable<T> Get<T>() where T : class;

        /// <summary>
        /// Function to get a set of objects
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <returns>set of object of a specific type</returns>
        DbSet<T> GetSet<T>() where T : class;

        /// <summary>
        /// Function to add some object to repository
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="input">Domain Object</param>
        /// <returns>Domain Object</returns>
        T Add<T>(T input) where T : class;
        /// <summary>
        /// Function to delete some object from repository
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="input">Domain Object</param>
        /// <returns>Boolean</returns>
        bool Delete<T>(T input) where T : class;
        /// <summary>
        /// Function to commit changes to Repository
        /// </summary>
        /// <returns>Boolean</returns>
        bool SaveChanges();

        /// <summary>
        /// Function for Closeing databaseconnection
        /// </summary>
        void Close();

        DbContext Context();

    }
}
