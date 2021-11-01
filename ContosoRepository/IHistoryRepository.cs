using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// Các phương thức cho bảng Bridges.
    /// </summary>
    public interface IHistoryRepository
    {
        /// <summary>
        /// Lấy danh sách cầu. 
        /// </summary>
        Task<IEnumerable<History>> GetAsync();

        /// <summary>
        /// Lấy danh sách cầu + search. 
        /// </summary>
        Task<IEnumerable<History>> GetAsync(string search);

        /// <summary>
        /// Lấy thông tin cầu theo ID. 
        /// </summary>
        Task<History> GetAsync(Guid id);

        /// <summary>
        /// Thêm cầu, nếu cầu đã có rồi thì update thông tin.
        /// </summary>
        Task<History> UpsertAsync(History bridge);

        /// <summary>
        /// Xóa cầu.
        /// </summary>
        Task DeleteAsync(Guid historyId);
    }
}