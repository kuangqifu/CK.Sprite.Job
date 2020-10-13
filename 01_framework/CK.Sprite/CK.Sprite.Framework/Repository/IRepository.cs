using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public interface IRepository {
        IUnitOfWork _unitOfWork { get; }
    }

    public interface IRepository<T, TKeyType> : IRepository
        where T : class
    {
        T Get(TKeyType id);
        IEnumerable<T> GetAll();
        long Insert(T obj);
        long Insert(IEnumerable<T> list);
        bool Update(T obj);
        bool Update(IEnumerable<T> list);
        bool Delete(T obj);
        bool Delete(IEnumerable<T> list);
        bool DeleteAll();
        Task<T> GetAsync(TKeyType id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<long> InsertAsync(T obj);
        Task<long> InsertAsync(IEnumerable<T> list);
        Task<bool> UpdateAsync(T obj);
        Task<bool> UpdateAsync(IEnumerable<T> list);
        Task<bool> DeleteAsync(T obj);
        Task<bool> DeleteAsync(IEnumerable<T> list);
        Task<bool> DeleteAllAsync();
    }

    public interface IGuidRepository<T> : IRepository<T, Guid> where T : class
    {
    }
}
