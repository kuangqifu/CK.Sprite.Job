using CK.Sprite.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.JobCore
{
    [DisallowConcurrentExecution]
    public class CallApiJob : DomainService, IJob
    {
        public CallApiConfig CallApiConfig => LazyGetRequiredService(ref _callApiConfig).Value;
        private IOptions<CallApiConfig> _callApiConfig;

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var strJobParams = context.JobDetail.JobDataMap.GetString("Params");

                ECallLocationHostType callLocationHostType = ECallLocationHostType.GateWay;
                var methodParams = string.Empty;
                if (!string.IsNullOrEmpty(strJobParams))
                {
                    var callApiParam = JsonConvert.DeserializeObject<CallApiParam>(strJobParams);
                    callLocationHostType = callApiParam.CallLocationHostType;
                    methodParams = callApiParam.Params;
                }

                using (var client = new HttpClient())
                {
                    var url = GetCallUrl(callLocationHostType, context.JobDetail.JobDataMap.GetString("ExecLocation"));

                    var response = await client.PostAsync(
                        url,
                        new StringContent(
                            methodParams,
                            Encoding.UTF8,
                            "application/json"
                        )
                    );

                    if (response.IsSuccessStatusCode)
                    {
                        Logger.LogInformation($"{DateTime.Now}CallApiJob executed，{CallApiConfig.GateWayHostUrl}");
                        var responseContent = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new SpriteException($"CallApiJob error: {JsonConvert.SerializeObject(response)}");
                    }
                }

            }
            catch(Exception ex)
            {
                throw new JobExecutionException(ex);
            }
        }

        private string GetCallUrl(ECallLocationHostType callLocationHostType, string strExecLocation)
        {
            if(strExecLocation.StartsWith("http"))
            {
                return strExecLocation;
            }

            switch (callLocationHostType)
            {
                case ECallLocationHostType.GateWay:
                    return $"{CallApiConfig.GateWayHostUrl}{strExecLocation.TrimStart('/')}";
                default:
                    return $"{CallApiConfig.GateWayHostUrl}{strExecLocation.TrimStart('/')}";
            }
        }
    }
}
