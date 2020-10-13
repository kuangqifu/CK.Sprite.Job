using CK.Sprite.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CK.Sprite.JobCore
{
    /// <summary>
    /// 假期配置
    /// </summary>
    [Dapper.Contrib.Extensions.Table("HolidayCalendars")]
    public class HolidayCalendar : GuidEntityBase
    {
        /// <summary>
        /// job描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 假期值，用;号隔开
        /// </summary>
        public string Config { get; set; }
    }
}
