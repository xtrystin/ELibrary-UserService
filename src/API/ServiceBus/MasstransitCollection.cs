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
                x.AddConsumer<BookCreatedConsumer>();
                x.AddConsumer<BookCreatedUConsumer>();

                x.AddConsumer<BookRemovedConsumer>();
                x.AddConsumer<BookRemovedUConsumer>();

                x.AddConsumer<OvertimeReturnConsumer>();
                x.AddConsumer<OvertimeReturnUConsumer>();

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
                        EndpointConvention.Map<UserBlockedBr>(new Uri($"queue:{nameof(UserBlockedBr)}"));
                        cfg.Message<UserBlockedBr>(cfgTopology => cfgTopology.SetEntityName(nameof(UserBlockedBr)));
                       
                        EndpointConvention.Map<UserUnblockedBr>(new Uri($"queue:{nameof(UserUnblockedBr)}"));
                        cfg.Message<UserUnblockedBr>(cfgTopology => cfgTopology.SetEntityName(nameof(UserUnblockedBr)));


                        /// Consumers configuration ///
                        cfg.ReceiveEndpoint("bookcreatedu", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<BookCreatedUConsumer>(context);

                        });
                        cfg.ReceiveEndpoint("bookremovedu", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<BookRemovedUConsumer>(context);

                        });
                        cfg.ReceiveEndpoint("overtimereturnu", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<OvertimeReturnUConsumer>(context);

                        }); 
                        cfg.ReceiveEndpoint("usercreatedu", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<UserCreatedUConsumer>(context);
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
