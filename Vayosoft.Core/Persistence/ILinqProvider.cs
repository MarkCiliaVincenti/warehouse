﻿using System.Linq;
using Vayosoft.Core.SharedKernel.Entities;

namespace Vayosoft.Core.Persistence
{
    public interface ILinqProvider

    {
        IQueryable<TEntity> AsQueryable<TEntity>()
            where TEntity : class, IEntity;
    }
}
