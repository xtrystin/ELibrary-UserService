using MassTransit;

namespace ELibrary_UserService.RabbitMq
{
    public static class RabbitMqCollection
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            RabbitMqOptions rabbitMqOptions = configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((hostContext, cfg) =>
                {
                    cfg.Host(rabbitMqOptions.Uri, "/", c =>
                    {
                        c.Username(rabbitMqOptions.UserName);
                        c.Password(rabbitMqOptions.Password);
                    });
                });
            });

            return services;
        }
    }
}
