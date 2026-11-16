namespace OrderService.Responses;

public sealed record CreateOrderResponse(Guid OrderId, OrderStatus Status, string Details);
