using Dapper.Contrib.Extensions;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public class GuidRepositoryBase<T> : IGuidRepository<T> where T : class
    {
        public GuidRepositoryBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork _unitOfWork { get; private set; }

        public bool Delete(T obj)
        {
            return _unitOfWork.Connection.Delete(obj, _unitOfWork.Transaction);
        }

        public bool Delete(IEnumerable<T> list)
        {
            return _unitOfWork.Connection.Delete(list, _unitOfWork.Transaction);
        }

        public bool DeleteAll()
        {
            return _unitOfWork.Connection.DeleteAll<T>(_unitOfWork.Transaction);
        }

        public T Get(Guid id)
        {
            return _unitOfWork.Connection.Get<T>(id, _unitOfWork.Transaction);
        }

        public IEnumerable<T> GetAll()
        {
            return _unitOfWork.Connection.GetAll<T>(_unitOfWork.Transaction);
        }

        public long Insert(T obj)
        {
            return _unitOfWork.Connection.Insert<T>(obj, _unitOfWork.Transaction);
        }

        public long Insert(IEnumerable<T> list)
        {
            return _unitOfWork.Connection.Insert(list, _unitOfWork.Transaction);
        }

        public bool Update(T obj)
        {
            return _unitOfWork.Connection.Update(obj, _unitOfWork.Transaction);
        }

        public bool Update(IEnumerable<T> list)
        {
            return _unitOfWork.Connection.Update(list, _unitOfWork.Transaction);
        }

        public async Task<bool> DeleteAsync(T obj)
        {
            return await _unitOfWork.Connection.DeleteAsync(obj, _unitOfWork.Transaction);
        }

        public async Task<bool> DeleteAsync(IEnumerable<T> list)
        {
            return await _unitOfWork.Connection.DeleteAsync(list, _unitOfWork.Transaction);
        }

        public async Task<bool> DeleteAllAsync()
        {
            return await _unitOfWork.Connection.DeleteAllAsync<T>(_unitOfWork.Transaction);
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await _unitOfWork.Connection.GetAsync<T>(id, _unitOfWork.Transaction);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _unitOfWork.Connection.GetAllAsync<T>(_unitOfWork.Transaction);
        }

        public async Task<long> InsertAsync(T obj)
        {
            return await _unitOfWork.Connection.InsertAsync(obj, _unitOfWork.Transaction);
        }

        public async Task<long> InsertAsync(IEnumerable<T> list)
        {
            return await _unitOfWork.Connection.InsertAsync(list, _unitOfWork.Transaction);
        }

        public async Task<bool> UpdateAsync(T obj)
        {
            return await _unitOfWork.Connection.UpdateAsync(obj, _unitOfWork.Transaction);
        }

        public async Task<bool> UpdateAsync(IEnumerable<T> list)
        {
            return await _unitOfWork.Connection.UpdateAsync(list, _unitOfWork.Transaction);
        }
    }
}
