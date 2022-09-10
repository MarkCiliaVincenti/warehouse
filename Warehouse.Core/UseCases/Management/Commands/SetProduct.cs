﻿using FluentValidation;
using MediatR;
using Vayosoft.Core.Commands;
using Vayosoft.Core.Persistence;
using Vayosoft.Core.SharedKernel;
using Warehouse.Core.Entities.Models;
using Warehouse.Core.Services;
using Warehouse.Core.Services.Security;
using Warehouse.Core.UseCases.Management.Models;

namespace Warehouse.Core.UseCases.Management.Commands
{
    public class SetProduct : ProductDto, ICommand
    {
        public class ProductRequestValidator : AbstractValidator<SetProduct>
        {
            public ProductRequestValidator()
            {
                RuleFor(q => q.Name).NotEmpty();
                //RuleFor(q => q.MacAddress).MacAddress();
            }
        }
    }
    internal class HandleSetProduct : ICommandHandler<SetProduct>
    {
        private readonly IRepositoryBase<ProductEntity> _repository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public HandleSetProduct(IRepositoryBase<ProductEntity> repository, IMapper mapper, IUserContext userContext)
        {
            _repository = repository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(SetProduct request, CancellationToken cancellationToken)
        {
            ProductEntity entity;
            if (!string.IsNullOrEmpty(request.Id) &&
                (entity = await _repository.FindAsync(request.Id, cancellationToken)) != null)
            {
                entity.Name = request.Name;
                entity.Description = request.Description;
                entity.Metadata = request.Metadata;
                await _repository.UpdateAsync(entity, cancellationToken);
            }
            else
            {
                entity = _mapper.Map<ProductEntity>(request);
                var providerId = _userContext.User.Identity.GetProviderId();
                entity.ProviderId = providerId;
                await _repository.AddAsync(entity, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
