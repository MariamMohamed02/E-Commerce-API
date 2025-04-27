using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        //SaveChanges --> Complete

        Task<int> SaveChangesAsync();

        // Signature for function that will return an instance of class that implements IGenericRepo
        // Example: GenericRepository<Product,int>,   GenericRepository<ProductBrand,int>

        // add constraint to the tentity 
        IGenericRepository<TEntity,Tkey> GetRepository<TEntity,Tkey>() where TEntity:BaseEntity<Tkey>;
    }
}
