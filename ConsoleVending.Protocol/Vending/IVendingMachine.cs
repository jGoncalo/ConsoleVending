using ConsoleVending.Protocol.Items;
using ConsoleVending.Protocol.Currency;

namespace ConsoleVending.Protocol.Vending
{

    public interface IVendingMachine
    {
        bool InMaintenance { get; }

        VendingTransaction AcquireItem(IReadOnlyTransaction transaction, uint itemCode);

        Item[] AvailableItems(bool includeSoldOut = false);

        uint ItemCost(uint itemCode);
        bool IsItemAvailable(uint itemCode);
    }
}