using Dapper;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CK.Sprite.Framework;

namespace CK.Sprite.JobCore
{
    public class HolidayCalendarRepository : GuidRepositoryBase<HolidayCalendar>, IHolidayCalendarRepository
    {
        public HolidayCalendarRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
