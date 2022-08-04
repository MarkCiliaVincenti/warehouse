﻿using FluentValidation;
using MediatR;
using Vayosoft.Core.Commands;
using Vayosoft.Core.SharedKernel;
using Warehouse.Core.Entities.Models;
using Warehouse.Core.Persistence;
using Warehouse.Core.UseCases.Administration.Models;
using Warehouse.Core.UseCases.Management.Models;

namespace Warehouse.Core.UseCases.Management.Commands
{
    public class SetWarehouseSite : WarehouseSiteDto, ICommand
    {
        public class WarehouseRequestValidator : AbstractValidator<SetWarehouseSite>
        {
            public WarehouseRequestValidator()
            {
                RuleFor(q => q.Name).NotEmpty();
            }
        }
    }

    internal class HandleSetWarehouseSite : ICommandHandler<SetWarehouseSite>
    {
        private readonly WarehouseStore _store;
        private readonly IMapper _mapper;
        private readonly IdentityContext _context;

        public HandleSetWarehouseSite(WarehouseStore store, IMapper mapper, IdentityContext context)
        {
            _store = store;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Unit> Handle(SetWarehouseSite request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Id))
            {
                await _store.GetAndUpdateAsync<WarehouseSiteEntity>(request.Id, entity =>
                {
                    entity.Name = request.Name;
                    entity.TopLength = request.TopLength;
                    entity.LeftLength = request.LeftLength;
                    entity.Error = request.Error;
                }, cancellationToken);
            }
            else
            {
                var entity = _mapper.Map<WarehouseSiteEntity>(request);
                entity.ProviderId = _context.ProviderId ?? 0;
                await _store.AddAsync(entity, cancellationToken: cancellationToken);
            }

            return Unit.Value;
        }
    }
}
