using AlbumsSite.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite.Base
{
    public interface IBaseJobService
    {
        Task<DateTime> ExecuteAsync(DateTime nextRun);
    }
    public class BaseJobService : IBaseJobService
    {
        private readonly IJob _job;

        //"0 1/1 * * *"
        public BaseJobService(IJob job, IServiceProvider serviceProvider)
        {
            _job = job;
        
        }

        public async Task<DateTime> ExecuteAsync( DateTime nextRun)
        {
            if (DateTime.Now > nextRun)
            { 
                await _job.ExecuteAsync();

            }
             nextRun = CrontabSchedule.Parse("0 1/1 * * *").GetNextOccurrence(DateTime.Now);
            return nextRun;
        }      
       
    }
}
