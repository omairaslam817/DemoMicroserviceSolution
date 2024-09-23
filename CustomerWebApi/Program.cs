using CustomerWebApi.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var dbHost = Environment.GetEnvironmentVariable("DB_HOST"); //"DESKTOP-516PDKQ\\SQLEXPRESS";
var dbName = Environment.GetEnvironmentVariable("DB_NAME"); //"dms_customer";
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD"); //"password@123#";

var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID =sa;Password=${dbPassword};Integrated Security=True;TrustServerCertificate=True;";
builder.Services.AddDbContext<CustomerDbContext>(opt => opt.UseSqlServer(connectionString));

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

app.UseAuthorization();
app.MapControllers();
app.Run();
