using BabacusAPI.Model;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var BabacusConnectionstring = builder.Configuration.GetConnectionString("BabacusDb");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
builder.Services.AddDbContext<BabacusDb>(options => options.UseSqlServer(BabacusConnectionstring));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
