﻿using ISA.Core.Domain.Contracts.Repositories;
using ISA.Core.Domain.UseCases.Company;
using ISA.Core.Domain.UseCases.User;
using ISA.Core.Infrastructure.HttpClients;
using ISA.Core.Infrastructure.Identity.Entities;
using ISA.Core.Infrastructure.Identity.Services;
using ISA.Core.Infrastructure.Persistence.PostgreSQL.Repositories;
using ISA.Core.Infrastructure.Persistence.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.UseCases.LoyaltyProgram;
using ISA.Core.Domain.UseCases.Company;
using ISA.Core.Domain.UseCases.Reservation;
using ISA.Core.Domain.UseCases.Contract;
using ISA.Core.Domain.BackgroundTasks;

namespace ISA.Application.API.Startup.DI;

public static class DIConfig
{
    public static IServiceCollection AddDICconfig(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ICompanyRepository, CompanyRepository>();
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<IIdentityServices, IdentityServices>();
        services.AddTransient<IISAUnitOfWork, ISAUnitOfWork>();
        services.AddTransient<ITokenGenerator, JwtGenerator>();
        services.AddTransient<IHttpClientService, HttpClientService>();
        services.AddTransient<ILoyaltyProgramRepository, LoyaltyProgramRepository>();
        services.AddTransient<ILoyaltyProgramService, LoyaltyProgramService>();
        services.AddTransient<ICompanyService, CompanyService>();
        services.AddTransient<ICompanyAdminRepository, CompanyAdminRepository>();
        services.AddTransient<IAppointmentRepository, AppointmentRepository>();
        services.AddTransient<ICompanyService,CompanyService>();
        services.AddTransient<IEquipmentRepository, EquipmentRepository>();
        services.AddTransient<IReservationRepository, ReservationRepository>();
        services.AddTransient<IReservationEquipmentRepository, ReservationEquipmentRepository>();
        services.AddTransient<IDocumentService, DocumentService>();
        services.AddTransient<IContractRepository, ContractRepository>();
        services.AddTransient<DocumentService>();
        services.AddTransient<UserService>();
        services.AddTransient<CompanyService>();
        services.AddTransient<AppointmentService>();
        services.AddTransient<EquipmentService>();
        services.AddTransient<DeliverySimulatorService>();
        services.AddTransient<ReservationService>();
        services.AddTransient<ContractService>();
        services.AddTransient<UserManager<ApplicationUser>>();
        services.AddScoped<RoleManager<ApplicationRole>>();
        services.AddTransient<SignInManager<ApplicationUser>>();

        return services;
    }
}
