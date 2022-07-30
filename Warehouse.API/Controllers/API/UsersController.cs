﻿using Microsoft.AspNetCore.Mvc;
using Vayosoft.Core.Commands;
using Vayosoft.Core.Persistence.Commands;
using Vayosoft.Core.Persistence.Queries;
using Vayosoft.Core.Queries;
using Vayosoft.Core.SharedKernel.Models.Pagination;
using Warehouse.API.Services.Security.Attributes;
using Warehouse.API.Services.Security.Session;
using Warehouse.Core.Entities.Models;
using Warehouse.Core.UseCases.Administration.Specifications;
using Warehouse.API.Extensions;

namespace Warehouse.API.Controllers.API
{
    [Authorize]
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
            //IHttpContextAccessor httpContextAccessor
            //HttpContext.Items.TryGetValue("User", out var user3);
            //var user2 = HttpContext.Items["User"];
            //var user = HttpContext.User;
            var identityData = HttpContext.Session.Get<IdentityData>(nameof(IdentityData));

            var spec = new UserSpec(page, take, identityData.ProviderId);
            var query = new SpecificationQuery<UserSpec, IPagedEnumerable<UserEntityDto>>(spec);

            var data = await queryBus.Send(query, token);

            return Ok(new
            {
                data,
                totalItems = data.TotalCount,
                totalPages = (long) Math.Ceiling((double)data.TotalCount / take)
            });
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
