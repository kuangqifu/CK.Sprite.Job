using System;
using System.Collections.Generic;
using System.Text;
using CK.Sprite.Framework;

namespace CK.Sprite.JobCore
{
    /// <summary>
    /// 触发器类型
    /// </summary>
    public enum ETriggerType
    {
        /// <summary>
        /// 简单触发器，只处理简单时间间隔
        /// </summary>
        Simple = 1,
        /// <summary>
        /// 复杂触发器定义，参考(https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/crontrigger.html#introduction)
        /// </summary>
        Cron = 2,
        /// <summary>
        /// 具体时间点
        /// </summary>
        At = 3
    }

    /// <summary>
    /// 循环间隔单位
    /// </summary>
    public enum EIntervalUnit
    {
        Second = 1,
        Minute = 2,
        Hour = 3
    }

    /// <summary>
    /// job执行类型
    /// </summary>
    public enum EJobExecType
    {
        Api = 1,
        Reflect = 2
    }
}
