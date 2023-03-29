using Microsoft.EntityFrameworkCore;
using TradingViewBot.BackgroundServices;
using TradingViewBot.Data.Contexts;
using TradingViewBot.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ITelegramMessageService, TelegramMessageService>();
// Add services to the container.
builder.Services.AddDbContext<BotContexts>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddHostedService<BigParaBackgroundService>();
//builder.Services.AddHostedService<BotBackgroundService>();
//builder.Services.AddHostedService<RSIBackgroundService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
