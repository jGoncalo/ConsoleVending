using System;
using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Items;

namespace ConsoleVending.Protocol.Vending
{

    public class VendingMachine : IVendingMachine
    {
        public bool InMaintenance { get; }

        public VendingTransaction AcquireItem(IReadOnlyTransaction transaction, uint itemCode) =>
            throw new NotImplementedException();

        public Item[] AvailableItems(bool includeSoldOut = false) => throw new NotImplementedException();

        public uint ItemCost(uint itemCode) => throw new NotImplementedException();
        public bool IsItemAvailable(uint itemCode) => throw new NotImplementedException();
    }
}