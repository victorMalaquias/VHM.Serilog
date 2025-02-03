
using VHM.Nuget.Serilog.Configurations.ExceptionHandler;
using VHM.Nuget.Serilog.Configurations.Extensions;

namespace VHM.Nuget.Serilog;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Health Checks
        builder.Services.AddHealthChecks();

        //Logging Configuration
        builder.AddLogSettings("EventAPI", builder.Configuration, useConsole: true, useSeq: true);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        //Health Checks
        app.MapHealthChecks("/healthz");

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
