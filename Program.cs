using api_para_banco.Controllers;
using api_para_banco.model;
using Asp.Versioning;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var tipo = new Filtro();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().
AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}
);
// Para usar o Entity Framework, foi necessário adcionar o serviço dp dbContext, passando a string de conexão do banco de dados para o construtor do EntityFrameWorkModel
var conection = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<EntityFrameWorkModel>(
    options => options.UseSqlServer(conection));



//Para usar o versinamento de api
builder.Services.AddApiVersioning(opitions => 
{
    opitions.DefaultApiVersion = new ApiVersion(1, 0);
    opitions.AssumeDefaultVersionWhenUnspecified = true;

}).AddMvc(); 
//Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var classeCon = new ClasseCon();
builder.Configuration.GetConnectionString("Default");
builder.Services.AddSingleton(classeCon);

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
