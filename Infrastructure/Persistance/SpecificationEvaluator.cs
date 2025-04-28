using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore.Query;

namespace Persistance
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T> (IQueryable<T> inputQuery, Specifications<T> specifications) where T: class
        {
            var query = inputQuery;
            if(specifications.Criteria is not null) query=query.Where(specifications.Criteria);
            query = specifications.IncludeExpressions.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            // Sorting
            if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);
            else if (specifications.OrderByDescending is not null)
                query = query.OrderByDescending(specifications.OrderByDescending);

            // Pagination
            if (specifications.IsPaginated)
                query=query.Skip(specifications.Skip).Take(specifications.Take);
            return query;
        }
    }
}
