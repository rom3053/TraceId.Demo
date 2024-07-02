namespace TraceId.Demo.RabbitMQ.Configurations;

public class RabbitMqConfig
{
    public static string ConfigName => "RabbitMq";

    public string ConnectionString { get; set; } = string.Empty;

    public string RecievedDemoQueue { get; set; } = "RecievedDemoQueue";

    public string RequestClientDemoExchange { get; set; } = "RequestClientDemoExchange";
}