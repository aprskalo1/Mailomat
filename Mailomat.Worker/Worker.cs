using Mailomat.Integrations.SudReg.Interfaces;
using Mailomat.Integrations.SudReg.Models;

namespace Mailomat.Worker
{
    public class Worker(
        ILogger<Worker> logger,
        ISubjektiService subjektiService) : BackgroundService
    {
        // DI will inject ILogger<Worker> and ISubjektiService

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            // Example: fetch once at startup, then exit
            try
            {
                const int snapshotId = 42;
                const int offset = 0;
                const int limit = 100;
                const bool onlyActive = true;

                var subjekti
                    = await subjektiService.GetSubjektiAsync(
                        snapshotId, offset, limit
                    ).ConfigureAwait(false);

                foreach (var s in subjekti)
                {
                    logger.LogInformation(
                        "OIB={Oib}, MBS={Mbs}, DatumOsnivanja={Datum}",
                        s.Oib,
                        s.Mbs,
                        s.DatumOsnivanja.ToString("yyyy-MM-dd")
                    );
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching subjekti");
            }

            // If you want this Worker to run periodically instead of just once:
            // while (!stoppingToken.IsCancellationRequested)
            // {
            //     // repeat the same logic, then
            //     await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            // }
        }
    }
}