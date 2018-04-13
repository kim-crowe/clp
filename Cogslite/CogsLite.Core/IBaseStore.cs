using System;

namespace CogsLite.Core
{
    public interface IBaseStore<T> where T : BaseObject
    {
        void Add(T item);
        void UpdateOne(Guid id, Action<T> updateAction);
    }
}
