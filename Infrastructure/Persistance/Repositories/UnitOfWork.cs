using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly ConcurrentDictionary<string, object> _repositories;

        public UnitOfWork(AppDbContext dbContext) {
            _dbContext = dbContext;
            _repositories = new();
        }
        public IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            //The commented method below is not good
            // User can ask for many repsos in the same request
            // return new GenericRepository<TEntity,Tkey>(_dbContext);

            // Other Method: Use concurrent dictionaries
            return (IGenericRepository<TEntity, Tkey>) _repositories.GetOrAdd(typeof(TEntity).Name, (_) => new GenericRepository<TEntity, Tkey>(_dbContext));

        }
      

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
