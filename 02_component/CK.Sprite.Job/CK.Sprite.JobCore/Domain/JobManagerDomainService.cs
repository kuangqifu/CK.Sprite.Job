using CK.Sprite.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
    public class JobManagerDomainService : DomainService
    {
        private readonly JobManager _jobManager;
        public JobManagerDomainService(JobManager jobManager)
        {
            _jobManager = jobManager;
        }

        public async Task AddJob(JobConfig jobConfig)
        {
            jobConfig.Id = Guid.NewGuid();
            await _serviceProvider.DoDapperServiceAsync(DefaultDbConfig, async (unitOfWork) =>
            {
                IJobConfigRepository jobConfigRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IJobConfigRepository>(unitOfWork);
                await jobConfigRepository.InsertAsync(jobConfig);
                if(jobConfig.IsActive)
                {
                    await _jobManager.ScheduleJob(jobConfig);
                }
            });
            Logger.LogInformation($"AddJob:{JsonConvert.SerializeObject(jobConfig)}");
        }

        public async Task UpdateJob(JobConfig jobConfig)
        {
            await _serviceProvider.DoDapperServiceAsync(DefaultDbConfig, async (unitOfWork) =>
            {
                IJobConfigRepository jobConfigRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IJobConfigRepository>(unitOfWork);
                var dbJobConfig = await jobConfigRepository.GetAsync(jobConfig.Id);
                if(dbJobConfig == null)
                {
                    throw new SpriteException("未找到Job配置信息");
                }
                if (dbJobConfig.IsActive)
                {
                    await _jobManager.UnScheduleJob(dbJobConfig);
                }
                await jobConfigRepository.UpdateAsync(jobConfig);
                if (jobConfig.IsActive)
                {
                    await _jobManager.ScheduleJob(jobConfig);
                }
            });
            Logger.LogInformation($"UpdateJob:{JsonConvert.SerializeObject(jobConfig)}");
        }

        public async Task DeleteJob(Guid id)
        {
            await _serviceProvider.DoDapperServiceAsync(DefaultDbConfig, async (unitOfWork) =>
            {
                IJobConfigRepository jobConfigRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IJobConfigRepository>(unitOfWork);
                var dbJobConfig = await jobConfigRepository.GetAsync(id);
                await jobConfigRepository.DeleteAsync(dbJobConfig);
                if (dbJobConfig.IsActive)
                {
                    await _jobManager.UnScheduleJob(dbJobConfig);
                }
            });
            Logger.LogInformation($"DeleteJob:{id}");
        }

        public async Task ActiveJob(Guid id, bool isActive)
        {
            await _serviceProvider.DoDapperServiceAsync(DefaultDbConfig, async (unitOfWork) =>
            {
                IJobConfigRepository jobConfigRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IJobConfigRepository>(unitOfWork);
                var dbJobConfig = await jobConfigRepository.GetAsync(id);
                if (dbJobConfig.IsActive == isActive)
                {
                    throw new SpriteException($"job已经处于{(isActive ? "激活" : "未激活")}状态");
                }
                dbJobConfig.IsActive = isActive;
                await jobConfigRepository.UpdateAsync(dbJobConfig);
                if (isActive)
                {
                    await _jobManager.ScheduleJob(dbJobConfig);
                }
                else
                {
                    await _jobManager.UnScheduleJob(dbJobConfig);
                }
            });
            Logger.LogInformation($"ActiveJob:{id},{isActive}");
        }

        public async Task AddHolidayCalendar(HolidayCalendar holidayCalendar)
        {
            holidayCalendar.Id = Guid.NewGuid();
            await _serviceProvider.DoDapperServiceAsync(DefaultDbConfig, async (unitOfWork) =>
            {
                IHolidayCalendarRepository holidayCalendarRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IHolidayCalendarRepository>(unitOfWork);
                await holidayCalendarRepository.InsertAsync(holidayCalendar);
            });
            Logger.LogInformation($"AddHolidayCalendar:{JsonConvert.SerializeObject(holidayCalendar)}");
        }

        public async Task UpdateHolidayCalendar(HolidayCalendar holidayCalendar)
        {
            await _serviceProvider.DoDapperServiceAsync(DefaultDbConfig, async (unitOfWork) =>
            {
                IHolidayCalendarRepository holidayCalendarRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IHolidayCalendarRepository>(unitOfWork);
                IJobConfigRepository jobConfigRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IJobConfigRepository>(unitOfWork);
                var dbHolidayCalendar = await holidayCalendarRepository.GetAsync(holidayCalendar.Id);
                if (dbHolidayCalendar == null)
                {
                    throw new SpriteException("未找到假期信息");
                }
                await holidayCalendarRepository.UpdateAsync(holidayCalendar);
                var activeJobConfigs = await jobConfigRepository.GetActiveJobConfigs();
                foreach (var activeJobConfig in activeJobConfigs)
                {
                    if (activeJobConfig.HolidayCalendarId.HasValue && activeJobConfig.HolidayCalendarId.Value == holidayCalendar.Id)
                    {
                        await _jobManager.UnScheduleJob(activeJobConfig);
                    }
                }

                await _jobManager.DeleteCalendar(holidayCalendar.Id);

                await _jobManager.AddCalendar(holidayCalendar);

                foreach (var activeJobConfig in activeJobConfigs)
                {
                    if (activeJobConfig.HolidayCalendarId.HasValue && activeJobConfig.HolidayCalendarId.Value == holidayCalendar.Id)
                    {
                        await _jobManager.ScheduleJob(activeJobConfig);
                    }
                }
            });
            Logger.LogInformation($"UpdateHolidayCalendar:{JsonConvert.SerializeObject(holidayCalendar)}");
        }

        public async Task<List<JobConfig>> GetAllJobConfigs()
        {
            return await _serviceProvider.DoDapperServiceAsync(DefaultDbConfig, async (unitOfWork) =>
            {
                IJobConfigRepository jobConfigRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IJobConfigRepository>(unitOfWork);
                var results = await jobConfigRepository.GetAllAsync();
                return results.ToList();
            });
        }

        public async Task<List<QuartzJobInfo>> GetAllQuartzJobConfigs()
        {
            return await _jobManager.GetAllQuartzJobConfigs();
        }

        public async Task TriggerJob(Guid id)
        {
            var jobConfig = await _serviceProvider.DoDapperServiceAsync(DefaultDbConfig, async (unitOfWork) =>
            {
                IJobConfigRepository jobConfigRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IJobConfigRepository>(unitOfWork);
                var result = await jobConfigRepository.GetAsync(id);
                return result;
            });
            await _jobManager.TriggerJob(jobConfig);
            Logger.LogInformation($"TriggerJob:{id}");
        }

        public async Task PauseJob(Guid id)
        {
            var jobConfig = await _serviceProvider.DoDapperServiceAsync(DefaultDbConfig, async (unitOfWork) =>
            {
                IJobConfigRepository jobConfigRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IJobConfigRepository>(unitOfWork);
                var result = await jobConfigRepository.GetAsync(id);
                return result;
            });
            await _jobManager.PauseJob(jobConfig);
            Logger.LogInformation($"PauseJob:{id}");
        }

        public async Task PauseAll()
        {
            await _jobManager.PauseAll();
            Logger.LogInformation($"PauseAll");
        }

        public async Task ResumeJob(Guid id)
        {
            var jobConfig = await _serviceProvider.DoDapperServiceAsync(DefaultDbConfig, async (unitOfWork) =>
            {
                IJobConfigRepository jobConfigRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IJobConfigRepository>(unitOfWork);
                var result = await jobConfigRepository.GetAsync(id);
                return result;
            });
            await _jobManager.ResumeJob(jobConfig);
            Logger.LogInformation($"ResumeJob:{id}");
        }

        public async Task ResumeAll()
        {
            await _jobManager.ResumeAll();
            Logger.LogInformation($"ResumeAll");
        }
    }
}
