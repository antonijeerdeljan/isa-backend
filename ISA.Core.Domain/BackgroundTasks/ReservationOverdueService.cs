using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.UseCases.Company;
using ISA.Core.Domain.UseCases.Reservation;
using ISA.Core.Domain.UseCases.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ISA.Core.Domain.BackgroundTasks;

public class ReservationOverdueService : IHostedService, IDisposable
{

    private readonly ILogger<ReservationOverdueService> _logger;
    private Timer? _timer = null;

    private IServiceScopeFactory _serviceScopeFactory;

    public ReservationOverdueService(ILogger<ReservationOverdueService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(30));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        _ = DoWorkAsync(); // Fire and forget
    }

    private async Task DoWorkAsync()
    {
        await CheckForOverdueReservations();
    }

    private async Task CheckForOverdueReservations()
    {
        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IISAUnitOfWork>();
            var reservationService = scope.ServiceProvider.GetRequiredService<ReservationService>();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();
            var equipmentService = scope.ServiceProvider.GetRequiredService<EquipmentService>();


            await unitOfWork.StartTransactionAsync();

            var overdueReservations = await reservationService.OverdueReservations();
            if(overdueReservations is not null)
            {
                foreach(var reservation in overdueReservations)
                {
                    await userService.GivePenaltyPoints(reservation.Customer.UserId, 2); // give user two penalty points
                    await equipmentService.ReturnEqupment(reservation.Equipments);
                    await reservationService.SetReservationAsOverdue(reservation.AppointmentId);
                }
            }

            await unitOfWork.SaveAndCommitChangesAsync();

        }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
