using System.Data;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using TraceId.Demo.Configurations;

namespace WebAPI;

public static class ConfigureLogging
{
    public static IServiceCollection ConfigLogging(this WebApplicationBuilder builder)
    {
        var config = GetDemoConfig(builder.Configuration);

        builder.UseSerilog(config);


        return builder.Services;
    }

    private static void UseSerilog(this WebApplicationBuilder builder, TraceIdDemoConfig config)
    {
        var minimumLevel = GetLogEventLevel(config.Logging.MinimumLogLevel);

        var colOptions = new ColumnOptions();
        colOptions.Store.Remove(StandardColumn.MessageTemplate);

        colOptions.AdditionalColumns = new List<SqlColumn>
        {
            new SqlColumn { DataType = SqlDbType.VarChar, ColumnName = "RequestPath" },
            new SqlColumn { DataType = SqlDbType.VarChar, ColumnName = "SourceContext" },
            new SqlColumn { DataType = SqlDbType.VarChar, ColumnName = "TraceId" },
        };

        builder.Host.UseSerilog((context, configuration) => configuration
            .MinimumLevel.Is(minimumLevel)
            .Enrich.FromLogContext()
            .WriteTo.Async(
                x => x.MSSqlServer(
                    connectionString: config.Logging.DefaultConnectionString.DefaultConnection,
                    columnOptions: colOptions,
                    restrictedToMinimumLevel: minimumLevel,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "_logs", AutoCreateSqlTable = true }))
            .WriteTo.Console());
    }

    private static TraceIdDemoConfig GetDemoConfig(IConfiguration configuration)
    {
        var config = new TraceIdDemoConfig();
        configuration.GetSection(TraceIdDemoConfig.ConfigName).Bind(config);

        return config;
    }

    private static LogEventLevel GetLogEventLevel(string logLevel)
    {
        var minimumLogLevel =
            string.IsNullOrWhiteSpace(logLevel) ?
        LogEventLevel.Information : Enum.Parse<LogEventLevel>(logLevel);

        return minimumLogLevel;
    }
}
