using MassTransit;
using TraceId.Demo.Configurations;
using TraceId.Demo.Dtos.MassTransit;
using TraceId.Demo.Dtos.MassTransit.Requests;
using TraceId.Demo.MassTransit.Consumers;
using TraceId.Demo.MassTransit.Middlewares.TraceIdMiddlewares;
using TraceId.Demo.MassTransit.Services;
using TraceId.Demo.Services;
using WebAPI.Middlewares;

namespace TraceId.Demo;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConfig = GetRabbitMqConfig(configuration);

        services.AddMassTransit(x =>
        {
            x.AddRequestClient<RequestClientDemoRequest>();

            x.AddConsumer<MessageConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UsePublishFilter(typeof(TraceIdPublishMiddleware<>), context);
                cfg.UseSendFilter(typeof(TraceIdSendMiddleware<>), context);
                cfg.UseConsumeFilter(typeof(TraceIdConsumeMiddleware<>), context);

                cfg.Message<RequestClientDemoRequest>(x => x.SetEntityName(rabbitMqConfig.RequestClientDemoExchange));

                ////Raw JSON
                //cfg.ReceiveEndpoint(rabbitMqConfig.RecievedDemoQueue, e =>
                //{
                //    e.ConfigureConsumeTopology = false;
                //    e.ClearSerialization();
                //    e.UseRawJsonSerializer();
                //    e.UseRawJsonDeserializer();
                //    e.PrefetchCount = 1;
                //    e.UseMessageRetry(r => r.Interval(2, 3000));
                //    e.ConfigureConsumer<MessageConsumer>(context);
                //});

                cfg.Host(rabbitMqConfig.ConnectionString);
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddScoped<TraceIdService>();
        services.AddScoped<MassTransitService>();

        return services;
    }

    private static RabbitMqConfig GetRabbitMqConfig(IConfiguration configuration)
    {
        var config = new RabbitMqConfig();
        configuration.GetSection(RabbitMqConfig.ConfigName).Bind(config);

        return config;
    }
}