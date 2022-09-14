﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Vayosoft.Core.Specifications
{
    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>> Selector { get; } 
    }

    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        ICollection<Expression<Func<T, object>>> Includes { get; }
        ICollection<string> IncludeStrings { get; }
        ICollection<Expression<Func<T, bool>>> WhereExpressions { get; }
    }
}
