namespace TraceId.Demo.Configurations;

public class LoggerConfig
{
    public string MinimumLogLevel { get; set; } = "Warning";

    public DefaultConnectionStringConfig? DefaultConnectionString { get; set; }
}
