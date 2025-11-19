using API.Application.Interface;
using API.Application.Service;
using API.Domain.Interface;
using API.Infraestructure.Data.AppData;
using API.Infraestructure.Data.Repositories;
using Asp.Versioning;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(x =>
{
    x.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
});

builder.Services.AddTransient<ITrilhaUsuarioUseCase, TrilhaUsuarioUseCase>();
builder.Services.AddTransient<ITrilhaUsuarioRepository, TrilhaUsuarioRepository>();

builder.Services.AddTransient<ITrilhaUseCase, TrilhaUseCase>();
builder.Services.AddTransient<ITrilhaRepository, TrilhaRepository>();

builder.Services.AddTransient<IConteudoTrilhaUsuarioUseCase, ConteudoTrilhaUsuarioUseCase>();
builder.Services.AddTransient<IConteudoTrilhaUsuarioRepository, ConteudoTrilhaUsuarioRepository>();

builder.Services.AddTransient<IConteudoTrilhaUseCase, ConteudoTrilhaUseCase>();
builder.Services.AddTransient<IConteudoTrilhaRepository, ConteudoTrilhaRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new QueryStringApiVersionReader("api-version")
    );
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            }),
            totalDuration = report.TotalDuration
        });

        await context.Response.WriteAsync(result);
    }
});

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
