using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text.Json;
using VHM.Nuget.Serilog.Configurations.ExceptionHandler;
using VHM.Serilog.Nuget;

namespace VHM.Nuget.Serilog.Configurations.Extensions
{
    public static class LogSettings
    {
        public static void AddLogSettings(this WebApplicationBuilder builder, string applicationName, IConfiguration configuration, bool useConsole = false, bool useSeq = false, bool useSqlServer = false, bool useMongoDb = false)
        {
            var logOptions = configuration.GetSection("LoggingOptions").Get<LogOptions>();
            if (logOptions == null) throw new ArgumentNullException("LoggingOptions section is required in appsettings.json");

            Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

            builder.Logging.ClearProviders();
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationName", applicationName)
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.WithMemoryUsage()
                .Enrich.FromLogContext();

            if (useConsole)
                logger.WriteTo.Console(outputTemplate: "{Timestamp:yyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}");

            if (useSeq && !string.IsNullOrWhiteSpace(logOptions.SeqUrl))
                logger.WriteTo.Seq(logOptions.SeqUrl, restrictedToMinimumLevel: LogEventLevel.Error);

            if (useSqlServer && !string.IsNullOrWhiteSpace(logOptions.SqlServerConnection))
            {
                logger.WriteTo.MSSqlServer(
                    logOptions.SqlServerConnection,
                    new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
                    restrictedToMinimumLevel: LogEventLevel.Information);
            }

            if (useMongoDb && !string.IsNullOrWhiteSpace(logOptions.MongoDbConnection))
            {
                var mongoClient = new MongoClient(logOptions.MongoDbConnection);
                var database = mongoClient.GetDatabase(logOptions.MongoDbDatabase);
                logger.WriteTo.MongoDB(database.GetCollection<BsonDocument>("Logs").ToString());
            }

            builder.Logging.AddSerilog(logger.CreateLogger());
            builder.Services.AddTransient<GlobalException>();
            builder.Services.AddExceptionHandler<GlobalException>();
            builder.Services.AddProblemDetails();
        }
    }  
}
