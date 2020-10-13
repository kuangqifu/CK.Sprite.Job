using CK.Sprite.Framework;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.JobCore
{
    public class JobManagerAppService : AppService, IJobManagerAppService
    {
        private readonly JobManagerDomainService _jobManagerDomainService;
        public JobManagerAppService(JobManagerDomainService jobManagerDomainService)
        {
            _jobManagerDomainService = jobManagerDomainService;
        }

        public async Task AddJob(JobConfigDto jobConfig)
        {
            ValidateJobConfig(jobConfig);
            await _jobManagerDomainService.AddJob(Mapper.Map<JobConfig>(jobConfig));
        }

        public async Task UpdateJob(JobConfigDto jobConfig)
        {
            ValidateJobConfig(jobConfig);
            await _jobManagerDomainService.UpdateJob(Mapper.Map<JobConfig>(jobConfig));
        }

        public async Task DeleteJob(Guid id)
        {
            await _jobManagerDomainService.DeleteJob(id);
        }

        public async Task ActiveJob(Guid id, bool isActive)
        {
            await _jobManagerDomainService.ActiveJob(id, isActive);
        }

        public async Task AddHolidayCalendar(HolidayCalendarDto holidayCalendar)
        {
            await _jobManagerDomainService.AddHolidayCalendar(Mapper.Map<HolidayCalendar>(holidayCalendar));
        }

        public async Task UpdateHolidayCalendar(HolidayCalendarDto holidayCalendar)
        {
            await _jobManagerDomainService.UpdateHolidayCalendar(Mapper.Map<HolidayCalendar>(holidayCalendar));
        }

        public async Task<List<JobConfig>> GetAllJobConfigs()
        {
            return await _jobManagerDomainService.GetAllJobConfigs();
        }

        public async Task<List<QuartzJobInfo>> GetAllQuartzJobConfigs()
        {
            return await _jobManagerDomainService.GetAllQuartzJobConfigs();
        }

        public async Task TriggerJob(Guid id)
        {
            await _jobManagerDomainService.TriggerJob(id);
        }

        public async Task PauseJob(Guid id)
        {
            await _jobManagerDomainService.PauseJob(id);
        }

        public async Task PauseAll()
        {
            await _jobManagerDomainService.PauseAll();
        }

        public async Task ResumeJob(Guid id)
        {
            await _jobManagerDomainService.ResumeJob(id);
        }

        public async Task ResumeAll()
        {
            await _jobManagerDomainService.ResumeAll();
        }

        private void ValidateJobConfig(JobConfigDto jobConfig)
        {
            if (string.IsNullOrEmpty(jobConfig.JobName))
            {
                throw new SpriteException("JobName必须填写");
            }
            if (string.IsNullOrEmpty(jobConfig.JobGroup))
            {
                jobConfig.JobGroup = "Default";
            }

            if(jobConfig.JobExecType == EJobExecType.Api && !string.IsNullOrEmpty(jobConfig.Params))
            {
                try
                {
                    var tempParams = JsonConvert.DeserializeObject<CallApiParam>(jobConfig.Params);
                }
                catch(Exception ex)
                {
                    throw new SpriteException($"api 方法参数传递错误,{ex.Message}");
                }
            }

            switch (jobConfig.TriggerType)
            {
                case ETriggerType.Simple:
                    if (!jobConfig.SimpleIntervalUnit.HasValue)
                    {
                        throw new SpriteException("简单执行类型Job的需要指定循环单位");
                    }
                    if (!jobConfig.SimpleIntervalValue.HasValue)
                    {
                        throw new SpriteException("简单执行类型Job的需要指定循环值");
                    }
                    break;
                case ETriggerType.Cron:
                    if (string.IsNullOrEmpty(jobConfig.CronConfig))
                    {
                        throw new SpriteException("Cron执行类型Job的CronConfig字段必须填写");
                    }
                    break;
                case ETriggerType.At:
                    if (!jobConfig.StartTime.HasValue)
                    {
                        throw new SpriteException("具体时间执行类型Job的需要指定执行时间");
                    }
                    break;
            }
        }
    }
}
