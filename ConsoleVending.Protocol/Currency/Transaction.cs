using System.Linq;
using System.Collections.Generic;
using ConsoleVending.Protocol.Enums;

namespace ConsoleVending.Protocol.Currency
{

    public class Transaction : ITransaction
    {
        private readonly IDictionary<Denomination, int> _monetaryValues = new Dictionary<Denomination, int>
        {
            { Denomination.OnePenny, 0 },
            { Denomination.TwoPenny, 0 },
            { Denomination.FivePenny, 0 },
            { Denomination.TenPenny, 0 },
            { Denomination.TwentyPenny, 0 },
            { Denomination.FiftyPenny, 0 },
            { Denomination.OnePound, 0 },
            { Denomination.TwoPound, 0 }
        };

        public ITransaction Push(Denomination denomination, int amount)
        {
            _monetaryValues[denomination] += amount;
            return this;
        }

        public ITransaction Reset()
        {
            foreach (var key in _monetaryValues.Keys)
                _monetaryValues[key] = 0;
            return this;
        }

        public int AmountOf(Denomination denomination) => _monetaryValues[denomination];

        public int TotalValue => _monetaryValues.Sum((kv) => kv.Value * (int) kv.Key);
        public string TotalValueString => $"{TotalValue/100.0f:N2}£";
    }
}