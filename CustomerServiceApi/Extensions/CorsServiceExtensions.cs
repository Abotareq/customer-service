namespace CustomerService.Api.Extensions
{
    public static class CorsServiceExtensions
    {
        public const string TestPolicyName = "TestPolicy";

        public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(TestPolicyName, policy =>
                {
                    policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500", "null")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            return services;
        }
    }
}
