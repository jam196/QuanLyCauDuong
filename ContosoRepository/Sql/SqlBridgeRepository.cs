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
    public class SqlBridgeRepository : IBridgeRepository
    {
        private readonly ContosoContext _db;

        public SqlBridgeRepository(ContosoContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Bridge>> GetAsync()
        {
            return await _db.Bridges
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Bridge> GetAsync(Guid id)
        {
            return await _db.Bridges
                .AsNoTracking()
                .FirstOrDefaultAsync(bridge => bridge.Id == id);
        }

        public async Task<IEnumerable<Bridge>> GetWithCustomQueryAsync()
        {
            return await _db.Bridges
                .Where(bridge => bridge.Status == "Đang xây dựng")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Bridge>> GetAsync(string value)
        {
            string[] parameters = value.Split(' ');
            return await _db.Bridges
                .Where(bridge =>
                    parameters.Any(parameter =>
                        bridge.Name.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                        bridge.Investor.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                        bridge.Designer.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                        bridge.Builder.StartsWith(parameter) ||
                        bridge.Supervisor.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(bridge =>
                    parameters.Count(parameter =>
                        bridge.Name.StartsWith(parameter) ||
                        bridge.Investor.StartsWith(parameter) ||
                        bridge.Designer.StartsWith(parameter) ||
                        bridge.Builder.StartsWith(parameter) ||
                        bridge.Supervisor.StartsWith(parameter)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Bridge> UpsertAsync(Bridge bridge)
        {
            var current = await _db.Bridges.FirstOrDefaultAsync(_bridge => _bridge.Id == bridge.Id);
            if (null == current)
            {
                _db.Bridges.Add(bridge);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(bridge);
            }
            await _db.SaveChangesAsync();
            return bridge;
        }

        public async Task DeleteAsync(Guid bridgeId)
        {
            var match = await _db.Bridges.FindAsync(bridgeId);
            if (match != null)
            {
                _db.Bridges.Remove(match);
            }
            await _db.SaveChangesAsync();
        }
    }
}
