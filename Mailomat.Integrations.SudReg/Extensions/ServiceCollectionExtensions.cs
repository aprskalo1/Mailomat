using Mailomat.Integrations.SudReg.Clients;
using Mailomat.Integrations.SudReg.Interfaces;
using Mailomat.Integrations.SudReg.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mailomat.Integrations.SudReg.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSudRegIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            services.AddSingleton<ApiClientProvider>();
            services.AddSingleton<ISubjektiService, SubjektiService>();

            return services;
        }
    }
}