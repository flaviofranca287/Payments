using System.Net;
using System.Net.Mime;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.ClientServices;
using Payments.WebApi.Controllers.DataContracts;
using Payments.WebApi.Controllers.DataContracts.Mapper;

namespace Payments.WebApi.Controllers;

[ApiController]
public class ClientsController : ControllerBase
{
    [HttpPut("clients/register/{plate}")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult> Upsert(
        [FromRoute] string plate,
        [FromBody] UpsertClientRequest request, 
        [FromServices] IClientsService clientsService,
        [FromServices] IValidator<UpsertClientRequest> validator)
    {
        if (plate.Length != 7) return BadRequest("Invalid plate");
        
        var result = await validator.ValidateAsync(request);
        if (result.IsValid == false) return BadRequest(result.Errors);

        var upsertMdrFeeOperation = request.ToUpsertClientOperation(plate);

        await clientsService.Upsert(upsertMdrFeeOperation);
        return StatusCode((int)HttpStatusCode.Created);
    }
}