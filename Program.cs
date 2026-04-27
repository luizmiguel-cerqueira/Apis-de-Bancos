using api_para_banco.Controllers;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using api_para_banco.Services;
using api_para_banco.model.EF;

var tipo = new Filtro();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().
AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}
);
//Criação do modelo do JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option => 
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"])),
    };

});
//

//adicionar o serviço de TransferServices para ser injetado nas controllers, e também o serviço de Utilidade e SistemaFinanceiroContext para serem usados na TransferServices
builder.Services.AddScoped<IAccountServices, AccountServices>();
builder.Services.AddScoped<ITransferServices, TransferServices>();
builder.Services.AddScoped<IAccountManagerServices, AccountManagerServices>();
builder.Services.AddScoped<SistemaFinanceiroContext>();
builder.Services.AddScoped<ISafesServices, SafesServices>();
builder.Services.AddDbContext<SistemaFinanceiroContext>();
//

//fazer bind no obj SqlServerModel;
builder.Services.AddOptions<SqlServerModel>()
    .BindConfiguration(SqlServerModel.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();
object value = builder.Services.AddHttpClient<ClienteControllerV3>(c => 
{
    c.BaseAddress = new Uri("http://localhost:5021");
}).AddTransientHttpErrorPolicy( policy => policy.WaitAndRetryAsync(3,_ => TimeSpan.FromSeconds(Math.Pow(2,_))))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(3,TimeSpan.FromSeconds(30)));
// Para usar o Entity Framework, foi necessário adcionar o serviço dp dbContext, passando a string de conexão do banco de dados para o construtor do SistemaFinanceiroContext
builder.Services.AddDbContext<SistemaFinanceiroContext>
(
    (ServiceProvider, options) =>
    {
        var config = ServiceProvider.GetRequiredService<IOptions<SqlServerModel>>()
        .Value;
        options.UseSqlServer(config.ConnectionString);
    }
);



//Para usar o versinamento de api
builder.Services.AddApiVersioning(opitions => 
{
    opitions.DefaultApiVersion = new ApiVersion(1, 0);
    opitions.AssumeDefaultVersionWhenUnspecified = true;

}).AddMvc(); 
//Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Fazer bind no obj ClasseCon para pegar string de conexão do appSettings;
//deixar aq pq vai que eu decido fazer dnv
//

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
