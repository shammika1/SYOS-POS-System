using Microsoft.AspNetCore.Server.Kestrel.Core;
using SYOS.Server.DataGateway;
using SYOS.Server.Hubs;
using SYOS.Server.Services;
using SYOS.Shared.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenLocalhost(5000, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        listenOptions.UseHttps();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Gateways
builder.Services.AddSingleton<ItemGateway>();
builder.Services.AddSingleton<StockGateway>();
builder.Services.AddSingleton<ShelfGateway>();
builder.Services.AddSingleton<BillGateway>();
builder.Services.AddSingleton<BillItemGateway>();

// Register Services
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IShelfService, ShelfService>();
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<IReportService, ReportService>();



builder.Services.AddSignalR();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<SYOSHub>("/syoshub");
app.Run();