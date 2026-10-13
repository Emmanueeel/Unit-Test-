using Calculator;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Calculator.Tests;

public class BudgetTests
{
    [Fact]
    public void Calculate_WithoutInsurance_ReturnsCorrectTotal()
    {
        var budgetController = new BudgetController();

        var data = new BudgetRequest
        {
            DailyCost = 100,
            Days = 5,
            IncludeInsurance = false
        };

        var response = budgetController.Calculate(data);

        var okResponse = Assert.IsType<OkObjectResult>(response);

        var amount = okResponse.Value!
            .GetType()
            .GetProperty("TotalAmount")!
            .GetValue(okResponse.Value);

        Assert.Equal(500m, amount);
    }

    [Fact]
    public void Calculate_WithInsurance_AddsFivePercent()
    {
        var budgetController = new BudgetController();

        var data = new BudgetRequest
        {
            DailyCost = 100,
            Days = 5,
            IncludeInsurance = true
        };

        var response = budgetController.Calculate(data);

        var okResponse = Assert.IsType<OkObjectResult>(response);

        var amount = okResponse.Value!
            .GetType()
            .GetProperty("TotalAmount")!
            .GetValue(okResponse.Value);

        Assert.Equal(525m, amount);
    }

    [Fact]
    public void Calculate_NegativeDays_ReturnsBadRequest()
    {
        var budgetController = new BudgetController();

        var data = new BudgetRequest
        {
            DailyCost = 150,
            Days = -1,
            IncludeInsurance = false
        };

        var response = budgetController.Calculate(data);

        Assert.IsType<BadRequestObjectResult>(response);
    }

    [Fact]
    public void Calculate_ZeroCost_ReturnsZero()
    {
        var budgetController = new BudgetController();

        var data = new BudgetRequest
        {
            DailyCost = 0,
            Days = 7,
            IncludeInsurance = false
        };

        var response = budgetController.Calculate(data);

        var okResponse = Assert.IsType<OkObjectResult>(response);

        var amount = okResponse.Value!
            .GetType()
            .GetProperty("TotalAmount")!
            .GetValue(okResponse.Value);

        Assert.Equal(0m, amount);
    }

    [Fact]
    public void Calculate_DecimalPrice_CalculatesCorrectly()
    {
        var budgetController = new BudgetController();

        var data = new BudgetRequest
        {
            DailyCost = 100.50m,
            Days = 3,
            IncludeInsurance = false
        };

        var response = budgetController.Calculate(data);

        var okResponse = Assert.IsType<OkObjectResult>(response);

        var amount = okResponse.Value!
            .GetType()
            .GetProperty("TotalAmount")!
            .GetValue(okResponse.Value);

        Assert.Equal(301.50m, amount);
    }
}
