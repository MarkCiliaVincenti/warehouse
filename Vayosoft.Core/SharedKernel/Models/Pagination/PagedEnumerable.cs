﻿using System.Collections;
using System.Collections.Generic;

namespace Vayosoft.Core.SharedKernel.Models.Pagination
{
    public class PagedEnumerable<T> : IPagedEnumerable<T>
    {
        private readonly IEnumerable<T> _inner;
        private readonly long _totalCount;

        public PagedEnumerable(IEnumerable<T> inner, long totalCount)
        {
            _inner = inner;
            _totalCount = totalCount;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public long TotalCount => _totalCount;
    }
}
