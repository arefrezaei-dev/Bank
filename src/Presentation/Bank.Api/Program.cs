using Bank.Api.Registries;
using Bank.Domain.DomainServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.ConfigureMediatRPipelines();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ICurrencyConverter, FakeCurrencyConverter>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
