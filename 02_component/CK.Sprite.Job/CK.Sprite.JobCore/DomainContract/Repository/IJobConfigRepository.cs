using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CK.Sprite.Framework;

namespace CK.Sprite.JobCore
{
    public interface IJobConfigRepository : IGuidRepository<JobConfig>
    {
        /// <summary>
        /// 获取激活的Job配置
        /// </summary>
        /// <returns></returns>
        Task<List<JobConfig>> GetActiveJobConfigs();
    }
}
