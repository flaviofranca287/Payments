using System.Net;
using System.Net.Mime;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.PaymentServices;
using Payments.Domain.Exceptions;
using Payments.WebApi.Controllers.DataContracts;
using Payments.WebApi.Controllers.DataContracts.Mapper;

namespace Payments.WebApi.Controllers;

[ApiController]
public class PaymentsController : ControllerBase
{
    [HttpPost("/payments")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult> CreatePayment(
        [FromBody] InsertPaymentRequest request,
        [FromServices] IPaymentService paymentService,
        [FromServices] IValidator<InsertPaymentRequest> validator)
    {
        var result = await validator.ValidateAsync(request);
        if (result.IsValid == false) return BadRequest(result.Errors);

        var operation = request.ToInsertPaymentOperation();

        try
        {
            await paymentService.Insert(operation);
        }
        catch (UnregisteredException e)
        {
            return BadRequest(e.Message);
        }

        return StatusCode((int)HttpStatusCode.Created);
    }
}