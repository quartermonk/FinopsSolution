using FinopsSolution.Service.APIs.CostManagement;
using FinopsSolution.Service.Utilities;

namespace FinopsSolution.API
{
    public class CostManagementWorker : IHostedService, IDisposable
    {
        private Timer _timer = null;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(RunCostManagementService, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(Utils.RunCostManagemementApiInSeconds));

            //_timer = new Timer(RunListResourceGroupService, null, TimeSpan.Zero,
            //TimeSpan.FromSeconds(Utils.RunCostManagemementApiInSeconds));

            return Task.CompletedTask;
        }


        private void RunCostManagementService(object state)
        {
            
            var task = Task.Run(async () => await CostManagementService.callSubscriptionManagement());
            //var task2 = Task.Run(async () => await CostManagementService.callResourceGroupManagement());
            task.Wait();
            //task2.Wait();
        }

        //private void RunListResourceGroupService(object state)
        //{

        //    var task = Task.Run(async () => await CostManagementService.AzureListResourceGroups());
        //    task.Wait();
        //}

        public Task StopAsync(CancellationToken cancellationToken)
        {
            

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
