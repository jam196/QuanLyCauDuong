using Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Sql
{
    /// <summary>
    /// Contains methods for interacting with the users backend using 
    /// SQL via Entity Framework Core 2.0.
    /// </summary>
    public class SqlUserRepository : IUserRepository
    {
        private readonly ContosoContext _db;

        public SqlUserRepository(ContosoContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _db.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User> GetByEmailAsync(String email)
        {
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<IEnumerable<User>> GetAsync(string value)
        {
            string[] parameters = value.Split(' ');
            return await _db.Users
                .Where(user =>
                    parameters.Any(parameter =>
                        user.Name.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                        user.Email.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(user =>
                    parameters.Count(parameter =>
                        user.Name.StartsWith(parameter) ||
                        user.Email.StartsWith(parameter)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> UpsertAsync(User user)
        {
            /*try
            {
                var current = await _db.Users.FirstOrDefaultAsync(_user => _user.Email == user.Email);
                if (null == current)
                {
                    _db.Users.Add(user);
                }
                else
                {
                    _db.Entry(current).CurrentValues.SetValues(user);
                }

                try
                {
                    await _db.SaveChangesAsync();

                    return user;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            catch (Exception e)
            {
                throw e;
            }*/

            var current = await _db.Users.FirstOrDefaultAsync(_user => _user.Email == user.Email);
            if (null == current)
            {
                _db.Users.Add(user);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(user);
            }
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(Guid userId)
        {
            var match = await _db.Users.FindAsync(userId);
            if (match != null)
            {
                _db.Users.Remove(match);
            }
            await _db.SaveChangesAsync();
        }
    }
}
