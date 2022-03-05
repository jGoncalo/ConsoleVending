using System;
using ConsoleVending.Protocol.Enums;

namespace ConsoleVending.Protocol.Currency
{

    public interface IReadOnlyTransaction
    {
        int TotalValue { get; }
        string TotalValueString { get; }
        int AmountOf(Denomination denomination);
    }

    public interface ITransaction : IReadOnlyTransaction
    {
        ITransaction Push(Denomination denomination, int amount);
        ITransaction Reset();
    }
}