using System.Collections.Generic;
using System.Linq;

namespace ConsoleVending.Protocol.Repository
{

    public class Repository<TKey, TContent> : IRepository<TKey, TContent> where TKey : notnull
    {
        private readonly IDictionary<TKey, TContent> _content = new Dictionary<TKey, TContent>();

        public void Clear()
        {
            _content.Clear();
        }

        public TContent this[TKey key]
        {
            get => _content[key];
            set => _content[key] = value;
        }

        public IReadOnlyList<TContent> Contents => _content.Values.ToList();

        public bool TryGet(TKey key, out TContent? value) => _content.TryGetValue(key, out value);
    }
}