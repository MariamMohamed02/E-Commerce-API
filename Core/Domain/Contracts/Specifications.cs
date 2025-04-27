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


    }
}

//where-> expression, func(T,bool)
//include -> list of expression and returns object instead of bool



