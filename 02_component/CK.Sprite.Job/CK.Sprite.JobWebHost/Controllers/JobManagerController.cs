using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CK.Sprite.JobCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CK.Sprite.JobWebHost.Controllers
{
    /// <summary>
    /// 定时作业
    /// </summary>
    [ApiController]
    [Area("job")]
    [ControllerName("JobManager")]
    [Route("api/job/JobManager")]
    public class JobManagerController:Controller, IJobManagerAppService
    {
        private readonly IJobManagerAppService _jobManagerAppService;
        public JobManagerController(IJobManagerAppService jobManagerAppService)
        {
            _jobManagerAppService = jobManagerAppService;
        }

        /// <summary>
        /// 激活job或取消激活job
        /// </summary>
        /// <param name="id">job id</param>
        /// <param name="isActive">是否激活</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ActiveJob")]
        public async Task ActiveJob(Guid id, bool isActive)
        {
            await _jobManagerAppService.ActiveJob(id, isActive);
        }

        [HttpPost]
        [Route("AddHolidayCalendar")]
        public async Task AddHolidayCalendar(HolidayCalendarDto holidayCalendar)
        {
            await _jobManagerAppService.AddHolidayCalendar(holidayCalendar);
        }

        [HttpPost]
        [Route("AddJob")]
        public async Task AddJob(JobConfigDto jobConfig)
        {
            await _jobManagerAppService.AddJob(jobConfig);
        }

        [HttpGet]
        [Route("DeleteJob")]
        public async Task DeleteJob(Guid id)
        {
            await _jobManagerAppService.DeleteJob(id);
        }

        [HttpGet]
        [Route("GetAllJobConfigs")]
        public async Task<List<JobConfig>> GetAllJobConfigs()
        {
            return await _jobManagerAppService.GetAllJobConfigs();
        }

        [HttpGet]
        [Route("GetAllQuartzJobConfigs")]
        public async Task<List<QuartzJobInfo>> GetAllQuartzJobConfigs()
        {
            return await _jobManagerAppService.GetAllQuartzJobConfigs();
        }

        [HttpGet]
        [Route("PauseAll")]
        public async Task PauseAll()
        {
            await _jobManagerAppService.PauseAll();
        }

        [HttpGet]
        [Route("PauseJob")]
        public async Task PauseJob(Guid id)
        {
            await _jobManagerAppService.PauseJob(id);
        }

        [HttpGet]
        [Route("ResumeAll")]
        public async Task ResumeAll()
        {
            await _jobManagerAppService.ResumeAll();
        }

        [HttpGet]
        [Route("ResumeJob")]
        public async Task ResumeJob(Guid id)
        {
            await _jobManagerAppService.ResumeJob(id);
        }

        [HttpGet]
        [Route("TriggerJob")]
        public async Task TriggerJob(Guid id)
        {
            await _jobManagerAppService.TriggerJob(id);
        }

        [HttpPost]
        [Route("UpdateHolidayCalendar")]
        public async Task UpdateHolidayCalendar(HolidayCalendarDto holidayCalendar)
        {
            await _jobManagerAppService.UpdateHolidayCalendar(holidayCalendar);
        }

        [HttpPost]
        [Route("UpdateJob")]
        public async Task UpdateJob(JobConfigDto jobConfig)
        {
            await _jobManagerAppService.UpdateJob(jobConfig);
        }
    }
}
