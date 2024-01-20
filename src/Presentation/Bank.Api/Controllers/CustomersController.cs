﻿using Bank.Api.DTOs;
using Bank.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        #region Fields
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        #region Endpoints
        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerDto CreateCustomerDto, CancellationToken cancellationToken = default)
        {
            if (null == CreateCustomerDto)
                return BadRequest();
            var command = new CreateCustomer(Guid.NewGuid(), CreateCustomerDto.FirstName, CreateCustomerDto.LastName, CreateCustomerDto.Email);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
        [HttpGet, Route("{id:guid}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer(Guid id, CancellationToken cancellationToken = default)
        {
            //var query = new CustomerById(id);
            //var result = await _mediator.Send(query, cancellationToken);
            //if (null == result)
            //    return NotFound();
            //return Ok(results);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            //var query = new CustomersArchive();
            //var results = await _mediator.Send(query, cancellationToken);
            //if (null == results)
            //    return NotFound();
            //return Ok(results);
            return Ok();
        }
        #endregion
    }
}
