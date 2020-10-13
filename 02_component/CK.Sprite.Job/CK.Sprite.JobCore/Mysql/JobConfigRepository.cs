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
    public class JobConfigRepository : GuidRepositoryBase<JobConfig>, IJobConfigRepository
    {
        public JobConfigRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<List<JobConfig>> GetActiveJobConfigs()
        {
            string strSql = "SELECT * FROM JobConfigs WHERE IsActive=1;";
            var result = await _unitOfWork.Connection.QueryAsync<JobConfig>(strSql, transaction: _unitOfWork.Transaction);
            return result.ToList();
        }
    }
}
