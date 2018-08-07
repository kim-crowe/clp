using System;
using System.Threading.Tasks;

namespace CogsLite.Core
{
    public interface IBaseStore<T> where T : BaseObject
    {
        Task Add(T item);
        Task UpdateOne(Guid hashKey, Guid rangeKey, Action<T> updateAction);
    }
}
