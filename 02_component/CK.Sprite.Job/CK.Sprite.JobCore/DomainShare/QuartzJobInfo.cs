using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CK.Sprite.Framework;

namespace CK.Sprite.JobCore
{
    public class QuartzJobInfo
    {
         public string JobGroup { get; set; }
         public string JobName { get; set; }
         public string Description { get; set; }
         public string TriggerName { get; set; }
         public string StrTriggerState { get; set; }
         public string CalendarName { get; set; }
         public DateTime? PreviousFireTime { get; set; }
         public DateTime? NextFireTime { get; set; }
         public int Priority { get; set; }
    }
}
