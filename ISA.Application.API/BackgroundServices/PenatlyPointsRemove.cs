using ISA.Core.Domain.Contracts.Services;
using ISA.Core.Domain.UseCases.User;

namespace ISA.Application.API.BackgroundServices
{
    public class PenaltyPointsRemoveService : IHostedService, IDisposable
    {
        private Timer _timer;
        private IServiceScopeFactory serviceScopeFactory;

        public PenaltyPointsRemoveService(IServiceScopeFactory _serviceScopeFactory)
        {
            serviceScopeFactory = _serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var today = DateTime.Now;
            if (today.Day == 1)
            {
                RemovePenaltyPoints(); 
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void RemovePenaltyPoints()
        {
            Console.WriteLine("Removing penalty points...");

            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IISAUnitOfWork>();
                var userService = scope.ServiceProvider.GetRequiredService<UserService>();


                unitOfWork.StartTransactionAsync();

                userService.RemovePenaltyPoints();

                unitOfWork.SaveAndCommitChangesAsync();

            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
