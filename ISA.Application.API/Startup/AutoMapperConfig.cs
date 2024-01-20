using ISA.Application.API.Mappings;

namespace ISA.Application.API.Startup;

public static class AutoMapperConfig
{
    public static IServiceCollection MapperConfig(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        return services;
    }
}
