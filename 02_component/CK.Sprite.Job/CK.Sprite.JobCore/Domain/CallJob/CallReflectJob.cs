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
using System.Threading.Tasks;

namespace CK.Sprite.JobCore
{
    [DisallowConcurrentExecution]
    public class CallReflectJob : DomainService, IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var strJobParams = context.JobDetail.JobDataMap.GetString("Params");
                var strExecLocation = context.JobDetail.JobDataMap.GetString("ExecLocation");

                Type execType = Type.GetType(strExecLocation);
                var execObject = Activator.CreateInstance(execType);
                var execMethod = execType.GetMethod("Execute");
                object[] execParams = new object[] { strJobParams };

                Task execTask = execMethod.Invoke(execObject, execParams) as Task;
                await execTask;

                Logger.LogInformation($"{DateTime.Now}{strExecLocation} CallReflectJob executed");
            }
            catch (Exception ex)
            {
                throw new JobExecutionException(ex);
            }
        }
    }
}
