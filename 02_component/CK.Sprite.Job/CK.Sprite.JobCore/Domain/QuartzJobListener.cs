using CK.Sprite.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CK.Sprite.JobCore
{
    public class QuartzJobListener : IJobListener, ISingletonDependency
    {
        private readonly ILogger<QuartzJobListener> _logger;
        public QuartzJobListener(ILogger<QuartzJobListener> logger)
        {
            _logger = logger;
        }

        public string Name => "All Job Listener";

        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"JobExecutionVetoed:{context.JobDetail.Key.Name}#{context.JobDetail.Key.Group}");
            await Task.CompletedTask;
        }

        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"JobToBeExecuted:{context.JobDetail.Key.Name}#{context.JobDetail.Key.Group}");
            await Task.CompletedTask;
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"JobWasExecuted:{context.JobDetail.Key.Name}#{context.JobDetail.Key.Group}#{context.JobDetail.Description}#{context.PreviousFireTimeUtc?.LocalDateTime}#{context.FireTimeUtc.LocalDateTime}#{context.NextFireTimeUtc?.LocalDateTime}");
            await Task.CompletedTask;
        }
    }
}
