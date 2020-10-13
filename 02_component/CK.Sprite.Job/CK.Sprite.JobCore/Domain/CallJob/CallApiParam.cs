using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.JobCore
{
    public class CallApiParam
    {
        /// <summary>
        /// 请求类型，Post=1，只支持Post(使用者传递参数容易出错误)
        /// </summary>
        public EMethodType MethodType { get; set; } = EMethodType.Post;

        /// <summary>
        /// 执行参数
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        /// 请求地址host前缀类型，GateWay=1；如果ExecLocation带http前缀，则此参数无效
        /// </summary>
        public ECallLocationHostType CallLocationHostType { get; set; } = ECallLocationHostType.GateWay;
    }

    public class CallApiConfig
    {
        public string GateWayHostUrl { get; set; }
    }

    public enum EMethodType
    {
        Post = 1
    }

    public enum ECallLocationHostType
    {
        GateWay = 1
    }
}
