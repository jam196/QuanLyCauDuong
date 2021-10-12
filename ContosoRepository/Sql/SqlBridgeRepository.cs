using Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Sql
{
    /// <summary>
    /// Contains methods for interacting with the customers backend using 
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
            return await _db.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(customer => customer.Id == id);
        }

        public async Task<IEnumerable<Bridge>> GetAsync(string value)
        {
            string[] parameters = value.Split(' ');
            return await _db.Bridges
                .Where(customer =>
                    parameters.Any(parameter =>
                        customer.Name.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                        customer.Investor.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                        customer.Designer.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) ||
                        customer.Builder.StartsWith(parameter) ||
                        customer.Supervisor.StartsWith(parameter, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(customer =>
                    parameters.Count(parameter =>
                        customer.Name.StartsWith(parameter) ||
                        customer.Investor.StartsWith(parameter) ||
                        customer.Designer.StartsWith(parameter) ||
                        customer.Builder.StartsWith(parameter) ||
                        customer.Supervisor.StartsWith(parameter)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Bridge> UpsertAsync(Bridge bridge)
        {
            var current = await _db.Customers.FirstOrDefaultAsync(_customer => _customer.Id == bridge.Id);
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

        public async Task DeleteAsync(Guid id)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(_customer => _customer.Id == id);
            if (null != customer)
            {
                var orders = await _db.Orders.Where(order => order.CustomerId == id).ToListAsync();
                _db.Orders.RemoveRange(orders);
                _db.Customers.Remove(customer);
                await _db.SaveChangesAsync();
            }
        }
    }
}
