﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity,Tkey> where TEntity: BaseEntity<Tkey>
    {
        Task<TEntity?> GetByIdAsync(Tkey id);
        Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking=false); //default value

        Task<TEntity?> GetByIdAsync(Specifications<TEntity> specifications);
        Task<IEnumerable<TEntity>> GetAllAsync(Specifications<TEntity> specifications); 
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        // Pagination Function to return total count
        Task<int> CountAsync(Specifications<TEntity> specifications);
    }
}
