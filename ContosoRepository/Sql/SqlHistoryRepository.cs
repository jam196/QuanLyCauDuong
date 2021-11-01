using Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Sql
{
    /// <summary>
    /// Contains methods for interacting with the bridges backend using 
    /// SQL via Entity Framework Core 2.0.
    /// </summary>
    public class SqlHistoryRepository : IHistoryRepository
    {
        private readonly ContosoContext _db;

        public SqlHistoryRepository(ContosoContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<History>> GetAsync()
        {
            return await _db.Histories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<History> GetAsync(Guid id)
        {
            return await _db.Histories
                .AsNoTracking()
                .FirstOrDefaultAsync(history => history.Id == id);
        }

        public async Task<IEnumerable<History>> GetAsync(string value)
        {
            string[] parameters = value.Split(' ');
            return await _db.Histories
                .Where(history =>
                    parameters.Any(parameter =>
                        history.Content.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(history =>
                    parameters.Count(parameter =>
                        history.Content.StartsWith(parameter)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<History> UpsertAsync(History history)
        {
            var current = await _db.Histories.FirstOrDefaultAsync(_history => _history.Id == history.Id);
            if (null == current)
            {
                _db.Histories.Add(history);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(history);
            }
            await _db.SaveChangesAsync();
            return history;
        }

        public async Task DeleteAsync(Guid historyId)
        {
            var match = await _db.Histories.FindAsync(historyId);
            if (match != null)
            {
                _db.Histories.Remove(match);
            }
            await _db.SaveChangesAsync();
        }
    }
}
