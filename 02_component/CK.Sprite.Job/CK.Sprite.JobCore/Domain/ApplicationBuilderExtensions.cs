using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using CK.Sprite.JobCore;
using Microsoft.Extensions.DependencyInjection;
using CK.Sprite.Framework;

namespace System
{
    public static class ApplicationJobManagerExtensions
    {
        public static void UseQuartzJob(this IApplicationBuilder app)
        {
            AsyncHelper.RunSync(
                () => app.ApplicationServices
                    .GetRequiredService<JobManager>()
                    .StartAsync()
            );
        }
    }
}
