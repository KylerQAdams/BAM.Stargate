﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Models;
using StargateAPI.Business.Queries;
using System.Net;

namespace StargateAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(GetPeopleResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPeople()
        {
            var result = await _mediator.Send(new GetPeople());

            return Ok(result);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(GetPersonByNameResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPersonByName(string name)
        {
            var result = await _mediator.Send(new GetPersonByName()
            {
                Name = name
            });

            return Ok(result);
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(CreatePersonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePerson([FromBody] string name)
        {
            var result = await _mediator.Send(new CreatePerson()
            {
                Name = name
            });

            return Ok(result);
        }
    }
}