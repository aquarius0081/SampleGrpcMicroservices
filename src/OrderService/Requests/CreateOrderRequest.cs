namespace OrderService.Requests;

public sealed record CreateOrderRequest(string ProductId, int Quantity);