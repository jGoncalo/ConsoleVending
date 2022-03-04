using ConsoleVending.Protocol.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleVending.Tests
{

    public static class TestUtils
    {
        public static class Repo
        {
            private class MemRepo<TKey, TContent> : IRepository<TKey, TContent> where TKey : notnull
            {
                public readonly IDictionary<TKey, TContent> Dictionary;

                public MemRepo(ref Dictionary<TKey, TContent> dict)
                {
                    Dictionary = dict;
                }

                public TContent this[TKey key]
                {
                    get => Dictionary[key];
                    set => Dictionary[key] = value;
                }

                public bool TryGet(TKey key, out TContent? value) => Dictionary.TryGetValue(key, out value);
                public void Clear() => Dictionary.Clear();

                public IReadOnlyList<TContent> Contents => Dictionary.Values.ToList();
            }

            public static IRepository<TKey, TContent> InMemory<TKey, TContent>(
                ref Dictionary<TKey, TContent> dictionary) where TKey : notnull
                => new MemRepo<TKey, TContent>(ref dictionary);

        }
    }
}