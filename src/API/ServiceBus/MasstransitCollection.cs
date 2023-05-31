using ELibrary_UserService.Consumers;
using ELibrary_UserService.ServiceBus;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_UserService.RabbitMq
{
    public static class MasstransitCollection
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMessagePublisher, MessagePublisher>();

            services.AddMassTransit(x =>
            {
                // add consumers
                x.AddConsumer<UserCreatedConsumer>();
                x.AddConsumer<UserCreatedUConsumer>();
                x.AddConsumer<UserDeletedConsumer>();
                x.AddConsumer<UserDeletedUConsumer>();


                if (configuration["Flags:UserRabbitMq"] == "1")   //todo change to preprocessor directive #if
                {
                    RabbitMqOptions rabbitMqOptions = configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();
                    x.UsingRabbitMq((hostContext, cfg) =>
                    {
                        cfg.Host(rabbitMqOptions.Uri, "/", c =>
                        {
                            c.Username(rabbitMqOptions.UserName);
                            c.Password(rabbitMqOptions.Password);
                        });

                        // Consumers Configuration
                        cfg.ConfigureEndpoints(hostContext);
                    });
                }
                else
                {
                    // Azure Basic Tier - only 1-1 queues
                    x.UsingAzureServiceBus((context, cfg) =>
                    {
                        cfg.Host(configuration["AzureServiceBusConnectionString"]);

                        /// Publisher configuration ///
                        EndpointConvention.Map<UserBlocked>(new Uri($"queue:{nameof(UserBlocked)}"));
                        cfg.Message<UserBlocked>(cfgTopology => cfgTopology.SetEntityName(nameof(UserBlocked)));
                       
                        EndpointConvention.Map<UserUnblocked>(new Uri($"queue:{nameof(UserUnblocked)}"));
                        cfg.Message<UserUnblocked>(cfgTopology => cfgTopology.SetEntityName(nameof(UserUnblocked)));


                        /// Consumers configuration ///
                        // usercreated
                        cfg.ReceiveEndpoint("usercreated", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<UserCreatedConsumer>(context);

                        });
                        cfg.ReceiveEndpoint("usercreatedu", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<UserCreatedUConsumer>(context);

                        });

                        // userdeleted
                        cfg.ReceiveEndpoint("userdeleted", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<UserDeletedConsumer>(context);

                        });
                        cfg.ReceiveEndpoint("userdeletedu", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<UserDeletedUConsumer>(context);

                        });
                    });
                }

            });

            return services;
        }
    }
}
