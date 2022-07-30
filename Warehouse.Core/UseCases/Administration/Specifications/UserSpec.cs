﻿using Vayosoft.Core.SharedKernel.Models;
using Vayosoft.Core.SharedKernel.Models.Pagination;
using Vayosoft.Core.Specifications;
using Warehouse.Core.Entities.Models;

namespace Warehouse.Core.UseCases.Administration.Specifications
{
    public class UserSpec : SortByIdPaging<UserEntityDto>, ILinqSpecification<UserEntity>
    {
        private readonly long _providerId;
        private readonly string _searchTerm;

        public UserSpec(int page, int take, long providerId, string searchTerm = null)
            : base(page, take, SortOrder.Desc)
        {
            _providerId = providerId;
            _searchTerm = searchTerm;
        }

        public IQueryable<UserEntity> Apply(IQueryable<UserEntity> query)
        {
            if (!string.IsNullOrEmpty(_searchTerm))
                query = query
                    .Where(u => u.Username.IndexOf(_searchTerm, StringComparison.OrdinalIgnoreCase) > -1);

            return query.Where(u => u.ProviderId == _providerId);
        }
    }
}
