using ELibrary_UserService.Application.Command;
using ELibrary_UserService.Application.Query;

namespace ELibrary_UserService.Application
{
    public static class ProviderCollectionExtension
    {
        public static IServiceCollection AddProviderCollection(this IServiceCollection services)
        {
            // Read Providers
            services.AddScoped<IUserReadProvider, UserReadProvider>();

            // Write Providers
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<IBookProvider, BookProvider>();

            return services;
        }
    }
}
