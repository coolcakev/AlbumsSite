using AlbumsSite.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlbumsSite.Jobs
{
    public class ScopedBackgroundService : BackgroundService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDeletePhotoJobService _deletePhotoJobService;
        private DateTime nextRun;

        public ScopedBackgroundService(
             IServiceProvider serviceProvider,
            IWebHostEnvironment environment)
        {
            _environment = environment;
            _serviceProvider = serviceProvider;
          
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                _deletePhotoJobService = scope.ServiceProvider.GetRequiredService<IDeletePhotoJobService>();
                      
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Console.WriteLine($"{nameof(ScopedBackgroundService)} is running.");
            await DoWorkAsync(stoppingToken);
        }
        private async Task StartAsyncJobs()
        {
            nextRun=await _deletePhotoJobService.ExecuteAsync(nextRun);
        }
        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            if (_environment.IsDevelopment())
                return;
            nextRun = new DateTime();
            do
            {
                Console.WriteLine($"{nameof(ScopedBackgroundService)} is working.");
         
               
                await StartAsyncJobs();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); //1 hour delay
            } while (!stoppingToken.IsCancellationRequested);
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"{nameof(ScopedBackgroundService)} is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
