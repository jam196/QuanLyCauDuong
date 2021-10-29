using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// Các phương thức cho bảng Users.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Lấy danh sách user. 
        /// </summary>
        Task<IEnumerable<User>> GetAsync();

        /// <summary>
        /// Lấy danh sách user + search. 
        /// </summary>
        Task<IEnumerable<User>> GetAsync(string search);

        /// <summary>
        /// Lấy thông tin user theo ID. 
        /// </summary>
        Task<User> GetAsync(Guid id);

        /// <summary>
        /// Thêm cầu, nếu user đã có rồi thì update thông tin.
        /// </summary>
        Task<User> UpsertAsync(User user);

        /// <summary>
        /// Xóa cầu.
        /// </summary>
        Task DeleteAsync(Guid userId);

        /// <summary>
        /// Lấy thông tin user theo ID. 
        /// </summary>
        Task<User> GetByEmailAsync(String email);
    }
}