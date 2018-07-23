using System;
using System.Threading.Tasks;

namespace CogsLite.Core
{
    public interface IBaseStore<T> where T : BaseObject
    {
        Task Add(T item);
        void UpdateOne(Guid id, Action<T> updateAction);
    }
}
