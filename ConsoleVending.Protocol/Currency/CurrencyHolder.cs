using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleVending.Protocol.Enums;

namespace ConsoleVending.Protocol.Currency
{
    public class CurrencyHolder : ICurrencyHolder
    {
        private readonly Dictionary<Denomination, int> _currency;
        public uint TotalValue => (uint) _currency.Sum(kv => (int)kv.Key * kv.Value);

        public IReadOnlyDictionary<Denomination, int> Currency => _currency;

        public CurrencyHolder()
        {
            _currency = new Dictionary<Denomination, int>
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
        }
        public CurrencyHolder(ref Dictionary<Denomination, int> initialState) {
            _currency = initialState;
            foreach (var value in Enum.GetValues<Denomination>()) {
                var contains = _currency.TryGetValue(value, out var val);
                if (!contains) _currency[value] = val;
                else if (val < 0) _currency[value] = 0;
            }
        }
        
        public void AddCurrency(IReadOnlyTransaction transaction)
        {
            if (transaction.TotalValue == 0u) return;

            var deltas = Enum.GetValues<Denomination>()
                .Where(denomination => transaction.AmountOf(denomination) != 0)
                .Select(denomination => new { 
                    denomination, 
                    after = _currency[denomination] + transaction.AmountOf(denomination) 
                }).ToArray();
            //Reject if transaction would result in negative amount, ex: holder cannot have -10 of 1 pound
            if (deltas.Any(delta => delta.after < 0))
                throw new CurrencyOperationException($"Transaction would result in negative amount of denomination");
            
            foreach (var delta in deltas) {
                _currency[delta.denomination] = delta.after;
            }
        }

        public void RemoveCurrency(IReadOnlyTransaction transaction) {
            if (transaction.TotalValue == 0u) return;

            var deltas = Enum.GetValues<Denomination>()
                .Where(denomination => transaction.AmountOf(denomination) != 0)
                .Select(denomination => new { 
                    denomination, 
                    after = _currency[denomination] - transaction.AmountOf(denomination) 
                }).ToArray();
            //Reject if transaction would result in negative amount, ex: holder cannot have -10 of 1 pound
            if (deltas.Any(delta => delta.after < 0))
                throw new CurrencyOperationException($"Transaction would result in negative amount of denomination");
            
            foreach (var delta in deltas) {
                _currency[delta.denomination] = delta.after;
            }
        }
        
        public IReadOnlyTransaction? TransactionFor(uint cost)
        {
            //check if change is possible
            if (TotalValue < cost) return null;
            
            var remainder = (int) cost;
            var transaction = new Transaction();

            var current = new Dictionary<Denomination, int>(Currency.Where(kv => kv.Value > 0));

            while(remainder > 0){
                var distances = current.Where(kv => kv.Value > 0)
                    .Select(kv => new { 
                        denomination = kv.Key, 
                        distance = remainder - (int) kv.Key 
                    })
                    .Where(distance => distance.distance >= 0)
                    .ToArray();
                if (distances.Length == 0) break;
                var best = distances.Aggregate((minCandidate, distance) => 
                    distance.distance < minCandidate.distance ? distance : minCandidate);

                remainder -= (int) best.denomination;
                transaction.Push(best.denomination, 1);
                current[best.denomination] -= 1;
            }

            //if remainder is not zero, transaction is impossible
            if (remainder != 0) return null;
            //if reminader is zero, transaction has the expected value
            return transaction;

        }
    }
}
