using api_para_banco.Controllers;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using api_para_banco.Infrastructure.model;

var tipo = new Filtro();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().
AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}
);
//polly para retry e circuit breaker, para o cliente http do controller v2
builder.Services.AddHttpClient<ClienteControllerV2cs>(c => 
{
    c.BaseAddress = new Uri("http://localhost:5021");
}).AddTransientHttpErrorPolicy( 
    policy => policy.WaitAndRetryAsync(3,_ => TimeSpan.FromSeconds(Math.Pow(2,_))))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(3,TimeSpan.FromSeconds(30)));

//fazer bind no obj SqlServerModel;
builder.Services.AddOptions<SqlServerModel>()
    .BindConfiguration(SqlServerModel.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();
// Para usar o Entity Framework, foi necessário adcionar o serviço dp dbContext, passando a string de conexão do banco de dados para o construtor do EntityFrameWorkModel
builder.Services.AddDbContext<EntityFrameWorkModel> ((
    ServiceProvider, options) =>
    {
        var config = ServiceProvider.GetRequiredService<IOptions<SqlServerModel>>()
        .Value;
        options.UseSqlServer(config.ConnectionString);
    });



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
