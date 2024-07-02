using MassTransit;
using TraceId.Demo.MassTransit.Middlewares.TraceIdMiddlewares;
using TraceId.Demo.MassTransit.Services;
using TraceId.Demo.RabbitMQ.Configurations;
using TraceId.Demo.RabbitMQ.MassTransit.Consumers;
using TraceId.Demo.RabbitMQ.MassTransit.Requests;

namespace TraceId.Demo.RabbitMQ;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConfig = GetRabbitMqConfig(configuration);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<RequestClientDemoConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseSendFilter(typeof(TraceIdSendMiddleware<>), context);
                cfg.UseConsumeFilter(typeof(TraceIdConsumeMiddleware<>), context);

                cfg.Message<RequestClientDemoRequest>(x => x.SetEntityName(rabbitMqConfig.RequestClientDemoExchange));
                cfg.ReceiveEndpoint(rabbitMqConfig.RequestClientDemoExchange, e =>
                {
                    e.ConfigureConsumer<RequestClientDemoConsumer>(context);
                });

                cfg.Host(rabbitMqConfig.ConnectionString);
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<TraceIdService>();

        return services;
    }

    private static RabbitMqConfig GetRabbitMqConfig(IConfiguration configuration)
    {
        var config = new RabbitMqConfig();
        configuration.GetSection(RabbitMqConfig.ConfigName).Bind(config);

        return config;
    }
}
