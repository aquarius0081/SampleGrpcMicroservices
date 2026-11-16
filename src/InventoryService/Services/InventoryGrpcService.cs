using Grpc.Core;
using Shared.Protos;
using System.Collections.Concurrent;

namespace InventoryService.Services;

public sealed class InventoryGrpcService : Shared.Protos.InventoryService.InventoryServiceBase
{
    private static readonly ConcurrentDictionary<string, int> Stock = new()
    {
        ["Desktop"] = 50,
        ["Laptop"] = 0 // Out of stock
    };

    public override Task<CheckAvailabilityResponse> CheckAvailability(
        CheckAvailabilityRequest request,
        ServerCallContext context)
    {
        if (!Stock.TryGetValue(request.ProductId, out var availableQuantity))
        {
            return Task.FromResult(new CheckAvailabilityResponse
            {
                IsAvailable = false,
                AvailableQuantity = 0,
                Message = $"Product with ID '{request.ProductId}' not found."
            });
        }

        if (availableQuantity >= request.Quantity)
        {
            return Task.FromResult(new CheckAvailabilityResponse
            {
                IsAvailable = true,
                AvailableQuantity = availableQuantity,
                Message = $"Product with ID '{request.ProductId}' has been reserved."
            });
        }

        return Task.FromResult(new CheckAvailabilityResponse
        {
            IsAvailable = false,
            AvailableQuantity = availableQuantity,
            Message = $"Not enough products with ID '{request.ProductId}'! Available quantity: {availableQuantity}."
        });
    }
}