using api_para_banco.Controllers;
using api_para_banco.model;
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
//Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var classeCon = new ClasseCon();
builder.Configuration.GetSection("strConexao").Bind(classeCon);
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
