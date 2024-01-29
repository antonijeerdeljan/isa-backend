namespace ISA.Application.API.Startup;

public static class CorsConfig
{

    public static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("frontend-policy", opts =>
            {
                opts.WithOrigins("http://localhost:4200", "https://localhost:5104", "http://localhost:5105", "https://localhost:3000")
                .AllowAnyHeader()
                .AllowCredentials()
                .AllowAnyMethod();
            });
        });

        return services;
    }
}
