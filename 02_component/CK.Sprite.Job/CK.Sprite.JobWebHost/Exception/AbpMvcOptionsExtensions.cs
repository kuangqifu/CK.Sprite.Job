using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CK.Sprite.JobWebHost
{
    internal static class XhyzMvcOptionsExtensions
    {
        public static void AddException(this MvcOptions options, IServiceCollection services)
        {
            AddFilters(options);
        }

        private static void AddFilters(MvcOptions options)
        {
            options.Filters.AddService(typeof(AbpExceptionFilter));
        }
    }
}