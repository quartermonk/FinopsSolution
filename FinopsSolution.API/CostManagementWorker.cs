using FinopsSolution.API.Services;
using Microsoft.Extensions.Options;

namespace FinopsSolution.API;

internal class CostManagementWorker : BackgroundService
{
    private readonly PeriodicTimer _timer;
    private readonly CostManagementService _costManagementService;
    private readonly ILogger<CostManagementWorker> _logger;

    public CostManagementWorker(CostManagementService costManagementService,
                                IOptions<CostManagementWorkerSettings> options,
                                ILogger<CostManagementWorker> logger)
    {
        _timer = new PeriodicTimer(TimeSpan.FromMinutes(options.Value.JobIntervalInMinutes));
        _costManagementService = costManagementService ?? throw new ArgumentNullException(nameof(costManagementService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _costManagementService.InitializeAsync();
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            _logger.LogInformation("CostManagement job triggered.");
            await _costManagementService.CallSubscriptionManagement();
        }
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested);
    }

    public override void Dispose()
    {
        _timer.Dispose();
        base.Dispose();
    }
}
