using Bank.Api.DTOs;
using Bank.Domain;
using Bank.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        #region Feilds
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        #region Endpoints
        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountDto createAccountDto, CancellationToken cancellationToken = default)
        {
            if (null == createAccountDto)
                return BadRequest();
            var currency=Currency.FromCode(createAccountDto.CurrencyCode);
            var command = new CreateAccount(createAccountDto.CustomerId, Guid.NewGuid(), currency);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
        #endregion
    }
}
