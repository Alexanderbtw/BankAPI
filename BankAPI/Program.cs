using BankAPI;
using BankAPI.Database;
using BankAPI.Interfaces;
using BankAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddDbContext<BankContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.AddDbContext<AuditContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuditConnection"));
});
    
builder.Services.AddScoped<IConversionService>(service => new ConversionApi(builder.Configuration["Keys:Fixer"], service.GetRequiredService<IHttpClientFactory>()));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddControllers();

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(builder => builder.WithOrigins("https://localhost:44357").WithHeaders());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
