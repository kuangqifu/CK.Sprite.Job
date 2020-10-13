using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CK.Sprite.JobBusiness
{
    public class TestExecJob : IReflectExecJob
    {
        public async Task Execute(string execParams)
        {
            Thread.Sleep(1000);
            await Task.CompletedTask;
        }
    }
}
