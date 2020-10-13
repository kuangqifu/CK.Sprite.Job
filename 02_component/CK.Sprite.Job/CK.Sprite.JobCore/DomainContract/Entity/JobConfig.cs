using System;
using System.Collections.Generic;
using System.Text;
using CK.Sprite.Framework;

namespace CK.Sprite.JobCore
{
    /// <summary>
    /// Job配置
    /// </summary>
    [Dapper.Contrib.Extensions.Table("JobConfigs")]
    public class JobConfig : GuidEntityBase
    {
        /// <summary>
        /// Job分组，为空时，添加到默认组
        /// </summary>
        public string JobGroup { get; set; }
        /// <summary>
        /// job描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 触发器类型
        /// </summary>
        public ETriggerType TriggerType { get; set; }
        /// <summary>
        /// 触发器开始时间，触发器类型为At时，必须指定
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 触发器结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// Cron触发器配置
        /// </summary>
        public string CronConfig { get; set; }
        /// <summary>
        /// 简单触发器，循环单位
        /// </summary>
        public EIntervalUnit? SimpleIntervalUnit { get; set; }
        /// <summary>
        /// 简单触发器，循环值
        /// </summary>
        public int? SimpleIntervalValue { get; set; }
        /// <summary>
        /// 最多执行次数，不设置时，为一直执行
        /// </summary>
        public int? ExecCount { get; set; }
        /// <summary>
        /// 优先级(同一时间执行时生效)
        /// </summary>
        public int? Priority { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 假期配置Id
        /// </summary>
        public Guid? HolidayCalendarId { get; set; }
        /// <summary>
        /// Job名称
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// job执行参数
        /// </summary>
        public string Params { get; set; }
        /// <summary>
        /// job执行类型
        /// </summary>
        public EJobExecType JobExecType { get; set; }
        /// <summary>
        /// 用地址(api时为url，reflect时为类定义信息)
        /// </summary>
        public string ExecLocation { get; set; }
    }
}
