using System.Collections.Generic;

namespace ConsoleVending.Protocol.Repository
{

    public interface IReadOnlyRepository<TKey, TContent>
        where TKey : notnull
    {
        TContent this[TKey key] { get; }
        
        IReadOnlyList<TContent> Contents { get; }
        
        bool TryGet(TKey key, out TContent? value);
    }

    public interface IRepository<TKey, TContent>
        : IReadOnlyRepository<TKey, TContent> where TKey : notnull
    {
        void Clear();
        new TContent this[TKey key] { get; set; }
    }
}