using Discount;
using Discount.Extensions;
using Discount.Features.Discount;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.MigrateDatabase();

app.MapGrpcService<GrpcDiscountService>();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();
