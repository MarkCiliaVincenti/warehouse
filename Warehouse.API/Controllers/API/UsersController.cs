﻿using Microsoft.AspNetCore.Mvc;
using Vayosoft.Core.Commands;
using Vayosoft.Core.Persistence.Commands;
using Vayosoft.Core.Persistence.Queries;
using Vayosoft.Core.Queries;
using Vayosoft.Core.SharedKernel.Models.Pagination;
using Warehouse.API.Services.Authorization.Attributes;
using Warehouse.Core.Entities.Models;
using Warehouse.Core.Entities.Models.Security;
using Warehouse.Core.UseCases.Administration.Specifications;
using Warehouse.Core.Utilities;

namespace Warehouse.API.Controllers.API
{
    [PermissionAuthorization("USER", SecurityPermissions.Grant)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ICommandBus commandBus;
        private readonly IQueryBus queryBus;

        public UsersController(ICommandBus commandBus, IQueryBus queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page, int take, CancellationToken token)
        {
            var providerId = HttpContext.User.Identity?.GetProviderId() ?? 0;
            var spec = new UserSpec(page, take, providerId);
            var query = new SpecificationQuery<UserSpec, IPagedEnumerable<UserEntityDto>>(spec);

            return Ok((await queryBus.Send(query, token)).ToPagedResponse(take));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(ulong id, CancellationToken token)
        {
            var query = new SingleQuery<UserEntityDto>(id);
            return Ok(await queryBus.Send(query, token));
        } 
        
        [HttpPost("set")]
        public async Task<IActionResult> Post(UserEntityDto dto, CancellationToken token)
        {
            var command = new CreateOrUpdateCommand<UserEntityDto>(dto);
            await commandBus.Send(command, token);
            return Ok();
        }
    }
}
