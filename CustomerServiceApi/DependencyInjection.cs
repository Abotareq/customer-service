using CustomerService.Api.Common.Mapping;

namespace CustomerService.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiLayer(this IServiceCollection services)
        {


            services.AddMapping();


            return services;
        }
    }
}
