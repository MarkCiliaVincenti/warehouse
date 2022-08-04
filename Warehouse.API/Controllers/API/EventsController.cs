﻿using Microsoft.AspNetCore.Mvc;
using Vayosoft.Core.Queries;
using Warehouse.API.Services.Security.Attributes;
using Warehouse.Core.UseCases.BeaconTracking.Queries;
using Warehouse.Core.Utilities;

namespace Warehouse.API.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IQueryBus _queryBus;

        public EventsController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery] GetBeaconEvents query, CancellationToken token = default)
        {
            return Ok((await _queryBus.Send(query, token)).ToPagedResponse(query.Size));
        }
    }
}
