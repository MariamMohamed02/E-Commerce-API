using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public abstract class Specifications<T> where T:class
    {
        public Expression<Func<T, bool>>? Criteria { get; } // where

        public List<Expression<Func<T, object>>> IncludeExpressions { get; } = new();

        protected Specifications(Expression<Func<T, bool>>? creteria)
        {
            Criteria = creteria;
        }

        protected void AddInclude(Expression<Func<T, object>> expression)
            => IncludeExpressions.Add(expression);

        // ---------- SORTING and FILTERING ---------------
        public Expression<Func<T,object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        protected void SetOrderBy(Expression<Func<T, object>> expression)
            => OrderBy=expression;

        protected void SetOrderByDescending(Expression<Func<T, object>> expression)
            => OrderByDescending = expression;


        // ---------------- PAGINATION SPECIFICATION------
        public int Take {  get; private set; }
        public int Skip { get; private set; }
        public bool IsPaginated { get; private set; }

        protected void ApplyPagination(int pageIndex, int pageSize)
        {
            IsPaginated = true;
            Take=pageSize;
            Skip=(pageIndex-1)*pageSize;
        }



    }
}

//where-> expression, func(T,bool)
//include -> list of expression and returns object instead of bool



