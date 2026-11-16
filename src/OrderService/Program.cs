using FluentValidation;
using Shared.Protos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var inventoryServiceUrl = builder.Configuration["InventoryServiceUrl"]
    ?? throw new InvalidOperationException("InventoryServiceUrl key is missing in configuration!");

builder.Services.AddGrpcClient<InventoryService.InventoryServiceClient>(o =>
{
    o.Address = new Uri(inventoryServiceUrl);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
