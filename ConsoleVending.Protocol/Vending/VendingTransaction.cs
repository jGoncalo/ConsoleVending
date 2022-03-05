using ConsoleVending.Protocol.Currency;
using ConsoleVending.Protocol.Items;

namespace ConsoleVending.Protocol.Vending
{

    public struct VendingTransaction
    {
        public readonly Item? Item;
        public readonly IReadOnlyTransaction? Transaction;

        public VendingTransaction(Item? item, IReadOnlyTransaction? transaction)
        {
            Item = item;
            Transaction = transaction;
        }
    }
}