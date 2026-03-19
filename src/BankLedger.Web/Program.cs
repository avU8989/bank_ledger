using BankLedger.App;
using BankLedger.Core.Policies;
using BankLedger.Core.Common.Factories;
using MediatR;
using BankLedger.App.Ports;

using BankLedger.Infrastructure.Accounts;
using BankLedger.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AppAssemblyMaker>());

builder.Services.AddSingleton<IAccountPolicy, DefaultPolicy>();
builder.Services.AddScoped<AccountFactory>();

//just for development
builder.Services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
builder.Services.AddSingleton<IIbanGenerator, SimpleIbanGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AppAssemblyMaker>());


app.Run();

