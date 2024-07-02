namespace TraceId.Demo.Configurations;

public class TraceIdDemoConfig
{
    public static string ConfigName => "TraceIdDemo";

    public LoggerConfig Logging { get; set; }
}
