using CK.Sprite.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.JobCore
{
    public interface IJobManagerAppService : IAppService
    {
        /// <summary>
        /// 新增Job，直接加入调度
        /// </summary>
        /// <param name="jobConfig">job配置</param>
        /// <returns></returns>
        Task AddJob(JobConfigDto jobConfig);

        /// <summary>
        /// 修改job，先移除job调度，再添加job调度
        /// </summary>
        /// <param name="jobConfig"></param>
        /// <returns></returns>
        Task UpdateJob(JobConfigDto jobConfig);

        /// <summary>
        /// 删除job
        /// </summary>
        /// <param name="id">job id</param>
        /// <returns></returns>
        Task DeleteJob(Guid id);

        /// <summary>
        /// 激活job或取消激活job
        /// </summary>
        /// <param name="id">job id</param>
        /// <param name="isActive">是否激活</param>
        /// <returns></returns>
        Task ActiveJob(Guid id, bool isActive);

        /// <summary>
        /// 增加假期
        /// </summary>
        /// <param name="holidayCalendar">假期配置</param>
        /// <returns></returns>
        Task AddHolidayCalendar(HolidayCalendarDto holidayCalendar);

        /// <summary>
        /// 修改假期
        /// </summary>
        /// <param name="holidayCalendar">假期配置</param>
        /// <returns></returns>
        Task UpdateHolidayCalendar(HolidayCalendarDto holidayCalendar);

        /// <summary>
        /// 获取数据库所有job配置
        /// </summary>
        /// <returns></returns>
        Task<List<JobConfig>> GetAllJobConfigs();

        /// <summary>
        /// 获取运行中的job信息
        /// </summary>
        /// <returns></returns>
        Task<List<QuartzJobInfo>> GetAllQuartzJobConfigs();

        /// <summary>
        /// 立即执行job
        /// </summary>
        /// <param name="id">job id</param>
        /// <returns></returns>
        Task TriggerJob(Guid id);

        /// <summary>
        /// 暂停job
        /// </summary>
        /// <param name="id">job id</param>
        /// <returns></returns>
        Task PauseJob(Guid id);

        /// <summary>
        /// 暂停所有job
        /// </summary>
        /// <returns></returns>
        Task PauseAll();

        /// <summary>
        /// 恢复job
        /// </summary>
        /// <param name="id">job id</param>
        /// <returns></returns>
        Task ResumeJob(Guid id);

        /// <summary>
        /// 恢复所有job
        /// </summary>
        /// <returns></returns>
        Task ResumeAll();
    }
}
