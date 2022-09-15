﻿using Vayosoft.Core.SharedKernel.Entities;

namespace Vayosoft.Core.SharedKernel.Models.Pagination
{
    public class SortByIdPaging<TEntity>: SortByIdPaging<TEntity, long>
        where TEntity : class, IEntity<long>
    {
        public SortByIdPaging(int page, int size, SortOrder orderBy)
            : base(page, size, orderBy) { }
        
        public SortByIdPaging() { }
    }

    public class SortByIdPaging<TEntity, TKey>: PagingModelBase<TEntity, TKey> 
        where TEntity : class, IEntity<TKey>
    {
        public SortByIdPaging(int page, int size, SortOrder orderBy)
            : base(page, size, new Sorting<TEntity, TKey>(x => x.Id, orderBy)) { }

        public SortByIdPaging() { }

        protected override Sorting<TEntity, TKey> BuildDefaultSorting()
        {
            return new Sorting<TEntity, TKey>(x => x.Id, SortOrder.Desc);
        }
    }
}
