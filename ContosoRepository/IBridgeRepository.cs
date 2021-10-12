using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// Defines methods for interacting with the customers backend.
    /// </summary>
    public interface IBridgeRepository
    {
        /// <summary>
        /// Returns all bridges. 
        /// </summary>
        Task<IEnumerable<Bridge>> GetAsync();

        /// <summary>
        /// Returns all bridges with a data field matching the start of the given string. 
        /// </summary>
        Task<IEnumerable<Bridge>> GetAsync(string search);

        /// <summary>
        /// Returns the customer with the given id. 
        /// </summary>
        Task<Bridge> GetAsync(Guid id);

        /// <summary>
        /// Adds a new bridge if the customer does not exist, updates the 
        /// existing bridge otherwise.
        /// </summary>
        Task<Bridge> UpsertAsync(Bridge bridge);

        /// <summary>
        /// Deletes a bridge.
        /// </summary>
        Task DeleteAsync(Guid bridgeId);
    }
}