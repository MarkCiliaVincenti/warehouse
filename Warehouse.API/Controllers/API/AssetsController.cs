﻿using Microsoft.AspNetCore.Mvc;
using Vayosoft.Core.Queries;
using Vayosoft.Core.Queries.Query;
using Vayosoft.Core.SharedKernel.Models.Pagination;
using Warehouse.API.Services.Security.Attributes;
using Warehouse.Core.Entities.Models;
using Warehouse.Core.UseCases.Warehouse.Specifications;

namespace Warehouse.API.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IQueryBus _queryBus;

        public AssetsController(IQueryBus queryBus)
        {
            this._queryBus = queryBus;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(int page, int size, string? searchTerm = null, CancellationToken token = default)
        {
            var spec = new BeaconPositionSpec(page, size, searchTerm);
            var query = new SpecificationQuery<BeaconPositionSpec, IPagedEnumerable<BeaconIndoorPositionEntity>>(spec);

            var result = await _queryBus.Send(query, token);

            return Ok(new
            {
                data = result,
                totalItems = result.TotalCount,
                totalPages = (long)Math.Ceiling((double)result.TotalCount / size)
            });
        }
    }
}
