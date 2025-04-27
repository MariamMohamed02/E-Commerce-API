using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly AppDbContext _dbContext;

        public GenericRepository(AppDbContext dbContext) {
            _dbContext = dbContext;
        }
        public async Task AddAsync(TEntity entity)
        {
           await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking=false)=> asNoTracking?
            await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync() //true
            : await _dbContext.Set<TEntity>().ToListAsync();  //false



        public async Task<TEntity?> GetByIdAsync(Tkey id) => await _dbContext.Set<TEntity>().FindAsync(id);


    }
}
