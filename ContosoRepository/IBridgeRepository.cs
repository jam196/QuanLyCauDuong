using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// Các phương thức cho bảng Bridges.
    /// </summary>
    public interface IBridgeRepository
    {
        /// <summary>
        /// Lấy danh sách cầu. 
        /// </summary>
        Task<IEnumerable<Bridge>> GetAsync();

        Task<IEnumerable<Bridge>> GetWithCustomQueryAsync();

        /// <summary>
        /// Lấy danh sách cầu + search. 
        /// </summary>
        Task<IEnumerable<Bridge>> GetAsync(string search);

        /// <summary>
        /// Lấy thông tin cầu theo ID. 
        /// </summary>
        Task<Bridge> GetAsync(Guid id);

        /// <summary>
        /// Thêm cầu, nếu cầu đã có rồi thì update thông tin.
        /// </summary>
        Task<Bridge> UpsertAsync(Bridge bridge);

        /// <summary>
        /// Xóa cầu.
        /// </summary>
        Task DeleteAsync(Guid bridgeId);
    }
}