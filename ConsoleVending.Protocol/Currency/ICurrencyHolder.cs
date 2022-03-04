using System.Collections.Generic;
using ConsoleVending.Protocol.Enums;

namespace ConsoleVending.Protocol.Currency
{

    public interface ICurrencyHolder
    {
        uint TotalValue { get; }

        IReadOnlyDictionary<Denomination, int> Currency { get; }

        void AddCurrency(IReadOnlyTransaction transaction);
        void RemoveCurrency(IReadOnlyTransaction transaction);

        IReadOnlyTransaction? TransactionFor(uint cost);
    }
}