using System.Net;
using System.Net.Mime;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.ClientServices;
using Payments.Application.CompanyServices;
using Payments.WebApi.Controllers.DataContracts;
using Payments.WebApi.Controllers.DataContracts.Mapper;

namespace Payments.WebApi.Controllers;

[ApiController]
public class CompaniesController : ControllerBase
{
    [HttpPut("companies/register/{documentNumber}")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult> Upsert(
        [FromRoute] string documentNumber,
        [FromBody] UpsertCompanyRequest request,
        [FromServices] ICompanyService companyService,
        [FromServices] IValidator<UpsertCompanyRequest> validator)
    {
        if (string.IsNullOrWhiteSpace(documentNumber) 
            || documentNumber.Length != 11 && documentNumber.Length != 14 ||
            !documentNumber.All(char.IsDigit))
            return BadRequest("Invalid DocumentNumber. It must be either 11 or 14 numeric digits.");

        var result = await validator.ValidateAsync(request);
        if (result.IsValid == false) return BadRequest(result.Errors);

        var upsertMdrFeeOperation = request.ToUpsertCompanyOperation(documentNumber);

        await companyService.Upsert(upsertMdrFeeOperation);
        return StatusCode((int)HttpStatusCode.Created);
    }
}