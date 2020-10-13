using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CK.Sprite.JobBusiness
{
    public interface IReflectExecJob
    {
        Task Execute(string execParams);
    }
}
