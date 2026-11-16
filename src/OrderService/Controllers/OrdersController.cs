using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderService.Requests;
using OrderService.Responses;
using Shared.Protos;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly InventoryService.InventoryServiceClient _inventoryClient;
    private readonly IValidator<CreateOrderRequest> _validator;

    public OrdersController(InventoryService.InventoryServiceClient inventoryClient, IValidator<CreateOrderRequest> validator)
    {
        _inventoryClient = inventoryClient;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
        }

        var checkResponse = await _inventoryClient.CheckAvailabilityAsync(new CheckAvailabilityRequest
        {
            ProductId = request.ProductId,
            Quantity = request.Quantity
        });

        if (!checkResponse.IsAvailable)
        {
            return BadRequest(new { Error = checkResponse.Message });
        }

        return Ok(new CreateOrderResponse
        (
            OrderId: Guid.NewGuid(),
            Status: OrderStatus.Created,
            Details: checkResponse.Message
        ));
    }
}
