using AutoMapper;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.JobCore
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<JobConfig, JobConfigDto>(MemberList.None);
            CreateMap<JobConfigDto, JobConfig>(MemberList.None);

            CreateMap<HolidayCalendar, HolidayCalendarDto>(MemberList.None);
            CreateMap<HolidayCalendarDto, HolidayCalendar>(MemberList.None);
        }
    }
}
