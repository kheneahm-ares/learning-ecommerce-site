using Discount.Features.Discount;
using Discount.Features.Discount.DTOs;
using Discount.Features.Shared;
using FluentValidation;

namespace Discount.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // register postgresdb
            services.AddSingleton<DapperContext>();

            // add logger via logger factory
            ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger logger = factory.CreateLogger<Program>();
            services.AddSingleton(logger);

            // services
            services.AddScoped<DiscountService>();

            // fluent validation
            services.AddScoped<IValidator<CreateDiscountRequest>, CreateDiscountRequestValidator>();
            services.AddScoped<IValidator<UpdateDiscountRequest>, UpdateDiscountRequestValidator>();

            return services;

        }
    }
}
