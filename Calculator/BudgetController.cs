using Microsoft.AspNetCore.Mvc;

namespace Calculator;

[ApiController]
[Route("api/budget")]
public class BudgetController : ControllerBase
{
    [HttpPost("calculate")]
    public IActionResult Calculate([FromBody] BudgetRequest request)
    {
        if (request.Days <= 0)
        {
            return BadRequest("Количество дней должно быть больше нуля.");
        }

        decimal result = request.DailyCost * request.Days;

        if (request.IncludeInsurance)
            {
                result = result + result * 0.05m;
            }
        else
            {
                result = result * 1;
            }


        return Ok(new { TotalAmount = result });
    }
}
