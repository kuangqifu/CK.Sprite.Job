using AutoMapper;
using CK.Sprite.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
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
    public class JobManager : ISingletonDependency
    {
        private IScheduler _scheduler;
        #region common config

        protected IServiceProvider _serviceProvider => ServiceLocator.ServiceProvider;
        protected readonly object ServiceProviderLock = new object();

        protected TService LazyGetRequiredService<TService>(ref TService reference)
            => LazyGetRequiredService(typeof(TService), ref reference);

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)_serviceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        public IMapper Mapper => LazyGetRequiredService(ref _mapper);
        private IMapper _mapper;

        public DefaultDbConfig DefaultDbConfig => LazyGetRequiredService(ref _DefaultDbConfig).Value;

        public ILogger<DomainService> Logger => LazyGetRequiredService(ref _logger);
        private ILogger<DomainService> _logger;

        public QuartzJobListener QuartzJobListener => LazyGetRequiredService(ref _quartzJobListener);
        private QuartzJobListener _quartzJobListener;

        private IOptions<DefaultDbConfig> _DefaultDbConfig;

        #endregion
        public async Task StartAsync()
        {
            _scheduler = await new StdSchedulerFactory().GetScheduler();
            await _serviceProvider.DoDapperServiceAsync(DefaultDbConfig, async (unitOfWork) =>
            {
                IJobConfigRepository jobConfigRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IJobConfigRepository>(unitOfWork);
                IHolidayCalendarRepository holidayCalendarRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IHolidayCalendarRepository>(unitOfWork);
                var activeJobConfigs = await jobConfigRepository.GetActiveJobConfigs();
                List<HolidayCalendar> holidayCalendars = new List<HolidayCalendar>();
                if (activeJobConfigs.Any(r => r.HolidayCalendarId.HasValue))
                {
                    holidayCalendars = (await holidayCalendarRepository.GetAllAsync()).ToList();
                    Quartz.Impl.Calendar.HolidayCalendar calendar = new Quartz.Impl.Calendar.HolidayCalendar();
                    foreach (var holidayCalendar in holidayCalendars)
                    {
                        if (activeJobConfigs.Any(r => r.HolidayCalendarId == holidayCalendar.Id))
                        {
                            await AddCalendar(holidayCalendar);
                        }
                    }
                }
                foreach (var activeJobConfig in activeJobConfigs)
                {
                    IJobDetail job = CreateJobDetail(activeJobConfig);
                    ITrigger trigger = CreateJobTriggerAsync(activeJobConfig, holidayCalendars);
                    await _scheduler.ScheduleJob(job, trigger);
                }
                _scheduler.ListenerManager.AddJobListener(QuartzJobListener, GroupMatcher<JobKey>.AnyGroup());
                await _scheduler.Start();
            });
        }

        // 添加Job调度（如果job已经存在，抛出异常）
        public async Task ScheduleJob(JobConfig jobConfig)
        {
            if (await _scheduler.CheckExists(new JobKey(GetJobName(jobConfig), GetJobGroupName(jobConfig)))) // job是否已经存在
            {
                throw new SpriteException("job 已经存在");
            }

            // 判断假期
            if (jobConfig.HolidayCalendarId.HasValue)
            {
                if (!(await _scheduler.GetCalendarNames()).Any(r => r.Contains(jobConfig.HolidayCalendarId.ToString())))
                {
                    await _serviceProvider.DoDapperServiceAsync(DefaultDbConfig, async (unitOfWork) =>
                    {
                        IHolidayCalendarRepository holidayCalendarRepository = ConnectionFactory.GetConnectionProvider(DefaultDbConfig.ConnectionType).GetRepository<IHolidayCalendarRepository>(unitOfWork);

                        var holidayCalendar = await holidayCalendarRepository.GetAsync(jobConfig.HolidayCalendarId.Value);
                        if (holidayCalendar == null)
                        {
                            throw new SpriteException("未找到假期配置信息");
                        }

                        await AddCalendar(holidayCalendar);
                    });
                }
            }

            IJobDetail job = CreateJobDetail(jobConfig);
            ITrigger trigger = CreateJobTriggerAsync(jobConfig, null);
            await _scheduler.ScheduleJob(job, trigger);
        }

        public async Task UnScheduleJob(JobConfig jobConfig)
        {
            if (await _scheduler.CheckExists(new JobKey(GetJobName(jobConfig), GetJobGroupName(jobConfig)))) // job是否已经存在，删除
            {
                await _scheduler.DeleteJob(new JobKey(GetJobName(jobConfig), GetJobGroupName(jobConfig)));
            }
            else
            {
                throw new SpriteException("job 不存在");
            }
        }

        public async Task TriggerJob(JobConfig jobConfig)
        {
            await DoSchedulerMethod((jobKey) =>
            {
                return _scheduler.TriggerJob(jobKey);
            }, jobConfig);
        }

        public async Task PauseJob(JobConfig jobConfig)
        {
            await DoSchedulerMethod((jobKey) =>
            {
                return _scheduler.PauseJob(jobKey);
            }, jobConfig);
        }

        public async Task PauseAll()
        {
            await _scheduler.PauseAll();
        }

        public async Task ResumeJob(JobConfig jobConfig)
        {
            await DoSchedulerMethod((jobKey) =>
            {
                return _scheduler.ResumeJob(jobKey);
            }, jobConfig);
        }

        public async Task ResumeAll()
        {
            await _scheduler.ResumeAll();
        }

        public async Task DeleteCalendar(Guid holidayCalendarId)
        {
            await _scheduler.DeleteCalendar($"{holidayCalendarId}_calendar");
        }
        
        public async Task AddCalendar(HolidayCalendar holidayCalendar)
        {
            var holidayDates = holidayCalendar.Config.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(r => DateTime.Parse(r));
            Quartz.Impl.Calendar.HolidayCalendar calendar = new Quartz.Impl.Calendar.HolidayCalendar();
            foreach (var holidayDate in holidayDates)
            {
                calendar.AddExcludedDate(holidayDate);
            }
            await _scheduler.AddCalendar($"{holidayCalendar.Id}_calendar", calendar, false, false);
        }

        public async Task<List<QuartzJobInfo>> GetAllQuartzJobConfigs()
        {
            List<QuartzJobInfo> quartzJobInfos = new List<QuartzJobInfo>();
            var allJobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            foreach (JobKey jobKey in allJobKeys)
            {
                var detail = await _scheduler.GetJobDetail(jobKey);
                 var triggers = await _scheduler.GetTriggersOfJob(jobKey);
                var temptrigger = triggers.FirstOrDefault();

                quartzJobInfos.Add(new QuartzJobInfo()
                {
                    JobGroup = jobKey.Group,
                    JobName = jobKey.Name,
                    Description = detail.Description,
                    TriggerName = temptrigger.Key.Name,
                    StrTriggerState = _scheduler.GetTriggerState(temptrigger.Key).ToString(),
                    NextFireTime = temptrigger.GetNextFireTimeUtc()?.LocalDateTime,
                    PreviousFireTime = temptrigger.GetPreviousFireTimeUtc()?.LocalDateTime,
                    CalendarName = temptrigger.CalendarName,
                    Priority = temptrigger.Priority
                });
            }

            return quartzJobInfos;
        }

        #region private Method

        private string GetJobName(JobConfig jobConfig, bool isJobName = true)
        {
            if (isJobName)
            {
                return $"{jobConfig.Id}_{jobConfig.JobName}_job";
            }
            else
            {
                return $"{jobConfig.Id}_{jobConfig.JobName}_trigger";
            }
        }

        private string GetJobGroupName(JobConfig jobConfig)
        {
            var jobGroupName = jobConfig.JobGroup;
            if (string.IsNullOrEmpty(jobConfig.JobGroup))
            {
                jobGroupName = "Default";
            }

            return jobGroupName;
        }

        private IJobDetail CreateJobDetail(JobConfig jobConfig)
        {
            JobBuilder jobBuilder;
            switch (jobConfig.JobExecType)
            {
                case EJobExecType.Api:
                    jobBuilder = JobBuilder.Create<CallApiJob>();
                    break;
                case EJobExecType.Reflect:
                    jobBuilder = JobBuilder.Create<CallReflectJob>();
                    break;
                default:
                    jobBuilder = JobBuilder.Create<CallReflectJob>();
                    break;
            }
            jobBuilder.WithIdentity(GetJobName(jobConfig), GetJobGroupName(jobConfig));
            if (!string.IsNullOrEmpty(jobConfig.Description))
            {
                jobBuilder.WithDescription(jobConfig.Description);
            }

            jobBuilder.UsingJobData("Params", jobConfig.Params);
            jobBuilder.UsingJobData("ExecLocation", jobConfig.ExecLocation);

            IJobDetail job = jobBuilder.Build();
            return job;
        }

        private ITrigger CreateJobTriggerAsync(JobConfig jobConfig, List<HolidayCalendar> holidayCalendars)
        {
            var nowTime = DateTime.Now;
            var triggerBuilder = TriggerBuilder.Create();
            triggerBuilder.WithIdentity(GetJobName(jobConfig, false), GetJobGroupName(jobConfig));

            // 设置开始时间
            if (jobConfig.StartTime.HasValue)
            {
                triggerBuilder.StartAt(jobConfig.StartTime.Value);
            }
            else
            {
                triggerBuilder.StartNow();
            }

            // 设置结束时间
            if (jobConfig.EndTime.HasValue)
            {
                if (jobConfig.EndTime.Value < nowTime.AddSeconds(10))
                {
                    Logger.LogWarning($"{GetJobName(jobConfig)}小于启动时间，Job未执行调度");
                    return null;
                }
                triggerBuilder.EndAt(jobConfig.EndTime.Value);
            }

            // 设置假期
            if (jobConfig.HolidayCalendarId.HasValue)
            {
                if (holidayCalendars == null)
                {
                    triggerBuilder.ModifiedByCalendar($"{jobConfig.HolidayCalendarId.Value}_calendar");
                }
                else
                {
                    var holidayCalendar = holidayCalendars.FirstOrDefault(r => r.Id == jobConfig.HolidayCalendarId.Value);
                    if (holidayCalendar == null)
                    {
                        Logger.LogWarning($"{GetJobName(jobConfig)}设置了假期，但未找到假期数据");
                    }
                    else
                    {
                        triggerBuilder.ModifiedByCalendar($"{jobConfig.HolidayCalendarId.Value}_calendar");
                    }
                }
            }

            // 优先级
            if (jobConfig.Priority.HasValue)
            {
                triggerBuilder.WithPriority(jobConfig.Priority.Value);
            }

            switch (jobConfig.TriggerType)
            {
                case ETriggerType.Simple:
                    if (!jobConfig.SimpleIntervalUnit.HasValue || !jobConfig.SimpleIntervalValue.HasValue)
                    {
                        Logger.LogWarning($"{GetJobName(jobConfig)}的简单作业，未设置执行参数");
                        throw new Exception($"{GetJobName(jobConfig)}的简单作业，未设置执行参数");
                    }
                    switch (jobConfig.SimpleIntervalUnit.Value)
                    {
                        case EIntervalUnit.Second:
                            triggerBuilder.WithSimpleSchedule(x =>
                            {
                                x.WithIntervalInSeconds(jobConfig.SimpleIntervalValue.Value);
                                SetSimpleRepeatCount(jobConfig, x);
                            });
                            break;
                        case EIntervalUnit.Minute:
                            triggerBuilder.WithSimpleSchedule(x =>
                            {
                                x.WithIntervalInMinutes(jobConfig.SimpleIntervalValue.Value);
                            });
                            break;
                        case EIntervalUnit.Hour:
                            triggerBuilder.WithSimpleSchedule(x =>
                            {
                                x.WithIntervalInHours(jobConfig.SimpleIntervalValue.Value);
                                SetSimpleRepeatCount(jobConfig, x);
                            });
                            break;
                    }
                    break;
                case ETriggerType.Cron:
                    triggerBuilder.WithCronSchedule(jobConfig.CronConfig);
                    break;
                case ETriggerType.At:
                    break;
            }

            return triggerBuilder.Build();
        }

        private void SetSimpleRepeatCount(JobConfig jobConfig, SimpleScheduleBuilder simpleScheduleBuilder)
        {
            if (jobConfig.ExecCount.HasValue)
            {
                simpleScheduleBuilder.WithRepeatCount(jobConfig.ExecCount.Value);
            }
            else
            {
                simpleScheduleBuilder.RepeatForever();
            }
        }

        public async Task DoSchedulerMethod(Func<JobKey, Task> func, JobConfig jobConfig)
        {
            var jobKey = new JobKey(GetJobName(jobConfig), GetJobGroupName(jobConfig));
            if (await _scheduler.CheckExists(jobKey)) // job是否已经存在，删除
            {
                await func(jobKey);
            }
            else
            {
                throw new SpriteException("job 不存在");
            }
        }

        #endregion
    }
}
