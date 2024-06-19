using Acef.Reasons.API.Extensions;
using Acef.Reasons.ApplicationCore.Entities.Profiles;
using Acef.Reasons.ApplicationCore.Interfaces;
using Acef.Reasons.ApplicationCore.Services;
using Acef.Reasons.Infrastructure;
using Acef.Reasons.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ReasonsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection")));

builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
builder.Services.AddScoped<IReasonService, ReasonService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API - Consultation Reasons Manager",
        Version = "v1",
        Description = "API for managing consultation reasons for ACEF",
        License = new OpenApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("http://www.apache.org")
        },
        Contact = new OpenApiContact
        {
            Name = "Équipe 3",
            Email = "équipe3@example.com",
            Url = new Uri("https://github.com/lcavalera/")
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddAutoMapper(c => c.AddProfile<MappingProfile>());

// Enable CORS (Cross Origin Resource Sharing)
var web2UIOrigins = "_enableCORS";

builder.Services.AddCors(options =>
    options.AddPolicy(name: web2UIOrigins,
        policy =>
        {
            policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        }
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseCors("_enableCORS");

app.UseAuthorization();

app.MapControllers();

// Generate data
app.CreateDbIfNotExists();

app.Run();
