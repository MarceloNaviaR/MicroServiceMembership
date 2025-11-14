using MicroserviceMembership.Application.Interfaces;
using MicroserviceMembership.Application.Services;
using MicroserviceMembership.Domain.Ports;
using MicroserviceMembership.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceMembership.Infrastructure.DependencyInjection
{
    public static class MembershipModuleServiceCollectionExtensions
    {
        public static IServiceCollection AddMembershipModule(this IServiceCollection services)
        {
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IMembershipService, MembershipService>();
            return services;
        }
    }
}